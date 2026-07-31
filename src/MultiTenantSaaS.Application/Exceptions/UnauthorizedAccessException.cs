using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Exceptions
{
    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException(string message) : base(message)
        {
            StatusCode = 401;
        }
    }
}
