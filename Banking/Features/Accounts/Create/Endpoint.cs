using Banking.Api.Application.Services;
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
            app.MapPost("/accounts", async (
               CreateAccountRequest req,
               AppDbContext db,
               IAccountNumberGenerator numberGen) =>
            {
                var customer = await db.Customers.FirstOrDefaultAsync(c => c.Id == req.CustomerId);
                if (customer is null) return Results.NotFound(new { message = "Customer not found" });

                var accNumber = await numberGen.GenerateUniqueAsync(db);

                var acc = new BankAccount
                {
                    AccountNumber = accNumber,
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
                return Results.Created($"/accounts/{acc.AccountNumber}", new
                {
                    acc.Id,
                    acc.AccountNumber,
                    acc.Currency,
                    acc.Balance
                });
            })
           .WithTags("Accounts")
           .WithName("CreateAccount")
           .Produces(StatusCodes.Status201Created)
           .Produces(StatusCodes.Status404NotFound)
           .AddEndpointFilter<ValidationFilter<CreateAccountRequest>>();

            return app;
        }
    }
}
