namespace Banking.Api.Features.Customers.GetById
{
    public record CustomerDetailResponse(
        Guid Id,
        string FullName,
        string NationalId,
        DateTime BirthDate,
        string Gender,
        decimal MonthlyIncomeAmount,
        string MonthlyIncomeCurrency,
        List<CustomerAccountItem> Accounts
    );
}
