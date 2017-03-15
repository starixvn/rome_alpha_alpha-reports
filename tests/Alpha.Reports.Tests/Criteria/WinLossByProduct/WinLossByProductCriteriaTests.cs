namespace Alpha.Reports.Tests.Criteria.WinLossByProduct
{
    using System;
    using Reports.Criteria.WinLossByProductCriteria;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    public class WinLossByProductCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLossByBettype_Member")]
        [InlineData(3, "Age_Rpt_WinLossByBettype_Agent")]
        [InlineData(4, "Age_Rpt_WinLossByBettype_Master")]
        public void WinLossByProductCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossByProductCriteria = new WinLossByProductCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, userLevel);

            // Act
            var actualSettingKey = winLossByProductCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
