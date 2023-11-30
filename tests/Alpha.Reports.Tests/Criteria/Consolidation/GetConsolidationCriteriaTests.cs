namespace Alpha.Reports.Tests.Criteria.Consolidation
{
    using System;
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
                productIds: anyValueCreatorFixture.Create<string>(),
                productDetails: anyValueCreatorFixture.Create<string>(),
                subfix: string.Empty);

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
                productIds: anyValueCreatorFixture.Create<string>(),
                productDetails: anyValueCreatorFixture.Create<string>(),
                subfix: string.Empty);

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
                productIds: string.Empty,
                productDetails: string.Empty,
                subfix: string.Empty);

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
                productIds: "1",
                productDetails: string.Empty,
                subfix: string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria =
                new GetConsolidationCriteria(1, 3, DateTime.MinValue, DateTime.MinValue, 2, DateTime.MinValue, DateTime.MinValue, string.Empty, string.Empty, string.Empty);

            // Assert
            Assert.True(
                criteria.CustId == 1
                && criteria.Currency == 3
                && criteria.ReportType == 2
                && criteria.BalanceUpDate == DateTime.MinValue
                && criteria.MoveBetsDate == DateTime.MinValue);
        }
    }
}
