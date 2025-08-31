using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Customers.GetById
{
    public static class EndPoint
    {
        public static IEndpointRouteBuilder MapGetCustomerById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers/{id:guid}", async (Guid id, AppDbContext db) =>
            {
                var dto = await db.Customers
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new CustomerDetailResponse(
                        c.Id,
                        c.FullName,
                        c.NationalId,
                        c.BirthDate,
                        c.Gender,
                        c.MonthlyIncomeAmount,
                        c.MonthlyIncomeCurrency,
                        c.Accounts
                            .OrderBy(a => a.AccountNumber)
                            .Select(a => new CustomerAccountItem(a.AccountNumber, a.Currency, a.Balance))
                            .ToList()
                    ))
                    .FirstOrDefaultAsync();

                return dto is null
                    ? Results.NotFound(new { message = "Customer not found" })
                    : Results.Ok(dto);
            })
            .WithTags("Customers")
            .WithName("GetCustomerById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
