namespace MultiTenantSaaS.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSaaS.Application.DTOs;
using MultiTenantSaaS.Domain.Entities;
using MultiTenantSaaS.Infrastructure.Data;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenantsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TenantsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/tenants
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants()
    {
        var tenants = await _context.Tenants
            .Select(t => new TenantDto
            {
                Id = t.Id,
                Name = t.Name,
                Slug = t.Slug,
                Email = t.Email,
                SubscriptionPlan = t.SubscriptionPlan,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(tenants);
    }

    // GET: api/tenants/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        var tenantDto = new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            Email = tenant.Email,
            SubscriptionPlan = tenant.SubscriptionPlan,
            IsActive = tenant.IsActive,
            CreatedAt = tenant.CreatedAt,
            UpdatedAt = tenant.UpdatedAt
        };

        return Ok(tenantDto);
    }

    // POST: api/tenants
    [HttpPost]
    public async Task<ActionResult<TenantDto>> CreateTenant(CreateTenantRequest request)
    {
   
        var existingTenant = await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == request.Slug);

        if (existingTenant != null)
        {
            return BadRequest(new { message = "Slug already exists" });
        }

        var tenant = new Tenant
        {
            Name = request.Name,
            Slug = request.Slug,
            Email = request.Email,
            SubscriptionPlan = request.SubscriptionPlan,
            IsActive = true
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();

        var tenantDto = new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Slug = tenant.Slug,
            Email = tenant.Email,
            SubscriptionPlan = tenant.SubscriptionPlan,
            IsActive = tenant.IsActive,
            CreatedAt = tenant.CreatedAt,
            UpdatedAt = tenant.UpdatedAt
        };

        return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenantDto);
    }

    // PUT: api/tenants/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(Guid id, UpdateTenantRequest request)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        tenant.Name = request.Name;
        tenant.Email = request.Email;
        tenant.SubscriptionPlan = request.SubscriptionPlan;
        tenant.IsActive = request.IsActive;
        tenant.UpdatedAt = DateTime.UtcNow;

        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/tenants/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        _context.Tenants.Remove(tenant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}