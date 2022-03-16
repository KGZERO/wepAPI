using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Common
{
    public class ApiErrorResult:ApiRespose
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
