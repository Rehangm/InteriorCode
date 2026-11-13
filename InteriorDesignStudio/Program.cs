using System.Text;
using System.Threading.RateLimiting;
using InteriorDesignStudio.Configuration;
using InteriorDesignStudio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configuration ----------
builder.Services.Configure<CompanySettings>(builder.Configuration.GetSection(CompanySettings.SectionName));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<CompanySettings>>().Value);

// ---------- Database (EF Core) ----------
// "DatabaseProvider" = "SqlServer" (default, production) or "Sqlite" (local development without SQL Server).
var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "SqlServer";

if (databaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
{
    var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataFolder);
    var sqliteConnection = $"Data Source={Path.Combine(dataFolder, "InteriorDesignStudio.db")}";
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(sqliteConnection));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' was not found. Add it to appsettings.json or user secrets.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(maxRetryCount: 3)));
}

// ---------- Razor Pages ----------
builder.Services.AddRazorPages();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;          // clean, SEO-friendly URLs: /about, /projects/marine-drive-penthouse
    options.AppendTrailingSlash = false;
});
builder.Services.AddMemoryCache();
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "IDS.Antiforgery";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// ---------- Rate limit contact form submissions (spam protection) ----------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        HttpMethods.IsPost(ctx.Request.Method) && ctx.Request.Path.StartsWithSegments("/contact")
            ? RateLimitPartition.GetFixedWindowLimiter(
                ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(10),
                    QueueLimit = 0
                })
            : RateLimitPartition.GetNoLimiter("unlimited"));
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "text/plain; charset=utf-8";
        await context.HttpContext.Response.WriteAsync(
            "You have sent several enquiries in a short time. Please wait a few minutes and try again.", token);
    };
});

var app = builder.Build();

// ---------- Database migration + sample data ----------
await InitialiseDatabaseAsync(app);

// ---------- HTTP pipeline ----------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();

// Compress responses in production only. In Development, Visual Studio injects its hot-reload
// script into HTML pages, and combining that with compression can cut pages off (blank page).
if (!app.Environment.IsDevelopment())
    app.UseResponseCompression();

// Basic security headers
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers["X-Content-Type-Options"] = "nosniff";
    headers["X-Frame-Options"] = "SAMEORIGIN";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (!app.Environment.IsDevelopment())
            ctx.Context.Response.Headers.CacheControl = "public,max-age=604800";
    }
});

app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();

app.MapRazorPages();

// ---------- SEO endpoints ----------
app.MapGet("/robots.txt", (CompanySettings company) =>
    Results.Text($"User-agent: *\nAllow: /\nSitemap: {company.SiteUrl.TrimEnd('/')}/sitemap.xml\n", "text/plain"));

app.MapGet("/sitemap.xml", async (AppDbContext db, CompanySettings company) =>
{
    var root = company.SiteUrl.TrimEnd('/');
    var urls = new List<string> { "/", "/about", "/services", "/projects", "/contact" };
    urls.AddRange(await db.Services.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder)
        .Select(s => "/services/" + s.Slug).ToListAsync());
    urls.AddRange(await db.Projects.OrderByDescending(p => p.CreatedDate)
        .Select(p => "/projects/" + p.Slug).ToListAsync());

    var xml = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
    xml.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
    foreach (var url in urls)
        xml.Append("  <url><loc>").Append(root).Append(url).Append("</loc></url>\n");
    xml.Append("</urlset>");
    return Results.Text(xml.ToString(), "application/xml", Encoding.UTF8);
});

app.Run();

static async Task InitialiseDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInit");
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        if (db.Database.IsSqlite())
        {
            // Local development database: a single file in App_Data, created automatically.
            await db.Database.EnsureCreatedAsync();
        }
        else if (db.Database.GetMigrations().Any())
        {
            // Normal path: apply any pending EF Core migrations (Add-Migration / Update-Database).
            await db.Database.MigrateAsync();
        }
        else
        {
            // No migrations have been generated yet – create the schema directly so the site runs
            // immediately. See README.md to switch to migrations.
            logger.LogWarning("No EF Core migrations found. Creating database with EnsureCreated().");
            await db.Database.EnsureCreatedAsync();
        }

        await DbSeeder.SeedAsync(db, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex,
            "Database initialisation failed. Check the 'DefaultConnection' connection string and that SQL Server is running.");
        if (!app.Environment.IsDevelopment()) throw;
    }
}
