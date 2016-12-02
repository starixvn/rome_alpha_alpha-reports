namespace Alpha.Reports.Tests.Params
{
    using Reports.Params;
    using Xunit;

    public class ProductByCustomerParamTests
    {
        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var productByCustomerParam = new ProductByCustomerParam(custId);

            // Act
            var actual = productByCustomerParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }
    }
}
