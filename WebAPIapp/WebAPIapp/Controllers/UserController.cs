using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using WebAPIapp.Services;

namespace WebAPIapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserResponsitory _user;

        public UserController(IUserResponsitory user)
        {
            _user = user;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVm login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var logins = await _user.LoginUser(login);
            if (string.IsNullOrEmpty(logins.ResultObj))
            {
                return BadRequest(logins);
            }
            return Ok(logins);
        }
    }
}
