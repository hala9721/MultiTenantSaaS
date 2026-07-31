using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantSaaS.Application.Services
{
    public interface IMultiTenantContext
    {
        Guid CurrentTenantId { get; }
        Guid CurrentUserId { get; }
    }
}
