namespace Alpha.Reports.Tests.Models.Statement
{
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Tests.Constants;
    using Alpha.Reports.Tests.Fixtures;
    using Xunit;

    [Trait("Category", "Statement")]
    public class StatementModelTests
    {
        [Fact]
        public void WinLossDateString_Always_ReturnsWinLossDateString()
        {
            // Arrange
            var model = new StatementModel(DefaultVariables.CurrentDate, 0, null, null, null, null, null, null, null, null, null);
            var expectedWinLossDateString = "12/02/2016";

            // Act
            var actualWinLossDateString = model.WinLossDateString;

            // Assert
            Assert.Equal(expectedWinLossDateString, actualWinLossDateString);
        }

        [Fact]
        public void IsNotedBalance_NullPreviousStatement_ReturnsFalse()
        {
            // Arrange
            StatementModel previousStatement = null;
            var model = new StatementModel(DefaultVariables.CurrentDate, 0, null, null, null, null, null, null, null, null, null);

            // Act
            var actualIsNotedBalance = model.IsNotedBalance(previousStatement);

            // Assert
            Assert.False(actualIsNotedBalance);
        }

        [Fact]
        public void IsNotedBalance_PreviousClosingBalanceIsTheSameWithOpeningBalance_ReturnsFalse()
        {
            // Arrange
            StatementModel previousStatement = new StatementModel(DefaultVariables.CurrentDate, 0, 10, null, null, null, null, null, null, 10, null);
            var model = new StatementModel(DefaultVariables.CurrentDate, 0, 10, null, null, null, null, null, null, null, null);

            // Act
            var actualIsNotedBalance = model.IsNotedBalance(previousStatement);

            // Assert
            Assert.False(actualIsNotedBalance);
        }

        [Fact]
        public void IsNotedBalance_PreviousClosingBalanceIsNotTheSameWithOpeningBalance_ReturnsTrue()
        {
            // Arrange
            StatementModel previousStatement = new StatementModel(DefaultVariables.CurrentDate, 0, 10, null, null, null, null, 10, null, null, null);
            var model = new StatementModel(DefaultVariables.CurrentDate, 0, 20, null, null, null, null, null, null, null, null);

            // Act
            var actualIsNotedBalance = model.IsNotedBalance(previousStatement);

            // Assert
            Assert.True(actualIsNotedBalance);
        }
    }
}
