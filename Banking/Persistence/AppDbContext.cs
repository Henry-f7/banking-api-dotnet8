using Banking.Api.Domain;
using Banking.Api.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Banking.Api.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<BankTransaction> BankTransactions => Set<BankTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.NationalId)
                .IsUnique();

            modelBuilder.Entity<BankAccount>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<BankTransaction>()
                .HasIndex(t => new { t.AccountId, t.IdempotencyKey });

            modelBuilder.Entity<BankAccount>()
                .Property(a => a.Version)
                .IsConcurrencyToken();

            // Filtro global soft-delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var param = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(param, nameof(AuditableEntity.DeletedAtUtc));
                    var body = Expression.Equal(prop, Expression.Constant(null));
                    var lambda = Expression.Lambda(body, param);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditableRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditableRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditableRules()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // ensure CreatedAtUtc is set on new entities
                        entry.Entity.CreatedAtUtc = utcNow;
                        break;

                    case EntityState.Modified:
                        // update updated timestamp
                        entry.Entity.UpdatedAtUtc = utcNow;
                        break;

                    case EntityState.Deleted:
                        // soft-delete: set DeletedAtUtc and mark as modified
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAtUtc = utcNow;
                        break;
                }
            }
        }
    }
}
