using Banking.Api.Domain.Primitives;
using FluentValidation;

namespace Banking.Api.Features.Accounts.Create
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Currency).Must(Currency.IsSupported).WithMessage("Currency must be NIO or USD");
        }
    }
}
