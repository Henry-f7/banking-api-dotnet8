using Banking.Api.Application.Services;
using Banking.Api.Domain;
using Banking.Api.Features.Transactions;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Tests
{
    public class InterestServiceTests
    {
        [Fact]
        public async Task ApplyMonthlyAsync_AddsInterestTransaction()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var db = new AppDbContext(options);
            db.Database.OpenConnection();
            db.Database.EnsureCreated();

            var acc = new BankAccount { AccountNumber = "T-001", Currency = "USD", Balance = 1000m, Customer = new Customer { FullName = "X", BirthDate = DateTime.UtcNow.AddYears(-20), NationalId = "001-010199-0001S", MonthlyIncomeAmount = 1, MonthlyIncomeCurrency = "USD" } };
            db.BankAccounts.Add(acc);
            await db.SaveChangesAsync();

            var svc = new InterestService(db);
            await svc.ApplyMonthlyAsync("T-001", 2m);

            var updated = await db.BankAccounts.Include(a => a.Transactions).FirstAsync(a => a.AccountNumber == "T-001");
            Assert.Equal(1020m, updated.Balance);
            Assert.Contains(updated.Transactions, t => t.Type == TransactionType.Interest && t.Amount == 20m);
        }
    }

}