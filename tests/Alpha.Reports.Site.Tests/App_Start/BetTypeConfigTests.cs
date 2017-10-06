namespace Alpha.Reports.Site.Tests.App_Start
{
    using Site.App_Start;
    using Sunplus.Agent.Model.BL.Definitions;
    using Xunit;

    public class BetTypeConfigTests
    {
        [Fact]
        public void DisplayWinLossByBetTypeRules_NumberGame_ReturnsCorrectRule()
        {
            // Act
            var actualRule = BetTypeConfig.DisplayWinLossByBetTypeRules[3];

            // Assert
            Assert.True(actualRule(BetType.Next_High_Low, "Next High Low", false) == "Next High Low");
        }
    }
}
