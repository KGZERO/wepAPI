using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIapp.Common;
using WebAPIapp.Data;
using WebAPIapp.Model;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace WebAPIapp.Services
{
    public class UserResponsitory : IUserResponsitory
    {
       
        private readonly MyDbContext _context;
        private readonly AppSetting _appSetting;
       
        public UserResponsitory(MyDbContext context, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _appSetting = optionsMonitor.CurrentValue;
           
        }
        public async Task<ApiRespose> LoginUser(LoginVm login)
        {
            var user = _context.NguoiDungs.SingleOrDefault(x => x.UserName == login.UserName
             && x.Password == login.Password);
            if (user == null)
            {
                return new ApiErrorResult("Tài khoản không tồn tại");
            }
            var token = await GenarateToken(user);
            return new ApiSuccessResult(token);

        }
        private async Task<TokenModel> GenarateToken(NguoiDung nguoiDung)
        {
            var jwtTokenHanler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),
                    
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHanler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHanler.WriteToken(token);
            var refreshtoken = GenerateRefreshToken(25) + Guid.NewGuid();
            var refreshTokenEntity = new RefreshToken()
            {
               Id=Guid.NewGuid(),
                JwtId = token.Id,
                UserId=nguoiDung.Id,
                Token=refreshtoken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1),

            };
             await _context.AddAsync(refreshTokenEntity);
           await _context.SaveChangesAsync();
            var tokenModels=  new TokenModel
            {
                AccessToken = accessToken,
               RefreshToken = refreshtoken
            };
            return  tokenModels;
        }

        private string GenerateRefreshToken(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<ApiRespose> VerifyToken(TokenModel tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var refreshTokenValidationParams = new TokenValidationParameters
            {

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
            };
            try
            {
                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.AccessToken, refreshTokenValidationParams, out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new ApiErrorResult("We cannot refresh this since the token has not expired");
                    
                }

                // Check the token we got if its saved in the db
                var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new ApiErrorResult("refresh token doesnt exist");
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiredAt)
                {
                    return new ApiErrorResult("token has expired, user needs to relogin");
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return new ApiErrorResult("token has expired, user needs to relogin");
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return new ApiErrorResult("token has been used");
                    
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return new ApiErrorResult("the token doenst mateched the saved token");
                    
                }

                storedRefreshToken.IsUsed = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                var user =  _context.NguoiDungs.SingleOrDefault(nd => nd.Id == storedRefreshToken.UserId);
                var token=await GenarateToken(user);
                return new ApiSuccessResult(token);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult("Something went wrong");
            }
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}

