namespace Alpha.Reports.Tests.Criteria.Statement
{
    using System;
    using System.Collections;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.Tests.Constants;
    using Alpha.Reports.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("AnyValueCreatorCollection")]
    public class GetStatementCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetStatementCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void GetSettingKey_BetTrans_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, string.Empty);
            var expectedSettingKey = "Statement_Report_GetList";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSettingKey_BetTrans14_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, "_14");
            var expectedSettingKey = "Statement_Report_GetList_14";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSettingKey_Warehouse_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, "_WH");
            var expectedSettingKey = "Statement_Report_GetList_WH";

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
            var criteria = new GetStatementCriteria(
                custId,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var fromDate = SystemTime.Today;

            var criteria = new GetStatementCriteria(
                1,
                fromDate,
                fromDate.AddDays(-1),
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_CalidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetStatementCriteria(
                1,
                SystemTime.Today,
                SystemTime.Today,
                anyValueCreator.Create<DateTime>(),
                anyValueCreator.Create<string>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("_WH")]
        public void GetSpParams_NotBetTrans14_ReturnsCorrectParams(string storeMappingSuffix)
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, SystemTime.Today, SystemTime.Today, DateTime.Now, storeMappingSuffix);
            var expectedParams = new Hashtable
            {
                ["custid"] = 0,
                ["fromdate"] = SystemTime.Today,
                ["todate"] = SystemTime.Today
            };

            // Act
            var actualParams = criteria.GetSpParams();

            // Assert
            Assert.Equal(expectedParams, actualParams);
        }

        [Fact]
        public void GetSpParams_BetTrans14_ReturnsCorrectParams()
        {
            // Arrange
            var criteria = new GetStatementCriteria(0, SystemTime.Today, SystemTime.Today, SystemTime.Today, "_14");
            var expectedParams = new Hashtable
            {
                ["custid"] = 0,
                ["fromdate"] = SystemTime.Today,
                ["todate"] = SystemTime.Today,
                ["balanceupday"] = SystemTime.Today
            };

            // Act
            var actualParams = criteria.GetSpParams();

            // Assert
            Assert.Equal(expectedParams, actualParams);
        }
    }
}
