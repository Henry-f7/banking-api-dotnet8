using FluentValidation;

namespace Banking.Api.Features.Transactions.Withdraw
{
    public class WithdrawValidator : AbstractValidator<WithdrawRequest>
    {
        public WithdrawValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.IdempotencyKey).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.IdempotencyKey));
        }
    }
}
