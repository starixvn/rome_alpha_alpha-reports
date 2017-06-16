namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Site.ViewModels.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class WinLossByDateWorkbookImplTests
    {
        private readonly WinLossByDateWorkbookImplForTesting winLossByDateWorkbookImpl;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public WinLossByDateWorkbookImplTests()
        {
            winLossByDateWorkbookImpl = new WinLossByDateWorkbookImplForTesting();
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void FillBodyRows_Always_SetsCorrectRowValues()
        {
            // Arrange
            winLossByDateWorkbookImpl.InitSheet();

            var data = new List<ConsolidationByDateViewModel>
            {
                new ConsolidationByDateViewModel(DefaultVariables.CurrentDate, Enumerable.Empty<ConsolidationByUsernameViewModel>()),
                new ConsolidationByDateViewModel(
                    DefaultVariables.CurrentDate,
                    new List<ConsolidationByUsernameViewModel> { anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>() })
            };

            winLossByDateWorkbookImpl.Body(data, null);

            var sheet = Substitute.For<Sheet>();
            sheet.OddRowTemplate = Substitute.For<Row>();

            var row = Substitute.For<Row>();
            sheet.GetRow(Arg.Any<int>()).Returns(row);

            // Act
            winLossByDateWorkbookImpl.CallFillBodyRows(sheet);

            // Assert
            row.Received().SetCellValue(0, "12/02/2016");
        }
    }
}
