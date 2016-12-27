namespace Alpha.Reports.Site.Tests.Controllers
{
    using System;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossDetails;
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
            mediator.Received().Send(Arg.Is<WinLossDetailQuery>(m => message == m));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new ExportWinLossDetailQuery(userSession);
            mediator.Send<ExcelFileViewModel>(Arg.Any<ExportWinLossDetailQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<ExportWinLossDetailQuery>(m => message == m));
        }
    }
}
