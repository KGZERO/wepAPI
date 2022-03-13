using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Common
{
    public class ApiSuccessResult<T>:ApiRespose<T>
    {

        public ApiSuccessResult(T resultObj)
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
