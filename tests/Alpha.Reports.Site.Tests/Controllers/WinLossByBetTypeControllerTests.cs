namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossByBetTypeControllerTests")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;
        private readonly UserFixture userFixture;

        public WinLossByBetTypeControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            winLossController = new WinLossController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void WinLoss_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossQuery(userFixture.User);

            // Act
            winLossController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossQuery>(message));
        }

        [Fact]
        public void WinLossExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new WinLossExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<WinLossExcelQuery>(message));
        }
    }
}