namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fixtures;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossDetailParamTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public MatchWinLossDetailParamTests(AnyValueCreatorFixture anyValueCreator)
        {
            SystemTime.Today = DefaultVariables.CurrentDate;
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void FromDate_NotSetFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new MatchWinLossDetailParam(
                anyValueCreator.Create<int>(),
                DateTime.MinValue,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                anyValueCreator.Create<UserLevel>());
            var expectedDate = DefaultVariables.CurrentDate;

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
            var param = new MatchWinLossDetailParam(
                anyValueCreator.Create<int>(),
                fromDate,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                anyValueCreator.Create<UserLevel>());
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
            var param = new MatchWinLossDetailParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<DateTime>(),
                DateTime.MinValue,
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                anyValueCreator.Create<UserLevel>());
            var expectedDate = DefaultVariables.CurrentDate;

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
            var param = new MatchWinLossDetailParam(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<DateTime>(),
                toDate,
                anyValueCreator.Create<string>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                anyValueCreator.Create<UserLevel>());
            var expectedDate = toDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}
