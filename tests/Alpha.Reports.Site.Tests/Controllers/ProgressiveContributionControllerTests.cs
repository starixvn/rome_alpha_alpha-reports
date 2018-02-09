namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.ProgressiveContribution;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    [Collection("UserCollection")]
    public class ProgressiveContributionControllerTests
    {
        private readonly IMediator mediator;
        private readonly UserFixture userFixture;
        private readonly ProgressiveContributionController controller;

        public ProgressiveContributionControllerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            mediator = Substitute.For<IMediator>();

            controller = new ProgressiveContributionController(mediator, userFixture.User);
        }

        [Fact]
        public void Index_Always_CallsMediatorSendProgressiveContributionQuery()
        {
            // Arrange
            var message = new ProgressiveContributionQuery(userFixture.User);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendProgressiveContributionExcelQuery()
        {
            // Arrange
            var message = new ProgressiveContributionExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<ProgressiveContributionExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}