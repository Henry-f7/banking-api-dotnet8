namespace Banking.Api.Domain
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public required string NationalId { get; set; }
        public decimal? MonthlyIncome { get; set; }
        public List<BankAccount> Accounts { get; set; } = new();
    }
}
