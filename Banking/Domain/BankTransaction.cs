using Banking.Api.Domain.Common;
using Banking.Api.Features.Transactions;

namespace Banking.Api.Domain
{
    public class BankTransaction : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public BankAccount Account { get; set; } = default!;
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string? IdempotencyKey { get; set; }
    }
}
