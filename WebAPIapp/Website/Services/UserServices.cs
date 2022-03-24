using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPIapp.Common;
using WebAPIapp.Model;

namespace Website.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
      
        public UserServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
          
        }
        public async Task<TokenModel> Authenticate(LoginVm request)
        {
           var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/user/login", httpContent);
            var token = await response.Content.ReadAsAsync<TokenModel>();
            return token;


        }
        public async Task<ApiRespose> RefreshToken(TokenModel token)
        {

            
            var json = JsonConvert.SerializeObject(token);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/user/refreshToken", httpContent);
            var tokens = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode)
                return new ApiSuccessResult(tokens);

            return null;



        }

    }
}
