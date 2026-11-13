using System.ComponentModel.DataAnnotations;

namespace InteriorDesignStudio.Models;

/// <summary>
/// View model bound to the contact form. Kept separate from the
/// <see cref="ContactEnquiry"/> entity to prevent over-posting.
/// </summary>
public class ContactFormInput
{
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(150)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number.")]
    [RegularExpression(@"^\+?[0-9\s\-()]{7,20}$", ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select the service you need.")]
    [StringLength(120)]
    [Display(Name = "Service Required")]
    public string ServiceRequired { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a project type.")]
    [StringLength(60)]
    [Display(Name = "Project Type")]
    public string ProjectType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select an estimated budget.")]
    [StringLength(60)]
    [Display(Name = "Estimated Budget")]
    public string EstimatedBudget { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please tell us a little about your project.")]
    [StringLength(2000, MinimumLength = 20, ErrorMessage = "Message must be between 20 and 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>Honeypot field – must stay empty. Hidden from real users.</summary>
    public string? Website { get; set; }
}

public static class ContactFormOptions
{
    public static readonly string[] ProjectTypes =
    {
        "Apartment", "Villa / Independent House", "Penthouse", "Office", "Retail Store",
        "Restaurant / Café", "Hotel / Hospitality", "Other"
    };

    public static readonly string[] Budgets =
    {
        "Under ₹10 Lakh", "₹10 – 25 Lakh", "₹25 – 50 Lakh", "₹50 Lakh – ₹1 Crore", "Above ₹1 Crore", "Not sure yet"
    };

    public const string OtherService = "Other / Not sure";
}
