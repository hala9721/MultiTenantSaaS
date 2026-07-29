namespace MultiTenantSaaS.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Application.DTOs;
using MultiTenantSaaS.Application.Services;
using MultiTenantSaaS.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
    
        var tenant = await _context.Tenants.FindAsync(request.TenantId);
        if (tenant == null)
        {
            return BadRequest(new { message = "Tenant not found" });
        }


        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.TenantId == request.TenantId && u.Email == request.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }


        var passwordHash = HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "User account is disabled" });
        }

        // اعمل tokens
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.TenantId, user.Email);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserDto
            {
                Id = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            }
        };

        return Ok(response);
    }

    // POST: api/auth/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(RefreshTokenRequest request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken);

        if (principal == null)
        {
            return Unauthorized(new { message = "Invalid refresh token" });
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        var tenantIdClaim = principal.FindFirst("TenantId");
        var emailClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.Email);

        if (userIdClaim == null || tenantIdClaim == null || emailClaim == null)
        {
            return Unauthorized(new { message = "Invalid token claims" });
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var tenantId = Guid.Parse(tenantIdClaim.Value);

        var user = await _context.Users.FindAsync(userId);

        if (user == null || user.TenantId != tenantId)
        {
            return Unauthorized(new { message = "User not found" });
        }

        var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.TenantId, user.Email);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var response = new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = new UserDto
            {
                Id = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            }
        };

        return Ok(response);
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}