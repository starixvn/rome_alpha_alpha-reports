namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests;
    using Xunit;

    [Trait("Category", "ReportLegend")]
    public class ReportLegendControllerTests
    {
        private readonly IMediator mediator;
        private readonly ReportLegendController reportLegendController;

        public ReportLegendControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            reportLegendController = new ReportLegendController(mediator);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendReportLegendQuery()
        {
            // Arrange
            var message = new ReportLegendQuery();

            // Act
            reportLegendController.Index(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}
