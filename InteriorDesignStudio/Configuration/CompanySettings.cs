namespace InteriorDesignStudio.Configuration;

/// <summary>
/// Company contact details bound from the "Company" section of appsettings.json.
/// Replace the placeholder values there with your real information.
/// </summary>
public class CompanySettings
{
    public const string SectionName = "Company";

    public string Name { get; set; } = "Interior Design Studio";
    public string Tagline { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    /// <summary>International format without "+" or spaces, used for wa.me links.</summary>
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string BusinessHoursWeekdays { get; set; } = string.Empty;
    public string BusinessHoursWeekend { get; set; } = string.Empty;
    /// <summary>Text used for the embedded Google Map query.</summary>
    public string MapQuery { get; set; } = string.Empty;
    public string SiteUrl { get; set; } = "https://www.example.com";
    public SocialLinks Social { get; set; } = new();

    public string FullAddress => string.Join(", ",
        new[] { AddressLine1, AddressLine2, City }.Where(s => !string.IsNullOrWhiteSpace(s)));

    public string PhoneHref => "tel:" + new string(Phone.Where(c => char.IsDigit(c) || c == '+').ToArray());

    public string WhatsAppLink(string? message = null) =>
        $"https://wa.me/{WhatsAppNumber}" +
        (string.IsNullOrWhiteSpace(message) ? string.Empty : "?text=" + Uri.EscapeDataString(message));
}

public class SocialLinks
{
    public string Instagram { get; set; } = "#";
    public string Facebook { get; set; } = "#";
    public string Pinterest { get; set; } = "#";
    public string LinkedIn { get; set; } = "#";
    public string Houzz { get; set; } = "#";
}
