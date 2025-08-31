namespace Banking.Api.Features.Transactions.Withdraw
{
    public record WithdrawRequest(decimal Amount, string? IdempotencyKey);
}
