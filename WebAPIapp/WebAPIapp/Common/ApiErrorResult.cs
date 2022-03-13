using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Common
{
    public class ApiErrorResult<T>:ApiRespose<T>
    {
        public string[] ValidationError { get; set; }
        public ApiErrorResult(string message)
        {
            Success = false;
            Message = message;

        }
        public ApiErrorResult(string[] validationError)
        {
            Success = false;
            validationError = ValidationError;
        }
    }
}
