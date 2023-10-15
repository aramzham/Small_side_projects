using FamousQuoteQuiz.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Dal;

public class QuizDbContext : DbContext
{
    private readonly string _connectionString;

    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
    }

    public QuizDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbSet<User> User { get; set; }
    public DbSet<Author> Author { get; set; }
    public DbSet<Quote> Quote { get; set; }
    public DbSet<UserAchievement> UserAchievement { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}