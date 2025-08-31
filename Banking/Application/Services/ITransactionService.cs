namespace Banking.Api.Application.Services
{
    public interface ITransactionService
    {
        Task DepositAsync(string accountNumber, decimal amount, string? idempotencyKey = null, CancellationToken ct = default);
        Task WithdrawAsync(string accountNumber, decimal amount, string? idempotencyKey = null, CancellationToken ct = default);
    }
}
