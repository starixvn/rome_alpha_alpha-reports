namespace Alpha.Reports.Tests.Criteria.WinLossDetail
{
    using System;
    using NUnit.Framework;
    using Reports.Criteria.WinLossDetail;
    using Reports.Models;
    using Reports.Models.Enumerations;

    [TestFixture]
    public class WinLossDetailCriteriaTests
    {
        [TestCase(2, "Age_Rpt_WinLossDetail_Member")]
        [TestCase(3, "Age_Rpt_WinLossDetail_Agent")]
        [TestCase(4, "Age_Rpt_WinLossDetail_Master")]
        public void WinLossDetailCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte levelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);
            var winLossDetailCriteria = new WinLossDetailCriteria(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, userLevel);

            // Act
            var actualSettingKey = winLossDetailCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }
    }
}
