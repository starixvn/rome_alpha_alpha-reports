namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Site.Tests.Constants;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models.Statement;
    using Xunit;

    [Trait("Category", "Statement")]
    public class MemberStatementWorkbookImplTests
    {
        private readonly MemberStatementWorkbookImplForTesting workbook;
        private readonly Sheet sheet;
        private readonly Row row;

        public MemberStatementWorkbookImplTests()
        {
            workbook = new MemberStatementWorkbookImplForTesting();
            sheet = Substitute.For<Sheet>();
            row = Substitute.For<Row>();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void FillHeader_Always_FillsCorrectTextToHeaderRow()
        {
            // Arrange
            sheet.GetRow(1).Returns(row);

            // Act
            workbook.CallFillHeader(sheet);

            // Assert
            row.Received().SetCellValue(0, "Date");
            row.Received().SetCellValue(1, "Opening Balance");
            row.Received().SetCellValue(2, "Turnover");
            row.Received().SetCellValue(3, "Void/Rejected");
            row.Received().SetCellValue(4, "Outstanding");
            row.Received().SetCellValue(5, "Win/Loss");
            row.Received().SetCellValue(6, "Commission");
            row.Received().SetCellValue(7, "Grand Prize");
            row.Received().SetCellValue(8, "Transfer Amt");
            row.Received().SetCellValue(9, "Accumulated Total");
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
            Assert.Equal(11, actualNotedBalanceCellIndex);
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
