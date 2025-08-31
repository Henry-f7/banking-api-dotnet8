using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Accounts.GetBalance
{
    public static class Endpoint
    {
        public static IEndpointRouteBuilder MapGetAccountBalance(this IEndpointRouteBuilder app)
        {
            app.MapGet("/accounts/{accountNumber}/balance", async (string accountNumber, AppDbContext db) =>
            {
                var acc = await db.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                return acc is null
                    ? Results.NotFound(new { message = "Account not found" })
                    : Results.Ok(new { acc.AccountNumber, acc.Currency, acc.Balance });
            })
            .WithTags("Accounts")
            .WithName("GetAccountBalance")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
