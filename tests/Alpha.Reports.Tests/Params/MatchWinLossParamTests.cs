namespace Alpha.Reports.Tests.Params
{
    using System;
    using Constants;
    using Fixtures;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossParamTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossParamTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
            DateTimeManager.Now = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromConstructor()
        {
            // Arrange
            var custId = anyValueCreatorFixture.Create<int>();
            var matchWinLossParam = new MatchWinLossParam(
                custId,
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>());
            var expectedCustId = custId;

            // Act
            var actualCustId = matchWinLossParam.CustId;

            // Assert
            Assert.Equal(expectedCustId, actualCustId);
        }

        [Fact]
        public void FromDate_MinValue_ReturnsCurrentDate()
        {
            // Arrange
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DateTime.MinValue,
                anyValueCreatorFixture.Create<DateTime>());
            var expectedFromDate = DefaultVariables.CurrentDate;

            // Act
            var actualFromDate = matchWinLossParam.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void FromDate_CurrentDate_ReturnsCurrentDate()
        {
            // Arrange
            var fromDate = DefaultVariables.CurrentDate;
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DateTime.MinValue,
                anyValueCreatorFixture.Create<DateTime>());
            var expectedFromDate = fromDate;

            // Act
            var actualFromDate = matchWinLossParam.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void FromDate_NotMinValueAndCurrentDate_ReturnsCurrentDate()
        {
            // Arrange
            var fromDate = new DateTime(2017, 1, 19);
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                fromDate,
                anyValueCreatorFixture.Create<DateTime>());
            var expectedFromDate = fromDate;

            // Act
            var actualFromDate = matchWinLossParam.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_MinValue_ReturnsCurrentDate()
        {
            // Arrange
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                DateTime.MinValue);
            var expectedToDate = DefaultVariables.CurrentDate;

            // Act
            var actualToDate = matchWinLossParam.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void ToDate_CurrentDate_ReturnsCurrentDate()
        {
            // Arrange
            var toDate = DefaultVariables.CurrentDate;
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                DateTime.MinValue);
            var expectedToDate = toDate;

            // Act
            var actualToDate = matchWinLossParam.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void ToDate_NotMinValueAndCurrentDate_ReturnsCurrentDate()
        {
            // Arrange
            var toDate = new DateTime(2017, 1, 19);
            var matchWinLossParam = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                toDate);
            var expectedToDate = toDate;

            // Act
            var actualToDate = matchWinLossParam.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}
