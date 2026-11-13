using Microsoft.EntityFrameworkCore;

namespace BankingSystem
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        public DbSet<BankAccount> Accounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().HasKey(a => a.AccountNumber);

            modelBuilder.Entity<CheckingAccount>();
            modelBuilder.Entity<SavingsAccount>();
        }
    }
}