using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages.Projects;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public IReadOnlyList<Project> Projects { get; private set; } = Array.Empty<Project>();

    /// <summary>Optional initial filter, e.g. /projects?category=kitchen (keeps filtered views linkable).</summary>
    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public string ActiveFilter { get; private set; } = "all";

    public async Task OnGetAsync()
    {
        Projects = await _db.Projects.AsNoTracking()
            .OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CreatedDate)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(Category))
        {
            var key = Category.Trim().ToLowerInvariant();
            if (ProjectCategories.All.Any(c => ProjectCategories.ToKey(c) == key))
                ActiveFilter = key;
        }
    }

    public int CountFor(string category) =>
        Projects.Count(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
}
