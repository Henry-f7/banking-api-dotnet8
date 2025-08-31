using Banking.Api.Domain;
using Banking.Api.Domain.People;
using Banking.Api.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Features.Customers.Create
{
    public static class Endpoint
    {
        public static IEndpointRouteBuilder MapCreateCustomer(this IEndpointRouteBuilder app)
        {
            app.MapPost("/customers", async (
            CreateCustomerRequest request,
            AppDbContext db,
            IValidator<CreateCustomerRequest> validator,
            CancellationToken ct) =>
            {
                var result = await validator.ValidateAsync(request, ct);
                if (!result.IsValid) return Results.ValidationProblem(result.ToDictionary());

                var genderCode = GenderTokens.TryNormalize(request.Gender, out var g)
                    ? g switch { Gender.Male => "M", Gender.Female => "F", _ => "O" }
                    : "O";

                var currency = request.MonthlyIncomeCurrency.Trim().ToUpperInvariant();
                var nid = request.NationalId.Trim().ToUpperInvariant();

                var existsNid = await db.Customers.AnyAsync(c => c.NationalId == request.NationalId);
                if (existsNid) return Results.Conflict(new { message = "NationalId already exists" });

                var customer = new Customer
                {
                    FullName = request.FullName.Trim(),
                    BirthDate = request.BirthDate.Date,
                    Gender = genderCode,
                    NationalId = nid,
                    MonthlyIncomeAmount = Math.Round(request.MonthlyIncomeAmount, 2, MidpointRounding.ToEven),
                    MonthlyIncomeCurrency = currency
                };

                db.Customers.Add(customer);
                await db.SaveChangesAsync(ct);

                return Results.Created($"/customers/{customer.Id}", new
                {
                    customer.Id,
                    customer.FullName,
                    customer.BirthDate,
                    customer.Gender,
                    customer.NationalId,
                    MonthlyIncome = new { amount = customer.MonthlyIncomeAmount, currency = customer.MonthlyIncomeCurrency }
                });
            })
        .WithTags("Customers")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status400BadRequest);

            return app;
        }
    }
}
