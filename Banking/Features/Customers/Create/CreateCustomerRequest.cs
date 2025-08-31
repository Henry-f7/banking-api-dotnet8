namespace Banking.Api.Features.Customers.Create
{
    public record CreateCustomerRequest
    (
        string FullName,
        DateTime BirthDate,
        string Gender,
        string NationalId,
        decimal MonthlyIncomeAmount,
        string MonthlyIncomeCurrency
    );
}
