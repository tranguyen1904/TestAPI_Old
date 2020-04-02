using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Controllers
{
    public static class ErrorMessage
    {
        public const string ServerError = "Internal Server Error";
        public const string ModelInvalid = "Invalid model object";
        public static string ObjectNull(string name)
        {
            return name + " object is null.";
        }
    }
}
