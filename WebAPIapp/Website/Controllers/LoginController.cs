using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIapp.Model;
using Website.Services;

namespace Website.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly AppSetting _appSetting;
       
        //private readonly IEmailSender _emailSender;
        public LoginController(IUserServices userServices, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _userServices = userServices;
            _appSetting = optionsMonitor.CurrentValue;
          
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm request)
        {

            if (!ModelState.IsValid)
                return View(ModelState);
            var tokenModel = await _userServices.Authenticate(request);
            var userPrincipal = await _userServices.RefreshToken(tokenModel);
            if (userPrincipal == null)
                return null;

            var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, request.UserName),
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            return View(forgotPasswordModel);
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
      
    
    private ClaimsPrincipal GetPrincipal(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            SecurityToken validatedToken;
            var refreshTokenValidationParams = new TokenValidationParameters
            {

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = false,
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, refreshTokenValidationParams, out validatedToken);

            return principal;
        }

    }
}
