using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

/// <summary>A gallery image belonging to a project.</summary>
public class ProjectImage
{
    [Key]
    public int ImageId { get; set; }

    public int ProjectId { get; set; }

    [Required, StringLength(500)]
    public string ImagePath { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }

    public Project? Project { get; set; }
}
