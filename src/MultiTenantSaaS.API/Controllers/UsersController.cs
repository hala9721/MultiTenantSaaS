namespace MultiTenantSaaS.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Application.DTOs;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/tenants/{tenantId}/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/tenants/{tenantId}/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetTenantUsers(Guid tenantId)
    {
        // تحقق أن الـ tenant موجود
        var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == tenantId);
        if (!tenantExists)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        var users = await _context.Users
            .Where(u => u.TenantId == tenantId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                TenantId = u.TenantId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/tenants/{tenantId}/users/{userId}
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid tenantId, Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        return Ok(userDto);
    }

    // POST: api/tenants/{tenantId}/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(Guid tenantId, CreateUserRequest request)
    {
      
        var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == tenantId);
        if (!tenantExists)
        {
            return NotFound(new { message = "Tenant not found" });
        }

    
        var userExists = await _context.Users
            .AnyAsync(u => u.TenantId == tenantId && u.Email == request.Email);

        if (userExists)
        {
            return BadRequest(new { message = "Email already exists for this tenant" });
        }


        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            TenantId = tenantId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        return CreatedAtAction(nameof(GetUser), new { tenantId, userId = user.Id }, userDto);
    }

    // PUT: api/tenants/{tenantId}/users/{userId}
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid tenantId, Guid userId, UpdateUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tenants/{tenantId}/users/{userId}
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid tenantId, Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper method لـ Hash Password (بسيط)
    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}