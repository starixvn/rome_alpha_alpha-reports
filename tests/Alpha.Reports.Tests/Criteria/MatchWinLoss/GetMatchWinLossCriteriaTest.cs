namespace Alpha.Reports.Tests.Criteria.MatchWinLoss
{
    using System;
    using Constants;
    using Fixtures;
    using Reports.Criteria.MatchWinLoss;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class GetMatchWinLossCriteriaTest
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public GetMatchWinLossCriteriaTest(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getMatchWinLossCriteria = new GetMatchWinLossCriteria(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>());
            var expectedSettingKey = "Age_Rpt_MatchWinLoss_Member";

            // Act
            var actualSettingKey = getMatchWinLossCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var getMatchWinLossCriteria = new GetMatchWinLossCriteria(
                custId,
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>());

            // Act
            var actualIsValid = getMatchWinLossCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = fromDate.AddDays(-1);
            var getMatchWinLossCriteria = new GetMatchWinLossCriteria(
                anyValueCreatorFixture.Create<int>(),
                fromDate,
                toDate);

            // Act
            var actualIsValid = getMatchWinLossCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var getMatchWinLossCriteria = new GetMatchWinLossCriteria(
                1,
                DefaultVariablesTest.CurrentDate,
                DefaultVariablesTest.CurrentDate);

            // Act
            var actualIsValid = getMatchWinLossCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
