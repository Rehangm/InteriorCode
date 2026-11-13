using InteriorDesignStudio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InteriorDesignStudio.ViewComponents;

/// <summary>Renders the service links in the footer (cached for 10 minutes).</summary>
public class FooterServicesViewComponent : ViewComponent
{
    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FooterServicesViewComponent> _logger;

    public FooterServicesViewComponent(AppDbContext db, IMemoryCache cache, ILogger<FooterServicesViewComponent> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            var services = await _cache.GetOrCreateAsync("footer-services", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _db.Services.AsNoTracking()
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.DisplayOrder)
                    .Take(6)
                    .Select(s => new FooterServiceLink(s.ServiceName, s.Slug))
                    .ToListAsync();
            });
            return View(services ?? new List<FooterServiceLink>());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not load footer services.");
            return View(new List<FooterServiceLink>());
        }
    }
}

public record FooterServiceLink(string Name, string Slug);
