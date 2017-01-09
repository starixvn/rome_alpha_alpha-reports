namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;
        private readonly IUser user;

        public WinLossControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossController = new WinLossController(mediator);
            user = Substitute.For<IUser>();
        }

        [Fact]
        public void WinLoss_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossQuery(user);

            // Act
            winLossController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossQuery>(message));
        }

        [Fact]
        public void WinLossExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new WinLossExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<WinLossExcelQuery>(message));
        }
    }
}