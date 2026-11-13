using InteriorDesignStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ContactEnquiry> ContactEnquiries => Set<ContactEnquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>(e =>
        {
            e.ToTable("Services");
            e.HasKey(s => s.ServiceId);
            e.HasIndex(s => s.Slug).IsUnique();
            e.Ignore(s => s.BenefitList);
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.ToTable("Projects");
            e.HasKey(p => p.ProjectId);
            e.HasIndex(p => p.Slug).IsUnique();
            e.HasIndex(p => p.Category);
            if (Database.IsSqlServer())
                e.Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            // One project → many gallery images; images are removed with their project.
            e.HasMany(p => p.Images)
             .WithOne(i => i.Project)
             .HasForeignKey(i => i.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectImage>(e =>
        {
            e.ToTable("ProjectImages");
            e.HasKey(i => i.ImageId);
            e.HasIndex(i => new { i.ProjectId, i.DisplayOrder });
        });

        modelBuilder.Entity<Testimonial>(e =>
        {
            e.ToTable("Testimonials");
            e.HasKey(t => t.TestimonialId);
        });

        modelBuilder.Entity<ContactEnquiry>(e =>
        {
            e.ToTable("ContactEnquiries");
            e.HasKey(c => c.EnquiryId);
            if (Database.IsSqlServer())
                e.Property(c => c.SubmittedDate).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(c => c.SubmittedDate);
        });
    }
}
