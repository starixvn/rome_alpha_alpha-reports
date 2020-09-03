namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossDetailController winLossDetailController;
        private readonly UserFixture userFixture;

        public WinLossDetailControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            winLossDetailController = new WinLossDetailController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_Always_CallsMediatorSendWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);

            // Act
            winLossDetailController.Index(message);

            // Assert
            mediator.Received().Send<WinLossDetailViewModel>(message);
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailExcelQuery(userFixture.User);
            ControllerBaseFuctions.ArrangeExportExcel<WinLossDetailExcelQuery>(mediator);

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
            var message = new WinLossDetailQuery(user);

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
            var message = new WinLossDetailExcelQuery(user);

            // Act
            var actualView = winLossDetailController.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }

        [Fact]
        public void ReportLegend_Always_CallsMediatorSendReportLegendQuery()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);

            // Act
            winLossDetailController.ReportLegend(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}