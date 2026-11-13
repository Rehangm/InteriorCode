using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

public class Testimonial
{
    public int TestimonialId { get; set; }

    [Required, StringLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [StringLength(150)]
    public string Designation { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Review { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;
}
