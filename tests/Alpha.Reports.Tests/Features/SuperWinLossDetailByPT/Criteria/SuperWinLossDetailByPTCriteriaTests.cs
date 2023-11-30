namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Criteria
{
    using System;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Criteria;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailByPTCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectProductDetails()
        {
            // Arrange
            var criteria = new SuperWinLossDetailByPTCriteria(
                123456,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1",
                "1,2,3,4,5");

            // Act
            var actualProductDetails = criteria.ProductDetails;

            // Assert
            Assert.Equal("1,2,3,4,5", actualProductDetails);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SuperWinLossDetailByPTCriteria(
                123456,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1",
                "1,2,3,4,5");

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_Rpt_WinLossDetailByPT_Super", actualSettingKey);
        }
    }
}
