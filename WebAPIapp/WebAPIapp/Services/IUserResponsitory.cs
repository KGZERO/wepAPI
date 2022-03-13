using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Common;
using WebAPIapp.Model;

namespace WebAPIapp.Services
{
    public  interface IUserResponsitory
    {
        Task<ApiRespose<string>> LoginUser(LoginVm login);
    }
}
