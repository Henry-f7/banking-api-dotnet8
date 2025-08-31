using Banking.Api.Features.Transactions.Deposit;
using Banking.Api.Features.Transactions.Withdraw;

namespace Banking.Tests
{
    public class TransactionValidatorsTests
    {
        [Fact]
        public void Deposit_Must_Be_Positive()
        {
            var v = new DepositValidator();
            Assert.False(v.Validate(new DepositRequest(0, null)).IsValid);
            Assert.False(v.Validate(new DepositRequest(-5, null)).IsValid);
            Assert.True(v.Validate(new DepositRequest(10, "abc")).IsValid);
        }

        [Fact]
        public void Withdraw_Must_Be_Positive()
        {
            var v = new WithdrawValidator();
            Assert.False(v.Validate(new WithdrawRequest(0, null)).IsValid);
            Assert.True(v.Validate(new WithdrawRequest(1, null)).IsValid);
        }
    }
}
