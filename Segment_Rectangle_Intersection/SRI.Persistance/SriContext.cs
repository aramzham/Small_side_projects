using Microsoft.EntityFrameworkCore;
using SRI.Common;

namespace SRI.Persistance;

public class SriContext(DbContextOptions<SriContext> options) : DbContext(options)
{
    public DbSet<Rectangle> Rectangles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rectangle>(entity =>
        {
            entity.ToTable("Rectangle"); // Maps the Rectangle entity to the "Rectangle" table in the database

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Ensure Id is database-generated

            entity.Property(e => e.XMin).IsRequired();
            entity.Property(e => e.YMin).IsRequired();
            entity.Property(e => e.XMax).IsRequired();
            entity.Property(e => e.YMax).IsRequired();
        });
        
        modelBuilder.Entity<Rectangle>().HasData(
            new Rectangle(2, 3, 4, 5) {Id = 1},
            new Rectangle(7, 1, 9, 6) {Id = 2},
            new Rectangle(0, 0, 3, 1) {Id = 3}
        );
    }
}