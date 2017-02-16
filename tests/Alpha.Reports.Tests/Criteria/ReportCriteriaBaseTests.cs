namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using Xunit;

    public class ReportCriteriaBaseTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsInvalid(int custId)
        {
            // Arrange
            var expectedIsValid = false;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(custId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidCustId_ReturnsValid(int custId)
        {
            // Arrange
            var expectedIsValid = true;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(custId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnsInvalid()
        {
            // Arrange
            var expectedIsValid = false;
            var fromDate = DateTime.Now;
            var toDate = fromDate.AddDays(-1);
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, fromDate, toDate, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateNotLessThanFromDate_ReturnsValid()
        {
            // Arrange
            var expectedIsValid = true;
            var fromDate = DateTime.Now;
            var toDate = fromDate;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(1, fromDate, toDate, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Fact]
        public void IsNotValid_ValidCriteria_ReturnsFalse()
        {
            // Arrange
            var expectedIsNotValid = false;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsNotValid = reportCriteriaBase.IsNotValid();

            // Assert
            Assert.Equal(expectedIsNotValid, actualIsNotValid);
        }

        [Fact]
        public void IsNotValid_InvalidCriteria_ReturnsTrue()
        {
            // Arrange
            var expectedIsNotValid = true;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsNotValid = reportCriteriaBase.IsNotValid();

            // Assert
            Assert.Equal(expectedIsNotValid, actualIsNotValid);
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = DateTime.Now;
            var expectedValue = initValue;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, DateTime.Now, DateTime.Now, initValue, DateTime.Now);

            // Act
            var actualValue = reportCriteriaBase.BalanceUpDay;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = DateTime.Now;
            var expectedValue = initValue;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, DateTime.Now, DateTime.Now, DateTime.Now, initValue);

            // Act
            var actualValue = reportCriteriaBase.MoveBetsDay;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetSettingKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act& Assert
            Assert.Throws<NotImplementedException>(() => reportCriteriaBase.GetSettingKey());
        }
    }
}
