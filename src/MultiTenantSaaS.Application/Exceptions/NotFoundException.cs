using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message)
        {
            StatusCode = 404;
        }
    }
}
