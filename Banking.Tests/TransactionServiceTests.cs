using Banking.Api.Application.Services;
using Banking.Api.Domain;
using Banking.Api.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Banking.Tests
{
    public class TransactionServiceTests
    {
        private static AppDbContext NewDb(out SqliteConnection conn)
        {
            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open(); // 1) abrir SIEMPRE antes de EnsureCreated
            var opt = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(conn)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;
            var db = new AppDbContext(opt);
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public async Task Deposit_Increases_Balance()
        {
            var db = NewDb(out var c);
            try
            {
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    FullName = "Test User",
                    BirthDate = new DateTime(1995, 1, 1),
                    Gender = "M",
                    NationalId = "001-010195-0001A",
                    MonthlyIncomeAmount = 1000,
                    MonthlyIncomeCurrency = "NIO"
                };
                db.Customers.Add(customer);
                await db.SaveChangesAsync();

                // 2) Crear la cuenta PARA ese customer
                var acc = new BankAccount
                {
                    AccountNumber = "A-1",
                    Currency = "NIO",
                    CustomerId = customer.Id,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                };
                db.BankAccounts.Add(acc);
                await db.SaveChangesAsync();

                var svc = new TransactionService(db);
                await svc.DepositAsync("A-1", 100m, "k1");

                var updated = await db.BankAccounts
                    .Include(a => a.Transactions)
                    .FirstAsync(a => a.AccountNumber == "A-1");

                Assert.Equal(100m, updated.Balance);
                Assert.Single(updated.Transactions);
            }
            finally { c.Dispose(); }
        }

        [Fact]
        public async Task Withdraw_Insufficient_Funds_Throws()
        {
            var db = NewDb(out var c);
            try
            {
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    FullName = "Test User",
                    BirthDate = new DateTime(1995, 1, 1),
                    Gender = "M",
                    NationalId = "001-010195-0002B",
                    MonthlyIncomeAmount = 800,
                    MonthlyIncomeCurrency = "USD"
                };
                db.Customers.Add(customer);
                await db.SaveChangesAsync();

                var acc = new BankAccount
                {
                    AccountNumber = "A-2",
                    Currency = "USD",
                    CustomerId = customer.Id,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                };
                db.BankAccounts.Add(acc);
                await db.SaveChangesAsync();

                var svc = new TransactionService(db);
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    svc.WithdrawAsync("A-2", 10m));
            }
            finally { c.Dispose(); }
        }
    }
}
