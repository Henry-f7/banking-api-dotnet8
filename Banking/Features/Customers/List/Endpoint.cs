using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Customers.List
{
    public static class Endpoint
    {
        public static IEndpointRouteBuilder MapGetCustomers(this IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async (
                string? q,
                int page,
                int size,
                AppDbContext db) =>
            {
                page = page <= 0 ? 1 : page;
                size = size <= 0 ? 20 : size;

                var query = db.Customers.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var like = $"%{q}%";
                    query = query.Where(c =>
                        EF.Functions.Like(c.FullName, like) ||
                        EF.Functions.Like(c.NationalId, like));
                }

                var total = await query.CountAsync();

                var items = await query
                    .OrderByDescending(c => c.CreatedAtUtc)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(c => new CustomerListItemResponse(
                        c.Id,
                        c.FullName,
                        c.NationalId,
                        c.BirthDate,
                        c.Gender,
                        c.MonthlyIncomeAmount,
                        c.MonthlyIncomeCurrency,
                        c.Accounts.Count
                    ))
                    .ToListAsync();

                return Results.Ok(new
                {
                    page,
                    size,
                    total,
                    items
                });
            })
            .WithTags("Customers")
            .WithName("GetCustomers")
            .Produces(StatusCodes.Status200OK);

            return app;
        }
    }
}
