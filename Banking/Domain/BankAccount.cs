namespace Banking.Api.Domain
{
    public class BankAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;

        public string AccountNumber { get; set; } = default!;
        public decimal Balance { get; set; }

        public int Version { get; set; } = 0; // concurrency token lógico

        public List<BankTransaction> Transactions { get; set; } = new();
    }
}
