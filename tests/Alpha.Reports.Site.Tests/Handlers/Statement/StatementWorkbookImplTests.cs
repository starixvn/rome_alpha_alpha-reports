namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Site.Handlers.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models.Statement;
    using Xunit;

    [Trait("Category", "Statement")]
    public class StatementWorkbookImplTests
    {
        private readonly StatementWorkbookImplForTesting workbook;
        private readonly Sheet sheet;

        public StatementWorkbookImplTests()
        {
            workbook = new StatementWorkbookImplForTesting();
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
            var actualValue = StatementWorkbookImpl.GetCellValue(value);

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
            var actualValue = StatementWorkbookImpl.GetCellValue(value);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetNotedBalanceCellIndex_Always_ReturnsCorrectValue()
        {
            // Arrange
            var row = Substitute.For<Row>();
            row.FirstCellIndex.Returns((short)1);

            // Act
            var actualNotedBalanceCellIndex = workbook.CallGetNotedBalanceCellIndex(row);

            // Assert
            Assert.Equal(8, actualNotedBalanceCellIndex);
        }

        [Fact]
        public void ExportWinLoss_Always_SetsValuesToCells()
        {
            // Arrange
            var row = Substitute.For<Row>();
            var data = new StatementModel(DefaultVariables.CurrentDate, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            // Act
            workbook.CallExportWinLoss(row, data, 0);

            // Assert
            row.Received().SetCellValue(0, (decimal)5);
        }
    }
}
