namespace Alpha.Reports.Tests.Criteria.Statement
{
    using System;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.Tests.Fixtures;
    using Constants;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("AnyValueCreatorCollection")]
    public class TransferDetailsCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public TransferDetailsCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void GetSettingKey_BetTrans_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new TransferDetailsCriteria(0, DateTime.Now, string.Empty);
            var expectedSettingKey = "Statement_Report_TransferDetails";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSettingKey_BetTrans14_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new TransferDetailsCriteria(0, DateTime.Now, "_14");
            var expectedSettingKey = "Statement_Report_TransferDetails_14";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSettingKey_Warehouse_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new TransferDetailsCriteria(0, DateTime.Now, "_WH");
            var expectedSettingKey = "Statement_Report_TransferDetails_WH";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

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
            var criteria = new TransferDetailsCriteria(
                custId,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsTrue()
        {
            // Arrange
            var criteria = new TransferDetailsCriteria(
                1,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void TransferDate_Always_ReturnsTransferDateSetFromCtor()
        {
            // Arrange
            var criteria = new TransferDetailsCriteria(
               1,
               DefaultVariablesTest.CurrentDate,
               anyValueCreator.Create<string>());

            // Act
            var actual = criteria.TransferDate;

            // Assert
            Assert.Equal(DefaultVariablesTest.CurrentDate, actual);
        }
    }
}
