using Banking.Api.Domain.Common;

namespace Banking.Api.Domain
{
    public class Customer : AuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        // "M", "F", "O" (Other).
        public string Gender { get; set; } = "O";
        public required string NationalId { get; set; }
        public decimal MonthlyIncomeAmount { get; set; }
        public string MonthlyIncomeCurrency { get; set; } = "NIO";
        public List<BankAccount> Accounts { get; set; } = new();
    }
}
