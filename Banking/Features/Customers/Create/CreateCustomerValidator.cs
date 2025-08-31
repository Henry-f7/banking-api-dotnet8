using Banking.Api.Domain.Identity;
using Banking.Api.Domain.Primitives;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Banking.Api.Features.Customers.Create
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MaximumLength(150);

            RuleFor(x => x.BirthDate)
               .LessThan(DateTime.UtcNow.Date)
               .WithMessage("BirthDate must be in the past");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(x => x.MonthlyIncomeAmount)
                .GreaterThan(0m).WithMessage("MonthlyIncomeAmount must be greater than 0.");

            RuleFor(x => x.MonthlyIncomeCurrency)
                .NotEmpty().WithMessage("MonthlyIncomeCurrency is required.")
                .Must(Currency.IsSupported)
                .WithMessage($"MonthlyIncomeCurrency must be one of: {string.Join(", ", Currency.All)}");

            RuleFor(x => x.NationalId)
                .Cascade(CascadeMode.Stop)
                .Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("NationalId is required.")
                .Must(NicaraguanNationalId.IsValidFormat)
                    .WithMessage("NationalId must match ###-DDMMAA-####X.")
                .Must((x, nid) => NicaraguanNationalId.DateMatches(nid, x.BirthDate))
                    .WithMessage("NationalId date (DDMMAA) does not match BirthDate.");
        }
    }
}
