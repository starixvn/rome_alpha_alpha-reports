namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class WinLossDetailControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossDetailController winLossDetailController;
        private readonly IUserSession userSession;

        public WinLossDetailControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossDetailController = new WinLossDetailController(mediator);
            userSession = Substitute.For<IUserSession>();
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailQuery(userSession);

            // Act
            winLossDetailController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossDetailQuery>(message));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailExcelQuery(userSession);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossDetailExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<WinLossDetailExcelQuery>(message));
        }
    }
}
