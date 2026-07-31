using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace MultiTenantSaaS.Application.Services
{
    public class MultiTenantContext : IMultiTenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultiTenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CurrentTenantId
        {
            get
            {
                var tenantIdClaim = _httpContextAccessor.HttpContext?.User
                    .FindFirst("TenantId")?.Value;

                if (string.IsNullOrEmpty(tenantIdClaim) || !Guid.TryParse(tenantIdClaim, out var tenantId))
                {
                    throw new UnauthorizedAccessException("TenantId not found in token");
                }

                return tenantId;
            }
        }

        public Guid CurrentUserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedAccessException("UserId not found in token");
                }

                return userId;
            }
        }

    }
}
