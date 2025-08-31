namespace Banking.Api.Features.Customers.List
{
    public record CustomerListItemResponse(
        Guid Id,
        string FullName,
        string NationalId,
        DateTime BirthDate,
        string Gender,
        decimal MonthlyIncomeAmount,
        string MonthlyIncomeCurrency,
        int AccountsCount
    );
}
