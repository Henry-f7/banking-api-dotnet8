namespace Banking.Api.Features.Accounts.Create
{
    public record CreateAccountRequest(Guid CustomerId, decimal InitialBalance, string Currency = "NIO");
}
