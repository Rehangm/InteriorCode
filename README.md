# Interior Design Studio – Luxury Interior Design Website

A premium, fully responsive website for a luxury residential and commercial interior design company, built with **ASP.NET Core Razor Pages (.NET 8)**, **C#**, **Entity Framework Core 8**, **SQL Server**, **Bootstrap 5**, **jQuery**, **Font Awesome** and **Google Fonts** (Playfair Display + Poppins).

---

## 1. Prerequisites

| Tool | Version |
|---|---|
| Visual Studio 2022 (17.8 or later) or Visual Studio 2026 | "ASP.NET and web development" workload |
| .NET SDK | 8.0 or later |
| SQL Server | LocalDB (installed with Visual Studio), Express, Developer or full SQL Server |

> **Upgrading to .NET 10 (recommended long term):** .NET 8 left Microsoft support in November 2026. Once you have Visual Studio 17.16+ / Visual Studio 2026 and the .NET 10 SDK, change `<TargetFramework>` to `net10.0` in `InteriorDesignStudio.csproj` and the three EF Core package versions to `10.0.0`. No code changes are required.

---

## 2. Run in Visual Studio

1. Open **`InteriorDesignStudio.sln`**.
2. Visual Studio restores the NuGet packages automatically (*Build → Restore NuGet Packages* if not).
3. Check the connection string (section 3). LocalDB works with no changes.
4. Create the database (section 4).
5. Press **F5** (or **Ctrl+F5**). The site opens at `https://localhost:7180`.

On first run the app creates the schema (if needed) and **seeds sample data**: 8 services, 10 projects with gallery images and 5 sample testimonials.

### Run from the command line

```bash
cd InteriorDesignStudio
dotnet restore
dotnet run --launch-profile https
```

---

## 3. No SQL Server installed? (Development mode)

`appsettings.Development.json` sets `"DatabaseProvider": "Sqlite"`, so pressing **F5** in Visual Studio runs the site on a local SQLite file
(`App_Data/InteriorDesignStudio.db`). It is created and seeded automatically, and you don't need SQL Server, `Add-Migration` or `Update-Database` for it.
Contact-form enquiries are saved there too.

To run on SQL Server instead (the production setup), change that value to `"SqlServer"` (or delete the line) and follow sections 4–5.
Migrations (`Add-Migration` / `Update-Database`) **always** target SQL Server through `Data/DesignTimeDbContextFactory.cs`.

## 3b. SQL Server connection string

`InteriorDesignStudio/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=InteriorDesignStudioDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Other common examples:

```text
SQL Express (Windows auth):
Server=.\SQLEXPRESS;Database=InteriorDesignStudioDb;Trusted_Connection=True;TrustServerCertificate=True

Full SQL Server with a SQL login (put this in user secrets, NOT appsettings.json):
Server=YOUR_SERVER;Database=InteriorDesignStudioDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

### Keep credentials out of source control

The committed connection string uses Windows authentication and contains no password. If you need a SQL login, store it with **User Secrets** (the project already has a `UserSecretsId`):

```bash
cd InteriorDesignStudio
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;User Id=...;Password=...;TrustServerCertificate=True"
```

In Visual Studio: right-click the project → **Manage User Secrets**. In production use environment variables (`ConnectionStrings__DefaultConnection`) or a key vault.

---

## 4. Database creation & migrations

The packages `Microsoft.EntityFrameworkCore.SqlServer`, `.Design` and `.Tools` are already referenced.

### Option A – Package Manager Console (Visual Studio)

*Tools → NuGet Package Manager → Package Manager Console* (Default project: **InteriorDesignStudio**):

```powershell
Add-Migration InitialCreate
Update-Database
```

### Option B – .NET CLI

