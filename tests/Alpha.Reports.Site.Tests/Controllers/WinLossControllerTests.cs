namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;
        private readonly IUserSession userSession;

        public WinLossControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossController = new WinLossController(mediator);
            userSession = Substitute.For<IUserSession>();
        }

        [Fact]
        public void EnteredResult_Always_CallsMediatorSendEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredSportResultQuery(userSession);

            // Act
            winLossController.EnteredResult(message);

            // Assert
            mediator.Received().Send(Arg.Is<EnteredSportResultQuery>(message));
        }

        [Fact]
        public void EnteredResultExcel_Always_CallsMediatorSendExportEnteredResultQuery()
        {
            // Arrange
            var message = new Site.Requests.WinLoss.EnteredSportResultExcelQuery(userSession);
            mediator.Send<ExcelFileViewModel>(Arg.Any<EnteredSportResultExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.EnteredResultExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<Site.Requests.WinLoss.EnteredSportResultExcelQuery>(message));
        }

        [Fact]
        public void WinLoss_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossQuery(userSession);

            // Act
            winLossController.WinLoss(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossQuery>(message));
        }

        [Fact]
        public void WinLossExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new WinLossExcelQuery(userSession);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.WinLossExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<WinLossExcelQuery>(message));
        }
    }
}