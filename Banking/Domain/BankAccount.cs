using Banking.Api.Domain.Common;

namespace Banking.Api.Domain
{
    public class BankAccount : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AccountNumber { get; set; } = default!;
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "NIO";
        public int Version { get; set; } = 0;
        public List<BankTransaction> Transactions { get; set; } = new();
    }
}
