namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.PokerGamesWinLossDetail;
    using Site.ViewModels;
    using Site.ViewModels.PokerGamesWinLossDetail;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    [Collection("UserCollection")]
    public class PokerGamesWinLossDetailControllerTests
    {
        private readonly IMediator mediator;
        private readonly PokerGamesWinLossDetailController winLossDetailController;
        private readonly UserFixture userFixture;

        public PokerGamesWinLossDetailControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            winLossDetailController = new PokerGamesWinLossDetailController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossDetailQuery()
        {
            // Arrange
            var message = new PokerGamesWinLossDetailQuery(userFixture.User);

            // Act
            winLossDetailController.Index(message);

            // Assert
            mediator.Received().Send<PokerGamesWinLossDetailViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new PokerGamesWinLossDetailExcelQuery(userFixture.User);
            ControllerBaseFuctions.ArrangeExportExcel<PokerGamesWinLossDetailExcelQuery>(mediator);

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new PokerGamesWinLossDetailQuery(user);

            // Act
            var actualView = winLossDetailController.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }

        [Fact]
        public void Excel_QueryIsNotValid_ReturnInvalidReportExcelFile()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new PokerGamesWinLossDetailExcelQuery(user);

            // Act
            var actualView = winLossDetailController.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }

        [Fact]
        public void ReportLegend_Always_CallsMediatorSendReportLegendQuery()
        {
            // Arrange
            var message = new PokerGamesReportLegendQuery();

            // Act
            winLossDetailController.ReportLegend(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}