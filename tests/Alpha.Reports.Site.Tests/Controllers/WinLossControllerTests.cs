namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossController winLossController;
        private readonly UserFixture userFixture;

        public WinLossControllerTests(UserFixture userFixture)
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
            mediator.Received().Send(message);
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

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossQuery(user);

            // Act
            var actualView = winLossController.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }

        [Fact]
        public void Excel_QueryIsNotValid_ReturnInvalidReportExcelFile()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossExcelQuery(user);

            // Act
            var actualView = winLossController.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }
    }
}