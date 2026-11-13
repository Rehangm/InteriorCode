using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

/// <summary>An enquiry submitted through the contact form.</summary>
public class ContactEnquiry
{
    [Key]
    public int EnquiryId { get; set; }

    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(25)]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string ServiceRequired { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string ProjectType { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string EstimatedBudget { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
}
