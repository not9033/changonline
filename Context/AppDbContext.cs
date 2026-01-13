using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ChangOnline.Models;

namespace ChangOnline.Context;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Subdistrict> Subdistricts { get; set; }
    public DbSet<ProviderProfile> ProviderProfiles { get; set; }
    public DbSet<ProviderCategoryModel> ProviderCategories { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<ProviderReview> ProviderReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ServiceRequest>()
            .HasOne(s => s.Requester)
            .WithMany()
            .HasForeignKey(s => s.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ServiceRequest>()
            .HasOne(s => s.Provider)
            .WithMany()
            .HasForeignKey(s => s.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProviderReview>()
            .HasOne(r => r.Provider)
            .WithMany()
            .HasForeignKey(r => r.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProviderReview>()
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}