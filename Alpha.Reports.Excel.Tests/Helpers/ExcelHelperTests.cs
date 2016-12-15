namespace Alpha.Reports.Excel.Tests.Helpers
{
    using Excel.Helpers;
    using Xunit;

    public class ExcelHelperTests
    {
        [Fact]
        public void ContentType_Always_ReturnsMSExcel()
        {
            // Arrange
            var expectedContentType = "application/vnd.ms-excel";

            // Act
            var actualContentType = ExcelHelper.ContentType;

            // Assert
            Assert.Equal(expectedContentType, actualContentType);
        }
    }
}
