using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; } = 500;

        public AppException(string message) : base(message) { }
    }
}
