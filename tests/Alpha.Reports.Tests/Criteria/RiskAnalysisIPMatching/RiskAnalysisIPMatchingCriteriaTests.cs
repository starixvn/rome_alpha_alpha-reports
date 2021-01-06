namespace Alpha.Reports.Tests.Criteria.MatchWinLossDetail
{
    using Alpha.Reports.Criteria.RiskAnalysisIPMatching;
    using Fixtures;
    using System;
    using Xunit;

    [Trait("Category", "iskAnalysisIPMatching")]
    [Collection("AnyValueCreatorCollection")]
    public class RiskAnalysisIPMatchingCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public RiskAnalysisIPMatchingCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<RiskAnalysisIPMatchingCriteria>();
            var expectedSettingKey = "Age_BetStatistics_IPMatching_Master";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(1, true)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId, bool expectedResult)
        {
            // Arrange
            var matchWinLossDetailCriteria = new RiskAnalysisIPMatchingCriteria(
                custId,
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<int>());

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.Equal(actualIsValid, expectedResult);
        }

        [Fact]
        public void RiskAnalysisIPMatching_Properties_AlwaysNotNull()
        {
            // Arrange
            var act = new RiskAnalysisIPMatchingCriteria(0, anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<int>());

            // Assert
            Assert.NotNull(act.WinlostDate);

            // Assert
            Assert.NotNull(act.MemberCount);

            // Assert
            Assert.NotNull(act.CustId);

            // Assert
            Assert.NotNull(act.MoveBetsDay);
        }
    }
}
