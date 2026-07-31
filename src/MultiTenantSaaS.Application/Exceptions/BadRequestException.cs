using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
}
