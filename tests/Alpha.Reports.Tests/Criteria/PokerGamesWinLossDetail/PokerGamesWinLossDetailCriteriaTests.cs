namespace Alpha.Reports.Tests.Criteria.PokerGamesWinLossDetail
{
    using System;
    using Reports.Criteria.PokerGamesWinLossDetail;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Params;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    public class PokerGamesWinLossDetailCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLossDetailPokerGames_Member")]
        [InlineData(3, "Age_Rpt_WinLossDetailPokerGames_Agent")]
        [InlineData(4, "Age_Rpt_WinLossDetailPokerGames_Master")]
        public void PokerGamesWinLossDetailCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var param = new PokerGamesWinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, userLevel, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            // Act
            var actualSettingKey = winLossDetailCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
