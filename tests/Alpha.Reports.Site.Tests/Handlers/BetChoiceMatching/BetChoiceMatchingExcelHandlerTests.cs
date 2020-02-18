namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceMatchingExcelHandlerTests
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly BetChoiceMatchingWorkbook workbook;
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
            workbook = Substitute.For<BetChoiceMatchingWorkbook>();
            handler = new BetChoiceMatchingExcelHandler(betChoiceMatchingService, workbook);
        }

        [Fact]
        public void Handle_Returns_ExcelFileViewModel()
        {
            // Arrange
            var events = BetChoiceMatchingTestCases.Events;
            betChoiceMatchingService.ExcelQuery(Arg.Any<BetChoiceMatchingExcelParam>()).Returns(events);
            var message = new BetChoiceMatchingExcelQuery(user, reportDateService);

            // Act
            // var actual = handler.Handle(message);

            // Assert
            // Assert.Equal("asd", actual.Name);
        }
    }
}
