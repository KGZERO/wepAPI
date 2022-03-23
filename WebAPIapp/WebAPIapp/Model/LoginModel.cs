﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Model
{
    public class LoginModel: AbstractValidator<LoginVm>
    {
       
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string PassWord { get; set; }
        [Required]
        [MaxLength(50)]
        public string ConfirmPassWord { get; set; }
    }
}
