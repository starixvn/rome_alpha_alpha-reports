namespace Alpha.Reports.Tests.Criteria.BetChoiceMatching
{
    using Alpha.Reports.Criteria.BetChoiceMatching;
    using System;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingCriteriaTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var winlostDate = DateTime.Parse("2019/09/09");
            var moveBetsDay = DateTime.Parse("2019/08/08");
            var criteria = new BetChoiceMatchingCriteria(1, winlostDate, moveBetsDay, 2, true);

            // Assert
            Assert.True(
#pragma warning disable S1067 // Expressions should not be too complex
                criteria.CustId == 1
                && criteria.WinlostDate == winlostDate
                && criteria.MoveBetsDay == moveBetsDay
                && criteria.BetCount == 2
                && criteria.IsExportExcel
                && criteria.GetSettingKey() == "Age_BetStatistics_BetMatching_Master"
                && criteria.IsValid());
#pragma warning restore S1067 // Expressions should not be too complex
        }
    }
}
