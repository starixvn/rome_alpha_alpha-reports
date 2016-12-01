namespace Alpha.Reports.Tests.Criteria.WinLossDetail
{
    using System;
    using Reports.Criteria.WinLossDetail;
    using Reports.Models;
    using Reports.Models.Enumerations;
    using Xunit;

    public class WinLossDetailCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLossDetail_Member")]
        [InlineData(3, "Age_Rpt_WinLossDetail_Agent")]
        [InlineData(4, "Age_Rpt_WinLossDetail_Master")]
        public void WinLossDetailCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte levelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);
            var winLossDetailCriteria = new WinLossDetailCriteria(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, userLevel);

            // Act
            var actualSettingKey = winLossDetailCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
