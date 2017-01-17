namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using Fixtures;
    using Reports.Criteria;
    using Reports.Params;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class GetLeaguesByCustomerCriteriaTest
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetLeaguesByCustomerCriteriaTest(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void IncludeTestLeagues_Always_ReturnsTrue()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());

            // Act
            var actualIncludeTestLeagues = getLeaguesByCustomerCriteria.IncludeTestLeagues;

            // Assert
            Assert.True(actualIncludeTestLeagues);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());
            var expectedSettingKey = "Age_Rpt_MatchWinloss_SelLeagueBySport";

            // Act
            var actualSettingKey = getLeaguesByCustomerCriteria.GetSettingKey();

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
            var param = new GetLeaguesByCustomerParam(
                custId,
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());

            // Act
            var actualIsValid = getLeaguesByCustomerCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsValid_NullOrWhiteSpaceProducts_ReturnsFalse(string products)
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                products,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());

            // Act
            var actualIsValid = getLeaguesByCustomerCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = anyValueCreator.Create<DateTime>();
            var toDate = fromDate.AddDays(-1);
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                fromDate,
                toDate,
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());

            // Act
            var actualIsValid = getLeaguesByCustomerCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var fromDate = anyValueCreator.Create<DateTime>();
            var toDate = fromDate;
            var param = new GetLeaguesByCustomerParam(
                1,
                "1,2,3",
                fromDate,
                toDate,
                anyValueCreator.Create<string>());

            var getLeaguesByCustomerCriteria = new GetLeaguesByCustomerCriteria(
                param,
                anyValueCreator.Create<DateTime>());

            // Act
            var actualIsValid = getLeaguesByCustomerCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
