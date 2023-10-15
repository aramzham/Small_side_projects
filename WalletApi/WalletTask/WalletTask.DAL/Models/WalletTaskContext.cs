using System;
using Microsoft.EntityFrameworkCore;

namespace WalletTask.DAL.Models
{
    public class WalletTaskContext : DbContext
    {
        private string _connectionString;

        public WalletTaskContext()
        {
        }

        public WalletTaskContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public WalletTaskContext(DbContextOptions<WalletTaskContext> options) : base(options)
        {
        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new Exception("no connection string");
                }

                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}