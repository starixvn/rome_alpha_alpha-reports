namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceMatchingExcelHandlerTests
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly Workbook<BetChoiceMatchingTicketModel> workbook;
        private readonly BetChoiceMatchingWorkbook betChoiceMatchingWorkbook;
        private readonly BetChoiceMatchingExcelHandler handler;
        private readonly IUser user;
        private readonly ReportDateService reportDateService;

        public BetChoiceMatchingExcelHandlerTests(UserFixture userFixture)
        {
            user = userFixture.User;
            reportDateService = Substitute.For<ReportDateService>();
            var reportDate = new BalanceUpDayAndMoveBetsDay(DateTime.Parse("2019-09-09"), DateTime.Parse("2019-08-08"));
            reportDateService.GetBalanceUpDayAndMoveBetsDay().Returns(reportDate);
            betChoiceMatchingService = Substitute.For<BetChoiceMatchingService>();
            workbook = Substitute.For<Workbook<BetChoiceMatchingTicketModel>>();
            betChoiceMatchingWorkbook = Substitute.For<BetChoiceMatchingWorkbook>();
            handler = Substitute.ForPartsOf<BetChoiceMatchingExcelHandler>(betChoiceMatchingService, betChoiceMatchingWorkbook);
        }

        private void Arrange()
        {
            betChoiceMatchingService.ExcelQuery(Arg.Any<BetChoiceMatchingExcelParam>()).Returns(BetChoiceMatchingTestCases.Events);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;
            betChoiceMatchingWorkbook.SetData(Arg.Any<IEnumerable<BetChoiceMatchingEventModel>>()).Returns(betChoiceMatchingWorkbook);
            betChoiceMatchingWorkbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private BetChoiceMatchingExcelQuery CreateMessageWithoutFilter()
        {
            return new BetChoiceMatchingExcelQuery(user, reportDateService)
            {
                WinlostDate = DateTime.Parse("2019-09-10"),
                BetCount = 2
            };
        }

        private BetChoiceMatchingExcelQuery CreateMessageWithFilter()
        {
            return new BetChoiceMatchingExcelQuery(user, reportDateService)
            {
                WinlostDate = DateTime.Parse("2019-09-10"),
                BetCount = 3,
                Stake = 1234.567m,
                MemberWinLossOperator = MemberWinLossOperators.GreatorOrEqual,
                MemberWinLossValue = 2345.678m
            };
        }

        [Fact]
        public void Handle_Returns_CorrectFileName()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithoutFilter();

            // Act
            var actual = handler.Handle(message);

            // Assert
            Assert.Equal("Matching Bet Choice-Name-09102019.xls", actual.Name);
        }

        [Fact]
        public void Handle_Always_UsesCorrectTemplateInfo()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithoutFilter();

            // Act
            handler.Handle(message);

            // Assert
            betChoiceMatchingWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"
                && tp.ColumnStartName == "A"));
        }

        [Fact]
        public void Sheet_Always_UsesCorrectSheetName()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithoutFilter();

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is<string>(t => t == "Sheet 1"));
        }

        [Fact]
        public void Title_Always_UsesCorrectTitleWithoutFilter()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithoutFilter();

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>(t => t == "Matching Bet Choice 09/10/2019 - Matching Bets >= 2"));
        }

        [Fact]
        public void ExportToBytes_Must_BeCalled()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithoutFilter();

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void Title_Always_UsesCorrectTitleWithFilter()
        {
            // Arrange
            Arrange();
            var message = CreateMessageWithFilter();

            // Act
            handler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>(t => t == "Matching Bet Choice 09/10/2019 - Matching Bets >= 3; Total Stake >= 1,235; Total Member WL >= 2,345.68"));
        }
    }
}
