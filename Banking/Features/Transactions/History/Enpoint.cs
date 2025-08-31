using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Transactions.History
{
    public static class Enpoint
    {
        public static IEndpointRouteBuilder MapGetTransactions(this IEndpointRouteBuilder app)
        {
            app.MapGet("/accounts/{accountNumber}/transactions", async (
                string accountNumber, DateTime? from, DateTime? to, int page, int size, AppDbContext db) =>
            {
                page = page <= 0 ? 1 : page;
                size = size <= 0 ? 20 : size;

                var acc = await db.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                if (acc is null) return Results.NotFound(new { message = "Account not found" });

                var q = db.BankTransactions.Where(t => t.AccountId == acc.Id);

                if (from.HasValue) q = q.Where(t => t.CreatedAtUtc >= from.Value);
                if (to.HasValue) q = q.Where(t => t.CreatedAtUtc < to.Value);

                var total = await q.CountAsync();
                var items = await q.OrderBy(t => t.CreatedAtUtc)
                                   .Skip((page - 1) * size)
                                   .Take(size)
                                   .Select(t => new {
                                       t.Id,
                                       t.Type,
                                       t.Amount,
                                       t.BalanceAfter,
                                       t.CreatedAtUtc
                                   }).ToListAsync();

                return Results.Ok(new
                {
                    accountNumber = acc.AccountNumber,
                    currency = acc.Currency,
                    currentBalance = acc.Balance,page,size,total,items
                });
            })
            .WithTags("Transactions")
            .WithName("GetTransactions")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            return app;
        }
    }
}
