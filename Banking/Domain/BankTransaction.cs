using Banking.Api.Features.Transactions;

namespace Banking.Api.Domain
{
    public class BankTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public BankAccount Account { get; set; } = default!;

        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //To avoid double charges if requests are repeated, you must use an IdempotencyKey
        public string? IdempotencyKey { get; set; }
    }
}
