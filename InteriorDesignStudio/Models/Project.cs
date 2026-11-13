using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

/// <summary>A completed portfolio project.</summary>
public class Project
{
    public int ProjectId { get; set; }

    [Required, StringLength(150)]
    public string ProjectName { get; set; } = string.Empty;

    [Required, StringLength(170)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>One of <see cref="ProjectCategories.All"/>.</summary>
    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string DesignConcept { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string CoverImage { get; set; } = string.Empty;

    /// <summary>e.g. "4,200 sq ft".</summary>
    [StringLength(40)]
    public string? Area { get; set; }

    public int? CompletionYear { get; set; }

    public bool IsFeatured { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
}

public static class ProjectCategories
{
    public const string Residential = "Residential";
    public const string Commercial = "Commercial";
    public const string LivingRoom = "Living Room";
    public const string Bedroom = "Bedroom";
    public const string Kitchen = "Kitchen";
    public const string Office = "Office";

    public static readonly string[] All =
        { Residential, Commercial, LivingRoom, Bedroom, Kitchen, Office };

    /// <summary>CSS-safe key used by the portfolio filter, e.g. "living-room".</summary>
    public static string ToKey(string category) =>
        category.Trim().ToLowerInvariant().Replace(' ', '-');
}
