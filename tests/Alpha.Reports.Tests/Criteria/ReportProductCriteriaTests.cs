namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using Reports.Criteria;
    using Xunit;

    public class ReportProductCriteriaTests
    {
        [Fact]
        public void GetProductIds_EmptyProductList_ReturnsEmpty()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, string.Empty);
            var expectedProductIds = string.Empty;

            // Act
            var actualProductIds = reportProductCriteria.ProductIds;

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void GetProductIds_WithProductList_ReturnsCorrectProductIds()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1,2");

            var expectedProductIds = "1,2";

            // Act
            var actualProductIds = reportProductCriteria.ProductIds;

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void GetSettingKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, string.Empty);

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => reportProductCriteria.GetSettingKey());
        }

        [Fact]
        public void IsValid_EmptyProductIds_ReturnsInvalid()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, string.Empty);
            var expectedIsValid = false;

            // Act
            var actualIsValid = reportProductCriteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
