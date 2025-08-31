namespace Banking.Api.Features.Transactions.Deposit
{
    public record DepositRequest(decimal Amount, string? IdempotencyKey);
}
