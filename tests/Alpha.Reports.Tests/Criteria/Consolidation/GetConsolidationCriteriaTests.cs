namespace Alpha.Reports.Tests.Criteria.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fixtures;
    using Reports.Criteria.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("AnyValueCreatorCollection")]
    public class GetConsolidationCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public GetConsolidationCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<GetConsolidationCriteria>();
            var expectedSettingKey = "Age_Rpt_ConsolidatedGroup";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetConsolidationCriteria(
                custId,
                currency: anyValueCreatorFixture.Create<int>(),
                fromDate: anyValueCreatorFixture.Create<DateTime>(),
                toDate: anyValueCreatorFixture.Create<DateTime>(),
                reportType: anyValueCreatorFixture.Create<byte>(),
                balanceUpDate: anyValueCreatorFixture.Create<DateTime>(),
                moveBetsDate: anyValueCreatorFixture.Create<DateTime>(),
                productIds: anyValueCreatorFixture.Create<IEnumerable<int>>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = fromDate.AddDays(-1);

            var criteria = new GetConsolidationCriteria(
                custId: anyValueCreatorFixture.Create<int>(),
                currency: anyValueCreatorFixture.Create<int>(),
                fromDate: fromDate,
                toDate: toDate,
                balanceUpDate: anyValueCreatorFixture.Create<DateTime>(),
                moveBetsDate: anyValueCreatorFixture.Create<DateTime>(),
                reportType: anyValueCreatorFixture.Create<byte>(),
                productIds: anyValueCreatorFixture.Create<IEnumerable<int>>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_EmptyProductIds_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetConsolidationCriteria(
                custId: anyValueCreatorFixture.Create<int>(),
                currency: anyValueCreatorFixture.Create<int>(),
                fromDate: anyValueCreatorFixture.Create<DateTime>(),
                toDate: anyValueCreatorFixture.Create<DateTime>(),
                balanceUpDate: anyValueCreatorFixture.Create<DateTime>(),
                moveBetsDate: anyValueCreatorFixture.Create<DateTime>(),
                reportType: anyValueCreatorFixture.Create<byte>(),
                productIds: Enumerable.Empty<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var balanceUpDate = anyValueCreatorFixture.Create<DateTime>();

            var criteria = new GetConsolidationCriteria(
                custId: 1,
                currency: anyValueCreatorFixture.Create<int>(),
                fromDate: balanceUpDate,
                toDate: balanceUpDate,
                balanceUpDate: balanceUpDate,
                moveBetsDate: anyValueCreatorFixture.Create<DateTime>(),
                reportType: anyValueCreatorFixture.Create<byte>(),
                productIds: new List<int> { 1 });

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
