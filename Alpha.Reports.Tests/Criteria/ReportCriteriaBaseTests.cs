namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ReportCriteriaBaseTests
    {
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsInvalid(int custId)
        {
            // Arrange
            var expectedIsValid = false;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(custId, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void IsValid_ValidCustId_ReturnsValid(int custId)
        {
            // Arrange
            var expectedIsValid = true;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(custId, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [Test]
        public void IsValid_ToDateLessThanFromDate_ReturnsInvalid()
        {
            // Arrange
            var expectedIsValid = false;
            var fromDate = DateTime.Now;
            var toDate = fromDate.AddDays(-1);
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, 0, 0, fromDate, toDate, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [Test]
        public void IsValid_ToDateNotLessThanFromDate_ReturnsValid()
        {
            // Arrange
            var expectedIsValid = true;
            var fromDate = DateTime.Now;
            var toDate = fromDate;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(1, 0, 0, fromDate, toDate, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public void IsValid_InvalidPageIndex_ReturnsInvalid(int pageIndex)
        {
            // Arrange
            var expectedIsValid = false;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, pageIndex, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        public void IsValid_ValidPageIndex_ReturnsValid(int pageIndex)
        {
            // Arrange
            var expectedIsValid = true;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(1, pageIndex, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }

        [Test]
        public void IsNotValid_ValidCriteria_ReturnsFalse()
        {
            // Arrange
            var expectedIsNotValid = false;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(1, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsNotValid = reportCriteriaBase.IsNotValid();

            // Assert
            Assert.AreEqual(expectedIsNotValid, actualIsNotValid);
        }

        [Test]
        public void IsNotValid_InvalidCriteria_ReturnsTrue()
        {
            // Arrange
            var expectedIsNotValid = true;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualIsNotValid = reportCriteriaBase.IsNotValid();

            // Assert
            Assert.AreEqual(expectedIsNotValid, actualIsNotValid);
        }

        [Test]
        public void GetPageSize_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = 0;
            var expectedValue = initValue;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, 0, initValue, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);

            // Act
            var actualValue = reportCriteriaBase.PageSize;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void GetBalanceUpDay_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = DateTime.Now;
            var expectedValue = initValue;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, 0, 0, DateTime.Now, DateTime.Now, initValue, DateTime.Now);

            // Act
            var actualValue = reportCriteriaBase.BalanceUpDay;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void GetMoveBetsDay_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = DateTime.Now;
            var expectedValue = initValue;
            var reportCriteriaBase = new ReportCriteriaBaseForTesting(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, initValue);

            // Act
            var actualValue = reportCriteriaBase.MoveBetsDay;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
