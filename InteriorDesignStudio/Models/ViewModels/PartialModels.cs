namespace InteriorDesignStudio.Models.ViewModels;

/// <summary>Model for the inner-page hero header (_PageHeader partial).</summary>
public record PageHeaderModel(
    string Eyebrow,
    string Title,
    string? Subtitle,
    string ImagePath,
    IReadOnlyList<BreadcrumbItem> Breadcrumbs);

public record BreadcrumbItem(string Label, string? Url = null);

/// <summary>Model for reusable section headings (_SectionHeading partial).</summary>
public record SectionHeadingModel(
    string Eyebrow,
    string Title,
    string? Lead = null,
    bool Centered = true,
    bool Light = false);

/// <summary>Model for the call-to-action band (_CallToAction partial).</summary>
public record CallToActionModel(
    string Title = "Let's Create Something Beautiful Together.",
    string Text = "Tell us about your space, your style and your ambitions. Our designers will get back to you within one business day.",
    string ButtonText = "Start Your Project",
    string ButtonUrl = "/contact#enquiry-form",
    string? ImagePath = null);

/// <summary>Model for previous / next navigation on project detail pages.</summary>
public record ProjectNavLink(string Name, string Slug, string Category, string CoverImage);
