namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fixtures;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Site.ViewModels.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class WinLossByProductWorkbookImplTests
    {
        private readonly WinLossByProductWorkbookImplForTesting winLossByProductWorkbookImpl;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public WinLossByProductWorkbookImplTests()
        {
            winLossByProductWorkbookImpl = new WinLossByProductWorkbookImplForTesting();
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void FillBodyRows_Always_SetsCorrectRowValues()
        {
            // Arrange
            winLossByProductWorkbookImpl.InitSheet();

            var data = new List<ConsolidationByProductViewModel>
            {
                new ConsolidationByProductViewModel(1, "Product 1", Enumerable.Empty<ConsolidationEntityViewModel>()),
                new ConsolidationByProductViewModel(
                    2,
                    "Product 2",
                    new List<ConsolidationEntityViewModel> { anyValueCreatorFixture.Create<ConsolidationEntityViewModel>() })
            };

            winLossByProductWorkbookImpl.Body(data, null);

            var sheet = Substitute.For<Sheet>();
            sheet.OddRowTemplate = Substitute.For<Row>();

            var row = Substitute.For<Row>();
            sheet.GetRow(Arg.Any<int>()).Returns(row);

            // Act
            winLossByProductWorkbookImpl.CallFillBodyRows(sheet);

            // Assert
            row.Received().SetCellValue(0, "Product 2");
        }
    }
}
