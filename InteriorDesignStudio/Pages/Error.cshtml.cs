using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteriorDesignStudio.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public int StatusCodeValue { get; private set; } = 500;
    public string? RequestId { get; private set; }
    public bool IsNotFound => StatusCodeValue == 404;

    public void OnGet(int? code) => Populate(code);
    public void OnPost(int? code) => Populate(code);

    private void Populate(int? code)
    {
        StatusCodeValue = code ?? 500;
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        Response.StatusCode = StatusCodeValue;
    }
}
