using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using InteriorDesignStudio.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages.Projects;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db) => _db = db;

    public Project Project { get; private set; } = null!;
    public IReadOnlyList<ProjectImage> Gallery { get; private set; } = Array.Empty<ProjectImage>();
    public ProjectNavLink? Previous { get; private set; }
    public ProjectNavLink? Next { get; private set; }
    public IReadOnlyList<Project> MoreProjects { get; private set; } = Array.Empty<Project>();

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var project = await _db.Projects.AsNoTracking()
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == slug);
        if (project is null) return NotFound();

        Project = project;
        Gallery = project.Images.OrderBy(i => i.DisplayOrder).ToList();

        // Previous / next follow the same order as the portfolio page and wrap around.
        var ordered = await _db.Projects.AsNoTracking()
            .OrderByDescending(p => p.IsFeatured)
            .ThenByDescending(p => p.CreatedDate)
            .Select(p => new ProjectNavLink(p.ProjectName, p.Slug, p.Category, p.CoverImage))
            .ToListAsync();

        var index = ordered.FindIndex(p => p.Slug == project.Slug);
        if (ordered.Count > 1 && index >= 0)
        {
            Previous = ordered[(index - 1 + ordered.Count) % ordered.Count];
            Next = ordered[(index + 1) % ordered.Count];
        }

        MoreProjects = await _db.Projects.AsNoTracking()
            .Where(p => p.ProjectId != project.ProjectId && p.Category == project.Category)
            .OrderByDescending(p => p.CreatedDate)
            .Take(3)
            .ToListAsync();

        return Page();
    }
}
