namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Services;
    using Site.Controllers;
    using Site.Requests.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("UserCollection")]
    public class MatchWinLossDetailControllerTests
    {
        private readonly IMediator mediator;
        private readonly MatchWinLossDetailController matchWinLossDetailController;
        private readonly UserFixture userFixture;

        public MatchWinLossDetailControllerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            mediator = Substitute.For<IMediator>();
            matchWinLossDetailController = new MatchWinLossDetailController(mediator);

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);
        }

        [Fact]
        public void Index_ValidQuery_CallsMediatorSendMatchWinLossDetailQuery()
        {
            // Arrange
            var message = new MatchWinLossDetailQuery(userFixture.User);

            // Act
            matchWinLossDetailController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new MatchWinLossDetailQuery(user);

            // Act
            var actualView = matchWinLossDetailController.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }
    }
}
