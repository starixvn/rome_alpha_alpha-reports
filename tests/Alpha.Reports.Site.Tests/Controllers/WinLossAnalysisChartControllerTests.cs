namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLossAnalysisChart;
    using System.Web.Mvc;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    [Collection("UserCollection")]
    public class WinLossAnalysisChartControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossAnalysisChartController controller;
        private readonly UserFixture userFixture;

        public WinLossAnalysisChartControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new WinLossAnalysisChartController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void WinLossAnalysisChart_Always_CallsMediatorSendWinLossAnalysisChartQuery()
        {
            // Arrange
            var message = new WinLossAnalysisChartQuery(userFixture.User);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnsInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossAnalysisChartQuery(user);

            // Act
            var actualView = controller.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }
    }
}