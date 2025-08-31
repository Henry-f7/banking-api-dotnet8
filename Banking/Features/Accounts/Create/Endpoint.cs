using Banking.Api.Domain;
using Banking.Api.Features.Transactions;
using Banking.Api.Infrastructure.Validation;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Accounts.Create
{
    public static class Endpoint
    {
        public static IEndpointRouteBuilder MapCreateBankAccount(this IEndpointRouteBuilder app)
        {
            app.MapPost("/accounts", async (CreateAccountRequest req, AppDbContext db) =>
            {
                var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == req.CustomerId);
                if (customer is null) return Results.NotFound(new { message = "Customer not found" });

                var dup = await db.BankAccounts.AnyAsync(a => a.AccountNumber == req.AccountNumber);
                if (dup) return Results.Conflict(new { message = "AccountNumber already exists" });

                var acc = new BankAccount
                {
                    AccountNumber = req.AccountNumber,
                    CustomerId = req.CustomerId,
                    Balance = req.InitialBalance,
                    Currency = req.Currency
                };

                db.BankAccounts.Add(acc);

                if (req.InitialBalance > 0)
                {
                    db.BankTransactions.Add(new BankTransaction
                    {
                        Account = acc,
                        Type = TransactionType.Deposit,
                        Amount = req.InitialBalance,
                        BalanceAfter = req.InitialBalance
                    });
                }

                await db.SaveChangesAsync();
                return Results.Created($"/accounts/{acc.AccountNumber}", new { acc.Id, acc.AccountNumber, acc.Currency, acc.Balance });
            })
            .WithTags("Accounts")
            .WithName("CreateAccount")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>();

            return app;
        }
    }
}
