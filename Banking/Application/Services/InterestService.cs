using Banking.Api.Domain;
using Banking.Api.Features.Transactions;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Application.Services
{
    public class InterestService : IInterestService
    {
        private readonly AppDbContext db;
        public InterestService(AppDbContext db) => this.db = db;

        public async Task ApplyMonthlyAsync(string accountNumber, decimal ratePercent, CancellationToken ct = default)
        {
            var acc = await db.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct)
                ?? throw new InvalidOperationException("Account not found");

            var interest = Math.Round(acc.Balance * (ratePercent / 100m), 2, MidpointRounding.ToZero);
            if (interest == 0) return;

            acc.Balance += interest;
            acc.Version++;

            db.BankTransactions.Add(new BankTransaction
            {
                AccountId = acc.Id,
                Type = TransactionType.Interest,
                Amount = interest,
                BalanceAfter = acc.Balance
            });

            await db.SaveChangesAsync(ct);
        }
    }
}
