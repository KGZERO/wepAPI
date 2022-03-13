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
        public async Task<ApiRespose<string>> LoginUser(LoginVm login)
        {
            var user = _context.NguoiDungs.SingleOrDefault(x => x.UserName == login.UserName
             && x.Password == login.Password);
            if (user == null)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại") ;
            }
            return new ApiSuccessResult<string>(GenarateToken(user));

        }
        public string GenarateToken(NguoiDung nguoiDung)
        {
            var jwtTokenHanler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, nguoiDung.Email),
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHanler.CreateToken(tokenDescriptor);
            return jwtTokenHanler.WriteToken(token);


        }
    }
}
