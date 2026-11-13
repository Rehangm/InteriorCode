using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages.Services;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public IReadOnlyList<Service> Services { get; private set; } = Array.Empty<Service>();

    public async Task OnGetAsync()
    {
        Services = await _db.Services.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }
}
