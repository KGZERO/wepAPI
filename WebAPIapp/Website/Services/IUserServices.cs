using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPIapp.Common;
using WebAPIapp.Model;

namespace Website.Services
{
    public interface IUserServices
    {
        Task<TokenModel> Authenticate(LoginVm login);
        Task<ApiRespose> RefreshToken(TokenModel token);



    }
}
