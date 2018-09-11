namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Requests.Outstanding;
    using Alpha.Reports.Site.Tests.Fixtures;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Outstanding")]
    [Collection("UserCollection")]
    public class OutstandingControllerTests
    {
        private readonly IMediator mediator;
        private readonly OutstandingController controller;
        private readonly IUser user;

        public OutstandingControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new OutstandingController(mediator);
            user = userFixture.User;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendOutstandingQuery()
        {
            // Arrange
            var message = new OutstandingQuery(user);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void ReportLegend_Always_CallsMediatorSendReportLegendQuery()
        {
            // Arrange
            var message = new ReportLegendQuery();

            // Act
            controller.ReportLegend(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}
