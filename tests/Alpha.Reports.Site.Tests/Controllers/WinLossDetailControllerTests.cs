namespace Alpha.Reports.Site.Tests.Controllers
{
    using System;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss Detail")]
    public class WinLossDetailControllerTests : IDisposable
    {
        private readonly IMediator mediator;
        private readonly WinLossDetailController winLossDetailController;

        public WinLossDetailControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            winLossDetailController = new WinLossDetailController(mediator);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            winLossDetailController.Dispose();
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailQuery();

            // Act
            winLossDetailController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is<WinLossDetailQuery>(m => message == m));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new ExportWinLossDetailQuery();
            mediator.Send<ExcelFileViewModel>(Arg.Any<ExportWinLossDetailQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<ExportWinLossDetailQuery>(m => message == m));
        }
    }
}
