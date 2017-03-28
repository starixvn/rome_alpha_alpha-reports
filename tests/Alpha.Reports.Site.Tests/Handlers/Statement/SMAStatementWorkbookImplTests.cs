using System.Globalization;
using System.Threading;
using Alpha.Reports.Site.Tests.Constants;
using NSubstitute;
using ONEbook.SpreadSheets;
using Xunit;

namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    [Trait("Category", "Statement")]
    public class SMAStatementWorkbookImplTests
    {
        private readonly SMAStatementWorkbookImplForTesting workbook;
        private readonly Sheet sheet;

        public SMAStatementWorkbookImplTests()
        {
            workbook = new SMAStatementWorkbookImplForTesting();
            sheet = Substitute.For<Sheet>();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void FillNoInfoRow_Always_FillsDataWithCorrectColumnLength()
        {
            // Act
            workbook.CallFillNoInfoRow(sheet);

            // Assert
            sheet.Received().SetCellRangeValue(3, 0, 3, -2, "No information available.");
        }

        [Fact]
        public void FillNoInfoRow_Always_AlignCenterText()
        {
            // Arrange
            var cell = Substitute.For<Cell>();
            sheet.GetCell(3, 0).Returns(cell);

            // Act
            workbook.CallFillNoInfoRow(sheet);

            // Assert
            cell.Received().SetCenterHorizontalAlignment();
        }

        [Fact]
        public void GetCellValue_NullValue_ReturnSymbolNoDataNumber()
        {
            // Arrange
            decimal? value = null;
            var expectedValue = "--";

            // Act
            var actualValue = workbook.GetCellValue(value);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetCellValue_NotNullValue_ReturnValue()
        {
            // Arrange
            decimal? value = 10;
            decimal expectedValue = 10;

            // Act
            var actualValue = workbook.GetCellValue(value);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
