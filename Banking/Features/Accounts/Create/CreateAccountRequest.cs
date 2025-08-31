namespace Banking.Api.Features.Accounts.Create
{
    public record CreateAccountRequest(Guid CustomerId, string AccountNumber, decimal InitialBalance, string Currency = "NIO");
}