```bash
dotnet tool install --global dotnet-ef      # once
cd InteriorDesignStudio
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Then run the app; it seeds the sample data automatically.

### How start-up works

`Program.cs` runs this on start-up:

* if migrations exist → `Database.Migrate()` applies any that are pending;
* if no migrations exist yet → `Database.EnsureCreated()` builds the schema so the site runs right away.

The migration route (Option A/B) is recommended. If you first ran the site **without** migrations and then want to use them, drop the database once (`dotnet ef database drop` or delete it in SSMS), then run `Update-Database`.

### Future schema changes

```powershell
Add-Migration DescribeYourChange
Update-Database
```

### Tables

| Table | Key columns |
|---|---|
| `Services` | ServiceId, ServiceName, Slug, ShortDescription, Description, KeyBenefits, DesignApproach, ImagePath, Icon, DisplayOrder, IsActive |
| `Projects` | ProjectId, ProjectName, Slug, Category, Location, Description, DesignConcept, CoverImage, Area, CompletionYear, IsFeatured, CreatedDate |
| `ProjectImages` | ImageId, ProjectId (FK → Projects, cascade delete), ImagePath, Caption, DisplayOrder |
| `Testimonials` | TestimonialId, ClientName, Designation, Review, Rating |
| `ContactEnquiries` | EnquiryId, FullName, Email, Phone, ServiceRequired, ProjectType, EstimatedBudget, Message, SubmittedDate |

On top of the required fields, a few extras (Slug, ShortDescription, KeyBenefits, DesignApproach, Icon, IsFeatured, Area and others) were added to support clean URLs and richer pages. To read submitted enquiries:

```sql
SELECT * FROM ContactEnquiries ORDER BY SubmittedDate DESC;
```

---

## 5. Project structure

```
InteriorDesignStudio.sln
InteriorDesignStudio/
├── Program.cs                     # DI, EF Core, routing, security headers, rate limiting, sitemap/robots
├── appsettings.json               # Connection string + company contact details
├── Configuration/CompanySettings.cs
├── Data/
│   ├── AppDbContext.cs            # DbSets + relationships/indexes
│   └── DbSeeder.cs                # Sample services, projects, gallery images, testimonials
├── Models/                        # Service, Project, ProjectImage, Testimonial, ContactEnquiry, ContactFormInput
│   └── ViewModels/PartialModels.cs
├── Helpers/                       # ImageHelper (responsive URLs), SiteImages (section imagery)
├── ViewComponents/FooterServicesViewComponent.cs
├── Pages/
│   ├── Index.cshtml               # Home  (/)
│   ├── About.cshtml               # /about
│   ├── Services/Index.cshtml      # /services
│   ├── Services/Details.cshtml    # /services/{slug}
│   ├── Projects/Index.cshtml      # /projects  (category filter, ?category=kitchen)
│   ├── Projects/Details.cshtml    # /projects/{slug}  (gallery, lightbox, prev/next)
│   ├── Contact.cshtml             # /contact  (form → SQL Server)
│   ├── Error.cshtml               # /error/{code}
│   └── Shared/                    # _Layout, _Navbar, _Footer, _PageHeader, _SectionHeading,
│                                  # _ServiceCard, _ProjectCard, _WhyChooseUs, _TestimonialSlider,
│                                  # _CallToAction, _ValidationScriptsPartial, Components/
└── wwwroot/
    ├── css/  site.css (design system) + home, about, services, projects, contact
    ├── js/   site.js (global) + home, projects, contact
    └── images/ favicon.svg, placeholder.svg
```

---

## 6. Customise

* **Company details** (phone, WhatsApp, email, address, hours, map, social links): edit the `Company` section in `appsettings.json`. Every page reads from it.
* **WhatsApp**: `WhatsAppNumber` is in international format with no `+` or spaces (e.g. `919876543210`).
* **Images**: the sample content uses royalty-free Unsplash photo URLs. Put your own photos in `wwwroot/images/` and update `Helpers/SiteImages.cs` and the `ImagePath`/`CoverImage` columns (e.g. `/images/projects/penthouse-01.jpg`). Any image that fails to load is replaced with `placeholder.svg`.
* **Content**: services, projects and testimonials live in the database. Edit them in SSMS, or change `Data/DbSeeder.cs` before the first run.
* **Testimonials**: the seeded testimonials are **sample text**. Replace them with real client reviews before the site goes live.
* **Colours & typography**: CSS custom properties at the top of `wwwroot/css/site.css`.

---

## 7. Features checklist

* Transparent navbar over the hero that turns solid on scroll; animated mobile menu
* Hero, introduction, services, featured projects, why choose us, design philosophy, testimonial slider, CTA, footer
* Portfolio category filtering with animation and shareable URLs, hover effects, responsive gallery, keyboard/swipe lightbox, previous/next project navigation
* Contact form: client-side (jQuery Unobtrusive) and server-side validation, whitelisted dropdowns, anti-forgery token, honeypot, rate limiting (5 posts / 10 min per IP), Post-Redirect-Get success message, saved to SQL Server
* WhatsApp floating button, back-to-top, smooth scrolling, scroll-reveal and count-up animations, page loader, form loading state
* SEO: unique titles and meta descriptions, canonical URLs, Open Graph tags, JSON-LD LocalBusiness, `/sitemap.xml`, `/robots.txt`, lowercase clean URLs, semantic HTML, alt text, lazy-loaded responsive images (`srcset`)
* Accessibility: skip link, ARIA labels, focus styles, `prefers-reduced-motion` support
* Security: EF Core parameterised queries, model validation, anti-forgery, security headers, HSTS in production, no credentials in source control
