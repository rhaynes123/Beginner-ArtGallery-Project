using ArtGallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Image> Images { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Image>(entity => {
            entity.Property(e => e.Id);
            entity.Property(e => e.Guid);
            entity.Property(e => e.FilePath);
            entity.Property(e => e.CreatedDate);
        });
    }
}
