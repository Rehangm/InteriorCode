using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages;

public class AboutModel : PageModel
{
    private readonly AppDbContext _db;

    public AboutModel(AppDbContext db) => _db = db;

    public IReadOnlyList<Service> Services { get; private set; } = Array.Empty<Service>();
    public IReadOnlyList<Testimonial> Testimonials { get; private set; } = Array.Empty<Testimonial>();

    public async Task OnGetAsync()
    {
        Services = await _db.Services.AsNoTracking()
            .Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToListAsync();
        Testimonials = await _db.Testimonials.AsNoTracking()
            .OrderByDescending(t => t.Rating).Take(3).ToListAsync();
    }
}
