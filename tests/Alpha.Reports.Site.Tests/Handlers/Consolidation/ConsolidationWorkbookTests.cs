namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Site.ViewModels.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class ConsolidationWorkbookTests
    {
        private readonly ConsolidationWorkbookForTesting consolidationWorkbook;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly CompareLogic compareLogic;

        public ConsolidationWorkbookTests()
        {
            consolidationWorkbook = new ConsolidationWorkbookForTesting();
            anyValueCreatorFixture = new AnyValueCreatorFixture();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void SetWinLossByUsernameValue_Always_SetsValuesToCorrectColumns()
        {
            // Arrange
            var data = new ConsolidationByUsernameViewModel(
                custId: anyValueCreatorFixture.Create<int>(),
                custLevel: anyValueCreatorFixture.Create<byte>(),
                username: anyValueCreatorFixture.Create<string>(),
                currencyId: anyValueCreatorFixture.Create<byte>(),
                turnover: anyValueCreatorFixture.Create<decimal>(),
                memberWinLoss: anyValueCreatorFixture.Create<decimal>(),
                memberComm: anyValueCreatorFixture.Create<decimal>(),
                memberTotal: anyValueCreatorFixture.Create<decimal>(),
                ownerTurnover: anyValueCreatorFixture.Create<decimal>(),
                ownerWinLoss: anyValueCreatorFixture.Create<decimal>(),
                ownerComm: anyValueCreatorFixture.Create<decimal>(),
                ownerTotal: anyValueCreatorFixture.Create<decimal>(),
                company: anyValueCreatorFixture.Create<decimal>(),
                betCount: anyValueCreatorFixture.Create<int>(),
                currencyName: anyValueCreatorFixture.Create<string>());
            var row = Substitute.For<Row>();

            // Act
            consolidationWorkbook.CallSetWinLossByUsernameValue(data, row);

            // Assert
            row.Received().SetCellValue(1, data.CustLevelName);
            row.Received().SetCellValue(2, data.Username);
            row.Received().SetCellValue(3, data.CurrencyName);
            row.Received().SetCellValue(4, data.Turnover);
            row.Received().SetCellValue(5, data.MemberWinLoss);
            row.Received().SetCellValue(6, data.MemberComm);
            row.Received().SetCellValue(7, data.MemberTotal);
            row.Received().SetCellValue(8, data.OwnerTurnover);
            row.Received().SetCellValue(9, data.OwnerWinLoss);
            row.Received().SetCellValue(10, data.OwnerComm);
            row.Received().SetCellValue(11, data.OwnerTotal);
            row.Received().SetCellValue(12, data.Company);
            row.Received().SetCellValue(13, data.BetCount);
        }

        [Fact]
        public void MergeCells_Always_AddsCorrectMergeRegion()
        {
            // Arrange
            consolidationWorkbook.InitSheet();

            var data = new List<ConsolidationByDateViewModel>
            {
                new ConsolidationByDateViewModel(anyValueCreatorFixture.Create<DateTime>(), Enumerable.Empty<ConsolidationByUsernameViewModel>()),
                new ConsolidationByDateViewModel(
                    anyValueCreatorFixture.Create<DateTime>(),
                    new List<ConsolidationByUsernameViewModel>
                    {
                        anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>(),
                        anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>()
                    })
            };
            consolidationWorkbook.Body(data, null);

            var sheet = Substitute.For<ISheet>();
            var npoiSheet = new NpoiSheet(sheet);

            Func<ConsolidationByDateViewModel, int> getGroupLength = (date) => date.WinLossByUsername.Count();

            var expectedMergeRegion = new CellRangeAddress(3, 4, 0, 0);

            // Act
            consolidationWorkbook.CallMergeCells(npoiSheet, getGroupLength);

            // Assert
            sheet.Received().AddMergedRegion(
                Arg.Is<CellRangeAddress>(mergeRegion => compareLogic.Compare(expectedMergeRegion, mergeRegion).AreEqual));
        }
    }
}
