using Microsoft.EntityFrameworkCore;

namespace Radancy.Api.Data;

public class RadancyDbContext : DbContext
{
    public RadancyDbContext()
    {
        
    }

    public RadancyDbContext(DbContextOptions<RadancyDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("RadancyInMemoryDatabase");
    }
}