using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public IReadOnlyList<Service> Services { get; private set; } = Array.Empty<Service>();
    public IReadOnlyList<Project> FeaturedProjects { get; private set; } = Array.Empty<Project>();
    public IReadOnlyList<Testimonial> Testimonials { get; private set; } = Array.Empty<Testimonial>();

    public async Task OnGetAsync()
    {
        Services = await _db.Services.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();

        FeaturedProjects = await _db.Projects.AsNoTracking()
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.CreatedDate)
            .Take(6)
            .ToListAsync();

        Testimonials = await _db.Testimonials.AsNoTracking()
            .OrderByDescending(t => t.Rating)
            .ThenBy(t => t.TestimonialId)
            .ToListAsync();
    }
}
