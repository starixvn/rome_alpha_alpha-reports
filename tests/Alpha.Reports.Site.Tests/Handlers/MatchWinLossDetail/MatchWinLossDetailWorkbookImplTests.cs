namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLossDetail
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Constants;
    using Reports.Models.MatchWinLossDetail;    
    using Site.Handlers.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using Utils;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossDetailWorkbookImplTests
    {
        private readonly MatchWinLossDetailWorkbookImpl matchWinLossDetailWorkbookImpl;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        private readonly Sheet sheet;
        private readonly Row row;

        public MatchWinLossDetailWorkbookImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            matchWinLossDetailWorkbookImpl = new MatchWinLossDetailWorkbookImpl();
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            sheet = Substitute.For<Sheet>();
            row = Substitute.For<Row>();

            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void SetData_Always_SetsCorrectData()
        {
            // Arrange
            var noParlayMatchWinLossDetail = anyValueCreatorFixture.Create<IEnumerable<MatchWinLossDetailViewModel>>();
            var noParlayWinLossTotal = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            var mixParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            var systemParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            var leapGamingsystemParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            var grandTotal = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();

            // Act
            matchWinLossDetailWorkbookImpl.SetData(noParlayMatchWinLossDetail, noParlayWinLossTotal, mixParlay, systemParlay, leapGamingsystemParlay, grandTotal);

            // Assert
            Assert.True(matchWinLossDetailWorkbookImpl.NoParlayMatchWinLossDetail == noParlayMatchWinLossDetail
                && matchWinLossDetailWorkbookImpl.NoParlayWinLossTotal == noParlayWinLossTotal
                && matchWinLossDetailWorkbookImpl.MixParlay == mixParlay
                && matchWinLossDetailWorkbookImpl.SystemParlay == systemParlay
                && matchWinLossDetailWorkbookImpl.GrandTotal == grandTotal);
        }

        [Fact]
        public void SetRowMappers_Always_SetsCorrectData()
        {
            // Arrange
            var betTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            var subTotalRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            var totalRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            var grandTotalRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();

            // Act
            matchWinLossDetailWorkbookImpl.SetRowMappers(betTypeRowMapper, subTotalRowMapper, totalRowMapper, grandTotalRowMapper);

            // Assert
            Assert.True(matchWinLossDetailWorkbookImpl.BetTypeRowMapper == betTypeRowMapper
                   && matchWinLossDetailWorkbookImpl.SubTotalRowMapper == subTotalRowMapper
                   && matchWinLossDetailWorkbookImpl.TotalRowMapper == totalRowMapper
                   && matchWinLossDetailWorkbookImpl.GrandTotalRowMapper == grandTotalRowMapper);
        }

        [Fact]
        public void FillMatchTitle_Always_SetsCorrectCellRangeValue()
        {
            // Arrange
            var rowIndex = 0;
            var data = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                "League Name",
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                new MatchWinLossDetailModel(false, 1, 1, 1, 1, false, 1,(byte)LeagueType.Self, 1, string.Empty, false, string.Empty, 0, 0, null, null, null, null),
                "Team 1",
                "Team 1");

            // Act
            matchWinLossDetailWorkbookImpl.FillMatchTitle(sheet, rowIndex, data);

            // Assert
            sheet.Received().SetCellRangeValue(rowIndex, 0, rowIndex, -1, "LEAGUE NAME >> TEAM 1 - VS - TEAM 1");
        }

        [Fact]
        public void FillMatchTitle_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            var data = anyValueCreatorFixture.Create<MatchWinLossDetailViewModel>();
            sheet.GetRow(rowIndex).Returns(row);

            var matchTitleRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.MatchTitleRowTemplate = matchTitleRowTemplate;

            // Act
            matchWinLossDetailWorkbookImpl.FillMatchTitle(sheet, rowIndex, data);

            // Assert
            row.Received().CopyCellsFrom(matchTitleRowTemplate);
        }

        [Fact]
        public void FillBetTypeWinLoss_EvenRow_UsesEvenRowTemplate()
        {
            // Arrange
            var rowIndex = 0;
            var data = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            sheet.GetRow(rowIndex).Returns(row);

            var evenRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.EvenRowTemplate = evenRowTemplate;
            matchWinLossDetailWorkbookImpl.BetTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();

            // Act
            matchWinLossDetailWorkbookImpl.FillBetTypeWinLoss(sheet, rowIndex, true, data);

            // Assert
            row.Received().CopyCellsFrom(evenRowTemplate);
        }

        [Fact]
        public void FillBetTypeWinLoss_OddRow_UsesOddRowTemplate()
        {
            // Arrange
            var rowIndex = 0;
            var data = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            sheet.GetRow(rowIndex).Returns(row);

            var oddRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.OddRowTemplate = oddRowTemplate;
            matchWinLossDetailWorkbookImpl.BetTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();

            // Act
            matchWinLossDetailWorkbookImpl.FillBetTypeWinLoss(sheet, rowIndex, false, data);

            // Assert
            row.Received().CopyCellsFrom(oddRowTemplate);
        }

        [Fact]
        public void FillMatchTotal_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            var data = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            sheet.GetRow(rowIndex).Returns(row);

            var subTotalRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.SubTotalRowTemplate = subTotalRowTemplate;
            matchWinLossDetailWorkbookImpl.SubTotalRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();

            // Act
            matchWinLossDetailWorkbookImpl.FillMatchTotal(sheet, rowIndex, data);

            // Assert
            row.Received().CopyCellsFrom(subTotalRowTemplate);
        }

        [Fact]
        public void FillTotal_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            var data = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();
            sheet.GetRow(rowIndex).Returns(row);

            var totalRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.TotalRowTemplate = totalRowTemplate;
            matchWinLossDetailWorkbookImpl.TotalRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();

            // Act
            matchWinLossDetailWorkbookImpl.FillTotal(sheet, rowIndex, data);

            // Assert
            row.Received().CopyCellsFrom(totalRowTemplate);
        }

        [Fact]
        public void FillMixParlayWinLoss_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            sheet.GetRow(rowIndex).Returns(row);

            var mixParlayRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.MixParlayRowTemplate = mixParlayRowTemplate;
            matchWinLossDetailWorkbookImpl.BetTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            matchWinLossDetailWorkbookImpl.MixParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();

            // Act
            matchWinLossDetailWorkbookImpl.FillMixParlayWinLoss(sheet, rowIndex);

            // Assert
            row.Received().CopyCellsFrom(mixParlayRowTemplate);
        }

        [Fact]
        public void FillSystemParlayWinLoss_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            sheet.GetRow(rowIndex).Returns(row);

            var systemParlayRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.SystemParlayRowTemplate = systemParlayRowTemplate;
            matchWinLossDetailWorkbookImpl.BetTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            matchWinLossDetailWorkbookImpl.SystemParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();

            // Act
            matchWinLossDetailWorkbookImpl.FillSystemParlayWinLoss(sheet, rowIndex);

            // Assert
            row.Received().CopyCellsFrom(systemParlayRowTemplate);
        }

        [Fact]
        public void FillLeapGamingSystemParlayWinLoss_Always_UsesCorrectTemplate()
        {
            // Arrange
            var rowIndex = 0;
            sheet.GetRow(rowIndex).Returns(row);

            var systemParlayRowTemplate = Substitute.For<Row>();
            matchWinLossDetailWorkbookImpl.SystemParlayRowTemplate = systemParlayRowTemplate;
            matchWinLossDetailWorkbookImpl.BetTypeRowMapper = anyValueCreatorFixture.Create<Mapper<BetTypeWinLossViewModel>>();
            matchWinLossDetailWorkbookImpl.LeapGamingSystemParlay = anyValueCreatorFixture.Create<BetTypeWinLossViewModel>();

            // Act
            matchWinLossDetailWorkbookImpl.FillSystemParlayWinLoss(sheet, rowIndex);

            // Assert
            row.Received().CopyCellsFrom(systemParlayRowTemplate);
        }
        [Fact]
        public void GrandTotalRowTemplate_Always_ReturnsCorrectValue()
        {
            // Arrange
            var row = Substitute.For<Row>();

            // Act
            matchWinLossDetailWorkbookImpl.GrandTotalRowTemplate = row;

            // Assert
            Assert.Equal(row, matchWinLossDetailWorkbookImpl.GrandTotalRowTemplate);
        }
    }
}
