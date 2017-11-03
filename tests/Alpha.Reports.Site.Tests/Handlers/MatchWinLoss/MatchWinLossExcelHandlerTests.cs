namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Reports.Models.MatchWinLoss;
    using Services;
    using Site.Handlers.MatchWinLoss;
    using Site.Requests.MatchWinLoss;
    using Site.ViewModels.MatchWinLoss;
    using Utils;
    using Xunit;
    using static ONEbook.SpreadSheets.Column;

    [Trait("Category", "MatchWinLoss")]
    [Collection("UserCollection")]
    public class MatchWinLossExcelHandlerTests
    {
        private readonly Workbook<MatchWinLossViewModel> workbook;
        private readonly MatchWinLossService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly MatchWinLossExcelHandler matchWinLossExcelHandler;
        private readonly IEnumerable<MatchWinLossModel> rawData;
        private readonly MatchWinLossExcelQuery matchWinLossExcelQuery;
        private readonly CompareLogic compareLogic;

        public MatchWinLossExcelHandlerTests(UserFixture userFixture)
        {
            workbook = Substitute.For<Workbook<MatchWinLossViewModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<MatchWinLossViewModel>>(), Arg.Any<Mapper<MatchWinLossViewModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<MatchWinLossViewModel>(), Arg.Any<Mapper<MatchWinLossViewModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            matchWinLossExcelQuery = new MatchWinLossExcelQuery(userFixture.User);
            matchWinLossExcelQuery.CustLevelId = 2;
            matchWinLossExcelQuery.FromDate = DefaultVariables.CurrentDate;
            matchWinLossExcelQuery.ToDate = DefaultVariables.CurrentDate;

            reportService = Substitute.For<MatchWinLossService>();
            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetTeamName(Arg.Any<int>()).Returns(callInfo => "Team " + callInfo.Args()[0]);
            cacheRefService.GetBetType(Arg.Any<int>()).Returns(callInfo => "Bet Type " + callInfo.Args()[0]);

            matchWinLossExcelHandler = Substitute.ForPartsOf<MatchWinLossExcelHandler>(reportService, cacheRefService, workbook);
            matchWinLossExcelHandler.MapPath = (path) => path;
            matchWinLossExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            rawData = new List<MatchWinLossModel>
            {
                new MatchWinLossModel(1, 2, 3, 4, false, false, "Match Code", "Show Time", 0, 0, 0),
                new MatchWinLossModel(5, 6, 7, 8, false, false, "Match Code", "Show Time", 0, 0, 0)
            };

            reportService.GetMatchWinLoss(Arg.Any<MatchWinLossParam>()).Returns(rawData);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_UsesCorrectTemplateInfo()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"
                && tp.FreezeColumnName == A
                && tp.FreezeRowIndex == -1));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsCorrectTitle()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().Title(Arg.Is("Match Win Loss 12/02/2016 --> 12/02/2016"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsCorrectHeader()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsCorrectBody()
        {
            // Arrange
            var expectedExportData = new List<MatchWinLossViewModel>
            {
                new MatchWinLossViewModel("Team 2 - vs - Team 3", "Bet Type 4", 1, 4, false, 0, 0, 0),
                new MatchWinLossViewModel("Team 6 - vs - Team 7", "Bet Type 8", 5, 8, false, 0, 0, 0)
            };

            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<MatchWinLossViewModel>>(actualExportData => compareLogic.Compare(expectedExportData, actualExportData).AreEqual),
                Arg.Any<Mapper<MatchWinLossViewModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsCorrectFooter()
        {
            // Arrange
            var expectedExportData = new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 0, 0, 0);

            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().Footer(
                Arg.Is<MatchWinLossViewModel>(actualExportData => compareLogic.Compare(expectedExportData, actualExportData).AreEqual),
                Arg.Any<Mapper<MatchWinLossViewModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsCorrectSheetName()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_Always_ExportsExcelToBytes()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossQuery_CallMapExcelTemplatePath()
        {
            // Act
            matchWinLossExcelHandler.Handle(matchWinLossExcelQuery);

            // Assert
            matchWinLossExcelHandler.Received().GetExcelTemplatePath("Member");
        }

        [Fact]
        public void GetRowMapper_BodyRow_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new MatchWinLossViewModel("EventInfo", "BetTypeName", 1, 2, false, 3, 4, 5);

            // Act
            var actualMapper = matchWinLossExcelHandler.GetRowMapper(false);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[A](viewModel) == "EventInfo" &&
                (string)actualMapper.Mappings[B](viewModel) == "BetTypeName" &&
                (decimal)actualMapper.Mappings[C](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[D](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[E](viewModel) == 5 &&
                (decimal)actualMapper.Mappings[F](viewModel) == 9);
        }

        [Fact]
        public void GetRowMapper_TotalRow_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new MatchWinLossViewModel("EventInfo", "BetTypeName", 1, 2, false, 3, 4, 5);

            // Act
            var actualMapper = matchWinLossExcelHandler.GetRowMapper(true);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[A](viewModel) == "Total" &&
                (string)actualMapper.Mappings[B](viewModel) == "BetTypeName" &&
                (decimal)actualMapper.Mappings[C](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[D](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[E](viewModel) == 5 &&
                (decimal)actualMapper.Mappings[F](viewModel) == 9);
        }
    }
}
