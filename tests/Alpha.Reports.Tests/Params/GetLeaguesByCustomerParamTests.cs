namespace Alpha.Reports.Tests.Params
{
    using System;
    using Constants;
    using Fixtures;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class GetLeaguesByCustomerParamTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetLeaguesByCustomerParamTests(AnyValueCreatorFixture anyValueCreator)
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void FromDate_NotSetFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                DateTime.MinValue,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());
            var expectedDate = DefaultVariablesTest.CurrentDate;

            // Act
            var actualDate = param.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_SetFromDate_ReturnsFromDateSetFromConstructor()
        {
            // Arrange
            var fromDate = anyValueCreator.Create<DateTime>();
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                fromDate,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());
            var expectedDate = fromDate;

            // Act
            var actualDate = param.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSetToDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<DateTime>(),
                DateTime.MinValue,
                anyValueCreator.Create<string>());
            var expectedDate = DefaultVariablesTest.CurrentDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SetToDate_ReturnsToDateSetFromConstructor()
        {
            // Arrange
            var toDate = anyValueCreator.Create<DateTime>();
            var param = new GetLeaguesByCustomerParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<DateTime>(),
                toDate,
                anyValueCreator.Create<string>());
            var expectedDate = toDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}
