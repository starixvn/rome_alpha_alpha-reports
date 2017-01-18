namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.MatchWinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("UserCollection")]
    public class MatchWinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly MatchWinLossController matchWinLossController;
        private readonly UserFixture userFixture;

        public MatchWinLossControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            matchWinLossController = new MatchWinLossController(mediator);

            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendMatchWinLossQuery()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            // Act
            matchWinLossController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendMatchWinLossExcelQuery()
        {
            // Arrange
            var message = new MatchWinLossExcelQuery(userFixture.User);
            mediator.Send(Arg.Any<MatchWinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            matchWinLossController.Excel(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}
