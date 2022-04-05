﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IMailService _emailSender;

        public UserController(IUserResponsitory user,IMailService emailSender)
        {
            _user = user;
            _emailSender = emailSender;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginVm login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var logins = await _user.LoginUser(login);
            if (logins==null)
                return BadRequest(logins);

            return Ok(logins);
        }
        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var res = await _user.VerifyToken(tokenRequest);

            if (res == null)
            {
                return BadRequest("null");
            }

            return Ok(res);


        }
       

       
    }
}
