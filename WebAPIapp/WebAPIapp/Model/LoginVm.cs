﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Model
{
    public class LoginVm: TokenModel 
    {
        
        
        public string UserName { get; set; }
        public string Password { get; set; }
      
        public string RememberMe { get; set; }
        public string Email { get; set; }

    }
}
