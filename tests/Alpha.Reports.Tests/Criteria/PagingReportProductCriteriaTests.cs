namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using System.Collections.Generic;
    using Reports.Criteria;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class PagingReportProductCriteriaTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidPageIndex_ReturnsInvalid(int pageIndex)
        {
            // Arrange
            var expectedIsValid = false;
            var reportCriteriaBase = new PagingReportProductCriteria(0, pageIndex, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, "1,2");

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidPageIndex_ReturnsValid(int pageIndex)
        {
            // Arrange
            var expectedIsValid = true;
            var reportCriteriaBase = new PagingReportProductCriteria(1, pageIndex, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, "1,2");

            // Act
            var actualIsValid = reportCriteriaBase.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Fact]
        public void GetPageSize_Always_ReturnsInitialValue()
        {
            // Arrange
            var initValue = 0;
            var expectedValue = initValue;
            var reportCriteriaBase = new PagingReportProductCriteria(0, 0, initValue, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, "1,2");

            // Act
            var actualValue = reportCriteriaBase.PageSize;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
