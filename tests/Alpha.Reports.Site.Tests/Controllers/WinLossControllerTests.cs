namespace Alpha.Reports.Site.Tests.Controllers
{
    using System;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossControllerTests : IDisposable
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;

        public WinLossControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossController = new WinLossController(mediator);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                winLossController.Dispose();
            }
        }

        [Fact]
        public void EnteredResult_Always_CallsMediatorSendEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredResultQuery();

            // Act
            winLossController.EnteredResult(message);

            // Assert
            mediator.Received().Send(Arg.Is<EnteredResultQuery>(m => message == m));
        }

        [Fact]
        public void EnteredResultExcel_Always_CallsMediatorSendExportEnteredResultQuery()
        {
            // Arrange
            var message = new ExportEnteredResultQuery();
            mediator.Send<ExcelFileViewModel>(Arg.Any<ExportEnteredResultQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.EnteredResultExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<ExportEnteredResultQuery>(m => message == m));
        }

        [Fact]
        public void WinLoss_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossQuery();

            // Act
            winLossController.WinLoss(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossQuery>(m => message == m));
        }

        [Fact]
        public void WinLossExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new ExportWinLossQuery();
            mediator.Send<ExcelFileViewModel>(Arg.Any<ExportWinLossQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossController.WinLossExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<ExportWinLossQuery>(m => message == m));
        }
    }
}
