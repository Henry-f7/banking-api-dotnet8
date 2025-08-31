using Banking.Api.Domain;
using Banking.Api.Features.Transactions;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Application.Services
{
    public sealed class TransactionService : ITransactionService
    {
        private readonly AppDbContext db;
        public TransactionService(AppDbContext db) => this.db = db;

        public async Task DepositAsync(string accountNumber, decimal amount, string? key = null, CancellationToken ct = default)
        {
            var acc = await db.BankAccounts.Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct)
                ?? throw new InvalidOperationException("Account not found");

            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (!string.IsNullOrWhiteSpace(key) && await db.BankTransactions.AnyAsync(t => t.AccountId == acc.Id && t.IdempotencyKey == key, ct))
                return; // idempotente

            acc.Balance += amount; acc.Version++;
            db.BankTransactions.Add(new BankTransaction { AccountId = acc.Id, Type = TransactionType.Deposit, Amount = amount, BalanceAfter = acc.Balance, IdempotencyKey = key });
            await db.SaveChangesAsync(ct);
        }

        public async Task WithdrawAsync(string accountNumber, decimal amount, string? key = null, CancellationToken ct = default)
        {
            var acc = await db.BankAccounts.Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct)
                ?? throw new InvalidOperationException("Account not found");

            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (acc.Balance < amount) throw new InvalidOperationException("Insufficient funds");
            if (!string.IsNullOrWhiteSpace(key) && await db.BankTransactions.AnyAsync(t => t.AccountId == acc.Id && t.IdempotencyKey == key, ct))
                return;

            acc.Balance -= amount; acc.Version++;
            db.BankTransactions.Add(new BankTransaction { AccountId = acc.Id, Type = TransactionType.Withdraw, Amount = amount, BalanceAfter = acc.Balance, IdempotencyKey = key });
            await db.SaveChangesAsync(ct);
        }
    }
}
