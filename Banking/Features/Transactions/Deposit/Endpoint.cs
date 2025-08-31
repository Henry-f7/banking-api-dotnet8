using Banking.Api.Domain;
using Banking.Api.Infrastructure.Validation;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Transactions.Deposit
{
    public static class Endpoint
    {
        public static IEndpointRouteBuilder MapDeposit(this IEndpointRouteBuilder app)
        {
            app.MapPost("/accounts/{accountNumber}/deposit", async (string accountNumber, DepositRequest req, AppDbContext db) =>
            {
                var acc = await db.BankAccounts
                    .Include(a => a.Transactions)
                    .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

                if (acc is null) return Results.NotFound(new { message = "Account not found" });

                if (!string.IsNullOrWhiteSpace(req.IdempotencyKey))
                {
                    var done = await db.BankTransactions.AnyAsync(t => t.AccountId == acc.Id && t.IdempotencyKey == req.IdempotencyKey);
                    if (done) return Results.Ok(new { acc.AccountNumber, acc.Balance, idempotent = true });
                }

                acc.Balance += req.Amount;
                acc.Version++;

                db.BankTransactions.Add(new BankTransaction
                {
                    AccountId = acc.Id,
                    Type = TransactionType.Deposit,
                    Amount = req.Amount,
                    BalanceAfter = acc.Balance,
                    IdempotencyKey = req.IdempotencyKey
                });

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Results.StatusCode(StatusCodes.Status409Conflict);
                }

                return Results.Ok(new { acc.AccountNumber, acc.Balance });
            })
            .WithTags("Transactions")
            .WithName("Deposit")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .AddEndpointFilter<ValidationFilter<DepositRequest>>();

            return app;
        }
    }
}
