namespace Alpha.Reports.Site.Tests.Helpers
{
    using Reports.Models;
    using Site.Helpers;
    using Xunit;

    public class JsonHelperTests
    {
        [Fact]
        public void ToJson_Always_ReturnsCorrectString()
        {
            // Arrange
            var data = new CustInfo(1, "CustName", 1);
            var expectedValue = "{'CustId':1,'CustName':'CustName','CustLevelId':1}";

            // Act
            var actualValue = JsonHelper.ToJson(data);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
