using Banking.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<BankAccount> bankAccounts => Set<BankAccount>();
        public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(x => x.FullName).IsRequired();
                entity.HasMany(x => x.Accounts).WithOne(x => x.Customer)
                      .HasForeignKey(x => x.CustomerId);
            });

            // bank account
            modelBuilder.Entity<BankAccount>(e =>
            {
                e.Property(x => x.AccountNumber).IsRequired();
                e.HasIndex(x => x.AccountNumber).IsUnique();
                e.Property(x => x.Balance).HasPrecision(18, 2); // contract
                e.Property(x => x.Version).IsConcurrencyToken(); // token 
            });

            // bank transaction
            modelBuilder.Entity<BankTransaction>(e =>
            {
                e.Property(x => x.Amount).HasPrecision(18, 2);
                e.Property(x => x.BalanceAfter).HasPrecision(18, 2);
                e.HasIndex(x => new { x.AccountId, x.CreatedAt });
            });
        }
    }
}
