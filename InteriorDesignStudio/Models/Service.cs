using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

/// <summary>An interior design service offered by the studio.</summary>
public class Service
{
    public int ServiceId { get; set; }

    [Required, StringLength(120)]
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>URL-friendly identifier, e.g. "modular-kitchen-design".</summary>
    [Required, StringLength(140)]
    public string Slug { get; set; } = string.Empty;

    /// <summary>One or two sentences used on cards.</summary>
    [Required, StringLength(300)]
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>Full description used on the services page and detail page.</summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>Key benefits, one per line.</summary>
    public string KeyBenefits { get; set; } = string.Empty;

    /// <summary>Our design approach for this service.</summary>
    public string DesignApproach { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string ImagePath { get; set; } = string.Empty;

    /// <summary>Font Awesome icon class, e.g. "fa-couch".</summary>
    [StringLength(60)]
    public string Icon { get; set; } = "fa-pen-ruler";

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public IEnumerable<string> BenefitList =>
        KeyBenefits.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
