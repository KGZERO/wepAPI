using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;

namespace WebAPIapp.Common
{
    public class ApiRespose
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public object ResultObj { get; set; }
        



    }
}
