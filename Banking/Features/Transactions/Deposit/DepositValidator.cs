using FluentValidation;

namespace Banking.Api.Features.Transactions.Deposit
{
    public class DepositValidator : AbstractValidator<DepositRequest>
    {
        public DepositValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.IdempotencyKey).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.IdempotencyKey));
        }
    }
}
