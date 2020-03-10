namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.Tests.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("AnyValueCreatorCollection")]
    public class BetChoiceMatchingWorkbookTests
    {
        private readonly IBetListExcelBuilder excelBuilder;
        private readonly ITicketHelper ticketHelper;
        private readonly BetChoiceMatchingWorkbook workbook;
        private readonly Sheet sheet;
        private readonly Row row;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public BetChoiceMatchingWorkbookTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
            sheet = Substitute.For<Sheet>();
            row = Substitute.For<Row>();
            excelBuilder = Substitute.For<IBetListExcelBuilder>();
            ticketHelper = Substitute.For<ITicketHelper>();
            workbook = new BetChoiceMatchingWorkbookImpl(excelBuilder, ticketHelper);
        }

        [Fact]
        public void FillNoInfoRow_Always_FillNoInfoRowWithEmptyData()
        {
            // Arrange
            sheet.GetRow(Arg.Any<int>()).Returns(row);
            Reflect.Invoke(workbook, "set_TemplateInfo", new TemplateInfo(
                anyValueCreatorFixture.Create<string>(),
                "No Info",
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                "B"));

            // Act
            Reflect.Invoke(workbook, "FillNoInfoRow", sheet);

            // Assert
            row.Received().CopyCellsFrom(Arg.Any<Row>());
            row.Received().SetCellValue(Arg.Is<int>(x => x == 1), Arg.Is<string>(x => x == "No Info"));
        }
    }
}
