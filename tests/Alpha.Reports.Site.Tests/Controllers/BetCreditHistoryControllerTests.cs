namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Reports.Utils;
    using MediatR;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    [Collection("UserCollection")]
    public class BetCreditHistoryControllerTests
    {
        private readonly IMediator mediator;
        private readonly BetCreditHistoryController controller;
        private readonly IUser user;

        public BetCreditHistoryControllerTests(UserFixture userFixture)
        {
            SystemTime.Today = DateTime.Today;
            mediator = Substitute.For<IMediator>();
            controller = new BetCreditHistoryController(mediator);
            user = userFixture.User;

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
        }

        [Fact]
        public void Search_ValidQuery_CallsMediatorSendBetCreditHistoryQuery()
        {
            // Arrange
            var message = new BetCreditHistoryQuery(user);

            // Act
            controller.Search(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_ValidQuery_CallsMediatorSendBetCreditHistoryExcelQuery()
        {
            // Arrange
            var message = new BetCreditHistoryExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<BetCreditHistoryExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
