using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;

namespace WebAPIapp.Common
{
    public class ApiSuccessResult : ApiRespose
    {
        public ApiSuccessResult(object resultObj)
        {
            ResultObj = resultObj;
            Success = true;
        }
        public ApiSuccessResult()
        {
            Success = true;
        }
    }
}
