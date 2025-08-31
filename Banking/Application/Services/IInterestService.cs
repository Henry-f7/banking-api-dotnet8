namespace Banking.Api.Application.Services
{
    public interface IInterestService
    {
        Task ApplyMonthlyAsync(string accountNumber, decimal ratePercent, CancellationToken ct = default);
    }
}
