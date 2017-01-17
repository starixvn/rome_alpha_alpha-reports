namespace Alpha.Reports.Tests.Criteria.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fixtures;
    using Reports.Criteria.MatchWinLossDetail;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossDetailCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public MatchWinLossDetailCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Theory]
        [InlineData(2, "Age_Rpt_MatchWinLossDetail_Agent")]
        [InlineData(3, "Age_Rpt_MatchWinLossDetail_Master")]
        [InlineData(4, "Age_Rpt_MatchWinLossDetail_Super")]
        public void GetSettingKey_Always_ReturnsValidSettingKey(
            byte userLevelId,
            string expectedSettingKey)
        {
            // Arrange
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
                anyValueCreator.Create<int>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                Enumeration.FromValue<UserLevel>(userLevelId),
                anyValueCreator.Create<string>());

            // Act
            var actualSettingKey = matchWinLossDetailCriteria.GetSettingKey();

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
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
                custId,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<IEnumerable<int>>(),
                anyValueCreator.Create<UserLevel>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_EmptyProducts_ReturnsFalse()
        {
            // Arrange
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
               anyValueCreator.Create<int>(),
               anyValueCreator.Create<DateTime>(),
               anyValueCreator.Create<DateTime>(),
               anyValueCreator.Create<DateTime>(),
               anyValueCreator.Create<DateTime>(),
               Enumerable.Empty<int>(),
               anyValueCreator.Create<UserLevel>(),
               anyValueCreator.Create<string>());

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = anyValueCreator.Create<DateTime>();
            var toDate = fromDate.AddDays(-1);
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
               anyValueCreator.Create<int>(),
               fromDate,
               toDate,
               anyValueCreator.Create<DateTime>(),
               anyValueCreator.Create<DateTime>(),
               anyValueCreator.Create<IEnumerable<int>>(),
               anyValueCreator.Create<UserLevel>(),
               anyValueCreator.Create<string>());

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsValid_NullOrWhiteSpaceLeagues_ReturnsFalse(string leagues)
        {
            // Arrange
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
             anyValueCreator.Create<int>(),
             anyValueCreator.Create<DateTime>(),
             anyValueCreator.Create<DateTime>(),
             anyValueCreator.Create<DateTime>(),
             anyValueCreator.Create<DateTime>(),
             anyValueCreator.Create<IEnumerable<int>>(),
             anyValueCreator.Create<UserLevel>(),
             leagues);

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var fromDate = anyValueCreator.Create<DateTime>();
            var toDate = fromDate;
            var matchWinLossDetailCriteria = new MatchWinLossDetailCriteria(
                1,
                fromDate,
                toDate,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                new List<int> { 1, 2, 3 },
                anyValueCreator.Create<UserLevel>(),
                "4,5,6");

            // Act
            var actualIsValid = matchWinLossDetailCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
