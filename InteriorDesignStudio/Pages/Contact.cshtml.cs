using InteriorDesignStudio.Data;
using InteriorDesignStudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Pages;

public class ContactModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly ILogger<ContactModel> _logger;

    public ContactModel(AppDbContext db, ILogger<ContactModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    [BindProperty]
    public ContactFormInput Input { get; set; } = new();

    public List<SelectListItem> ServiceOptions { get; private set; } = new();
    public List<SelectListItem> ProjectTypeOptions { get; } =
        ContactFormOptions.ProjectTypes.Select(t => new SelectListItem(t, t)).ToList();
    public List<SelectListItem> BudgetOptions { get; } =
        ContactFormOptions.Budgets.Select(b => new SelectListItem(b, b)).ToList();

    [TempData]
    public string? SuccessName { get; set; }

    public bool IsQuoteRequest { get; private set; }

    /// <summary>True after a successful submission (redirected to /contact?submitted=true).</summary>
    public bool ShowSuccess { get; private set; }

    public async Task OnGetAsync(string? service, string? enquiry, bool submitted = false)
    {
        // Show the thank-you message even if the TempData cookie was not kept by the browser.
        ShowSuccess = submitted || !string.IsNullOrEmpty(SuccessName);
        if (ShowSuccess) return;

        await LoadServiceOptionsAsync();
        IsQuoteRequest = string.Equals(enquiry, "quote", StringComparison.OrdinalIgnoreCase);

        // Pre-select a service when arriving from a service page (/contact?service=slug).
        if (!string.IsNullOrWhiteSpace(service))
        {
            var name = await _db.Services.AsNoTracking()
                .Where(s => s.Slug == service && s.IsActive)
                .Select(s => s.ServiceName)
                .FirstOrDefaultAsync();
            if (name is not null) Input.ServiceRequired = name;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadServiceOptionsAsync();

        // Honeypot: bots tend to fill every field. Pretend success, store nothing.
        if (!string.IsNullOrEmpty(Input.Website))
        {
            _logger.LogInformation("Contact form honeypot triggered; submission discarded.");
            SuccessName = "there";
            return RedirectToPage(new { submitted = true });
        }

        // Server-side whitelist checks for the dropdown values.
        if (!ServiceOptions.Any(o => o.Value == Input.ServiceRequired))
            ModelState.AddModelError("Input.ServiceRequired", "Please select a valid service.");
        if (!ContactFormOptions.ProjectTypes.Contains(Input.ProjectType))
            ModelState.AddModelError("Input.ProjectType", "Please select a valid project type.");
        if (!ContactFormOptions.Budgets.Contains(Input.EstimatedBudget))
            ModelState.AddModelError("Input.EstimatedBudget", "Please select a valid budget range.");

        if (!ModelState.IsValid)
            return Page();

        var enquiry = new ContactEnquiry
        {
            FullName = Input.FullName.Trim(),
            Email = Input.Email.Trim(),
            Phone = Input.Phone.Trim(),
            ServiceRequired = Input.ServiceRequired,
            ProjectType = Input.ProjectType,
            EstimatedBudget = Input.EstimatedBudget,
            Message = Input.Message.Trim(),
            SubmittedDate = DateTime.UtcNow
        };

        try
        {
            _db.ContactEnquiries.Add(enquiry);
            await _db.SaveChangesAsync();
            _logger.LogInformation("New enquiry {EnquiryId} received for {Service}.", enquiry.EnquiryId, enquiry.ServiceRequired);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to save contact enquiry.");
            ModelState.AddModelError(string.Empty,
                "Sorry, we could not send your enquiry right now. Please try again or contact us by phone or WhatsApp.");
            return Page();
        }

        SuccessName = enquiry.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? enquiry.FullName;

        // Post/Redirect/Get prevents duplicate submissions on refresh.
        return RedirectToPage(new { submitted = true });
    }

    private async Task LoadServiceOptionsAsync()
    {
        var names = await _db.Services.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => s.ServiceName)
            .ToListAsync();

        ServiceOptions = names.Select(n => new SelectListItem(n, n)).ToList();
        ServiceOptions.Add(new SelectListItem(ContactFormOptions.OtherService, ContactFormOptions.OtherService));
    }
}
