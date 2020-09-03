namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Site.Tests.Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests;
    using Xunit;

    [Trait("Category", "ReportLegend")]
    [Collection("UserCollection")]
    public class ReportLegendControllerTests
    {
        private readonly IMediator mediator;
        private readonly ReportLegendController reportLegendController;
        private readonly UserFixture userFixture;

        public ReportLegendControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            reportLegendController = new ReportLegendController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendReportLegendQuery()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);

            // Act
            reportLegendController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}
