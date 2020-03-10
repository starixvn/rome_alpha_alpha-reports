namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceMatchingControllerTests
    {
        private readonly IMediator mediator;
        private readonly BetChoiceMatchingController controller;
        private readonly IUser user;
        private readonly ReportDateService reportDateService;

        public BetChoiceMatchingControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new BetChoiceMatchingController(mediator);
            user = userFixture.User;
            reportDateService = Substitute.For<ReportDateService>();
            var reportDate = new BalanceUpDayAndMoveBetsDay(DateTime.Parse("2019-09-09"), DateTime.Parse("2019-08-08"));
            reportDateService.GetBalanceUpDayAndMoveBetsDay().Returns(reportDate);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendCommissionByBettypeQuery()
        {
            // Arrange
            var message = new BetChoiceMatchingQuery(user);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Query_Always_CallsMediatorSendBetChoiceMatchingJsonQuery()
        {
            // Arrange
            var message = new BetChoiceMatchingJsonQuery(user, reportDateService)
            {
                WinlostDate = DateTime.Parse("2019-09-10"),
                BetCount = 2
            };

            // Act
            controller.Query(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendBetChoiceMatchingExcelQuery()
        {
            // Arrange
            var message = new BetChoiceMatchingExcelQuery(user, reportDateService)
            {
                WinlostDate = DateTime.Parse("2019-09-10"),
                BetCount = 2
            };
            mediator.Send<ExcelFileViewModel>(Arg.Any<BetChoiceMatchingExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void Uplines_Always_CallsMediatorSendBetChoiceUplinesJsonQuery()
        {
            // Arrange
            var message = new BetChoiceUplinesJsonQuery(user)
            {
                SearchAccountId = 1
            };

            // Act
            controller.Uplines(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}
