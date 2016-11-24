namespace Alpha.Reports.Tests.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Reports.Criteria;

    [TestFixture]
    public class ReportProductCriteriaTests
    {
        [Test]
        public void GetProductIds_EmptyProductList_ReturnEmpty()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Enumerable.Empty<int>());
            var expectedProductIds = string.Empty;

            // Act
            var actualProductIds = reportProductCriteria.ProductIds;

            // Assert
            Assert.AreEqual(expectedProductIds, actualProductIds);
        }

        [Test]
        public void GetProductIds_WithProductList_ReturnCorrectProductIds()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                new List<int>() { 1, 2 });

            var expectedProductIds = "1,2";

            // Act
            var actualProductIds = reportProductCriteria.ProductIds;

            // Assert
            Assert.AreEqual(expectedProductIds, actualProductIds);
        }

        [Test]
        public void GetSettingKey_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Enumerable.Empty<int>());

            // Act & Assert
            Assert.Throws<NotImplementedException>(new TestDelegate(() => reportProductCriteria.GetSettingKey()));
        }

        [Test]
        public void IsValid_EmptyProductIds_ReturnInvalid()
        {
            // Arrange
            var reportProductCriteria = new ReportProductCriteria(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Enumerable.Empty<int>());
            var expectedIsValid = false;

            // Act
            var actualIsValid = reportProductCriteria.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }
    }
}
