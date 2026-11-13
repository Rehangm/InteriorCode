using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages.Services;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db) => _db = db;

    public Service Service { get; private set; } = null!;
    public IReadOnlyList<Service> OtherServices { get; private set; } = Array.Empty<Service>();
    public IReadOnlyList<Project> RelatedProjects { get; private set; } = Array.Empty<Project>();

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var service = await _db.Services.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);
        if (service is null) return NotFound();

        Service = service;

        OtherServices = await _db.Services.AsNoTracking()
            .Where(s => s.IsActive && s.ServiceId != service.ServiceId)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();

        // Match projects to the service by category keyword where possible.
        var category = MapCategory(service.Slug);
        var query = _db.Projects.AsNoTracking();
        if (category is not null) query = query.Where(p => p.Category == category);

        RelatedProjects = await query.OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CreatedDate).Take(3).ToListAsync();

        if (RelatedProjects.Count == 0)
        {
            RelatedProjects = await _db.Projects.AsNoTracking()
                .OrderByDescending(p => p.CreatedDate).Take(3).ToListAsync();
        }

        return Page();
    }

    private static string? MapCategory(string slug) => slug switch
    {
        "residential-interior-design" => ProjectCategories.Residential,
        "commercial-interior-design" => ProjectCategories.Commercial,
        "luxury-living-room-design" => ProjectCategories.LivingRoom,
        "modular-kitchen-design" => ProjectCategories.Kitchen,
        "bedroom-interior-design" => ProjectCategories.Bedroom,
        "office-interior-design" => ProjectCategories.Office,
        _ => null
    };
}
