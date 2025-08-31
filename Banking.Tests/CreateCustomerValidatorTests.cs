using Banking.Api.Features.Customers.Create;

namespace Banking.Tests
{
    public class CreateCustomerValidatorTests
    {
        [Theory]
        [InlineData("001-071296-0003A", "1996-12-07", "NIO", 1000)]
        [InlineData("001-010199-1234Z", "1999-01-01", "USD", 1)]
        public void Valid_Request_Ok(string nid, string birth, string cur, decimal income)
        {
            var v = new CreateCustomerValidator();
            var req = new CreateCustomerRequest("John Doe", DateTime.Parse(birth), "M", nid, income, cur);
            var r = v.Validate(req);
            Assert.True(r.IsValid, string.Join("; ", r.Errors.Select(e => e.ErrorMessage)));
        }

        [Fact]
        public void Invalid_Currency_Fails()
        {
            var v = new CreateCustomerValidator();
            var req = new CreateCustomerRequest("Ana", DateTime.Parse("2000-05-10"), "F", "001-100500-0001B", 500, "EUR");
            var r = v.Validate(req);
            Assert.False(r.IsValid);
            Assert.Contains(r.Errors, e => e.PropertyName == "MonthlyIncomeCurrency");
        }
    }
}
