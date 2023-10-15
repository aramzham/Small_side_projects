using Microsoft.EntityFrameworkCore;
using UrlShortener.Web.Entities;
using UrlShortener.Web.Services;

namespace UrlShortener.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(x => x.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);
            
            builder.HasIndex(x => x.Code).IsUnique();
        });
    }
}