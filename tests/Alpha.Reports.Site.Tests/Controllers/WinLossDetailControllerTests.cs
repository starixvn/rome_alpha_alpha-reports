namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossDetailController winLossDetailController;
        private readonly IUser user;

        public WinLossDetailControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossDetailController = new WinLossDetailController(mediator);
            user = Substitute.For<IUser>();
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailQuery(user);

            // Act
            winLossDetailController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossDetailExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}