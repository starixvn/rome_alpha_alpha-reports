namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using RemotingRef;
    using Reports.Models;
    using Services;
    using Site.Handlers.MatchWinLossDetail;
    using Site.Requests.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("UserCollection")]
    public class MatchWinLossDetailExcelHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly MatchWinLossDetailExcelHandler matchWinLossDetailExcelHandler;

        private readonly MatchWinLossDetailExcelQuery matchWinLossDetailExcelQuery;
        private readonly MatchWinLossDetailWorkbook matchWinLossDetailWorkbook;
        private readonly Workbook<BetTypeWinLossViewModel> onebookWorkbook;
        private readonly CompareLogic compareLogic;

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public MatchWinLossDetailExcelHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            var productService = Substitute.For<ProductService>();

            var leagueService = Substitute.For<LeagueService>();
            leagueService.GetLeagueNames(Arg.Any<IEnumerable<int>>()).Returns(new Dictionary<int, string> { [1837] = "League 1837" });

            var cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetTeamName(Arg.Any<int>()).Returns(callInfo => "Team " + callInfo.Args()[0]);
            cacheRefService.GetBetType(Arg.Any<int>()).Returns(callInfo => "Bet Type " + callInfo.Args()[0]);

            var reportService = Substitute.For<MatchWinLossDetailService>();
            reportService.GetMatchWinLossDetail(Arg.Any<MatchWinLossDetailParam>()).Returns(MatchWinLossDetailTestCases.MemberMatchWinLossDetailData());

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);

            matchWinLossDetailWorkbook = Substitute.For<MatchWinLossDetailWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<BetTypeWinLossViewModel>>();
            matchWinLossDetailWorkbook.SetData(
                Arg.Any<IEnumerable<MatchWinLossDetailViewModel>>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>())
                .Returns(matchWinLossDetailWorkbook);

            matchWinLossDetailWorkbook.SetRowMappers(
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>())
                .Returns(matchWinLossDetailWorkbook);

            matchWinLossDetailWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(Arg.Any<BetTypeWinLossViewModel>(), Arg.Any<Mapper<BetTypeWinLossViewModel>>()).Returns(onebookWorkbook);

            matchWinLossDetailExcelHandler = Substitute.ForPartsOf<MatchWinLossDetailExcelHandler>(
                productService,
                leagueService,
                reportService,
                cacheRefService,
                matchWinLossDetailWorkbook);
            matchWinLossDetailExcelHandler.MapPath = (path) => path;
            matchWinLossDetailExcelHandler.IsFileExist = (path) => true;

            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            matchWinLossDetailExcelQuery = new MatchWinLossDetailExcelQuery(userFixture.User);
            matchWinLossDetailExcelQuery.CustLevelId = 2;
            matchWinLossDetailExcelQuery.FromDate = DefaultVariables.CurrentDate;
            matchWinLossDetailExcelQuery.ToDate = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossDetailExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossDetailExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_SetsDataToWorkbook()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            matchWinLossDetailWorkbook.Received().SetData(
                Arg.Any<IEnumerable<MatchWinLossDetailViewModel>>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>(),
                Arg.Any<BetTypeWinLossViewModel>());
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_SetRowMappers()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            matchWinLossDetailWorkbook.Received().SetRowMappers(
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>(),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>());
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_UsesCorrectTemplate()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            matchWinLossDetailWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_ExportsCorrectSheetName()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_ExportsCorrectTitle()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            onebookWorkbook.Received().Title(Arg.Is("Match Win Loss Detail 12/02/2016 --> 12/02/2016"));
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_ExportsCorrectHeader()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_ExportsCorrectFooter()
        {
            // Arrange
            var expectedExportData = new BetTypeWinLossViewModel(
                id: 0,
                name: null,
                turnover: 2250000,
                grossComm: (decimal)321.3,
                isLive: false,
                memberNetWinLoss: new NetWinLoss(-2238000, (decimal)6.25),
                agentNetWinLoss: new NetWinLoss(3900, (decimal)218.75),
                masterNetWinLoss: null,
                superNetWinLoss: null,
                companyNetWinLoss: new NetWinLoss(null, 225, 2233875));

            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            onebookWorkbook.Received().Footer(
                Arg.Is<BetTypeWinLossViewModel>(actualExportData => compareLogic.Compare(expectedExportData, actualExportData).AreEqual),
                Arg.Any<Mapper<BetTypeWinLossViewModel>>());
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_ExportsExcelToBytes()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_NotNullMessage_CallMapExcelTemplatePath()
        {
            // Act
            matchWinLossDetailExcelHandler.Handle(matchWinLossDetailExcelQuery);

            // Assert
            matchWinLossDetailExcelHandler.Received().GetExcelTemplatePath("Member");
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var query = new MatchWinLossDetailExcelQuery(userFixture.User);
            query.CustLevelId = 3;

            // Act
            matchWinLossDetailExcelHandler.Handle(query);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleMatchWinLossExcelQuery_MAsterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var query = new MatchWinLossDetailExcelQuery(userFixture.User);
            query.CustLevelId = 4;

            // Act
            matchWinLossDetailExcelHandler.Handle(query);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }
    }
}
