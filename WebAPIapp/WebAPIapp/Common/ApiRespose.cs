using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Common
{
    public class ApiRespose<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
}
