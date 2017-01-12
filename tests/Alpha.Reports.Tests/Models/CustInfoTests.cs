namespace Alpha.Reports.Tests.Models
{
    using Reports.Models;
    using Xunit;

    public class CustInfoTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void CustId_Always_ReturnsIdFromCtor(int custId)
        {
            // Arrange
            var expectedCustId = custId;
            var custInfo = new CustInfo(custId, string.Empty, 0);

            // Act
            var actualCustId = custInfo.CustId;

            // Assert
            Assert.Equal(expectedCustId, actualCustId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("CustName")]
        public void CustName_Always_ReturnsIdFromCtor(string custName)
        {
            // Arrange
            var expectedCustName = custName;
            var custInfo = new CustInfo(0, custName, 0);

            // Act
            var actualCustName = custInfo.CustName;

            // Assert
            Assert.Equal(expectedCustName, actualCustName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(byte.MinValue)]
        [InlineData(byte.MaxValue)]
        public void UserLevelId_Always_ReturnsIdFromCtor(byte userLevelId)
        {
            // Arrange
            var expectedUserLevelId = userLevelId;
            var custInfo = new CustInfo(0, string.Empty, userLevelId);

            // Act
            var actualUserLevelId = custInfo.CustLevelId;

            // Assert
            Assert.Equal(expectedUserLevelId, actualUserLevelId);
        }
    }
}
