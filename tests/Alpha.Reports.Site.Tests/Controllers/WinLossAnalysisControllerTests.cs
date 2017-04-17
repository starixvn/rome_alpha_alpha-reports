namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLossAnalysis;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("UserCollection")]
    public class WinLossAnalysisControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossAnalysisController controller;
        private readonly UserFixture userFixture;

        public WinLossAnalysisControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new WinLossAnalysisController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void WinLossAnalysis_Always_CallsMediatorSendWinLossQuery()
        {
            // Arrange
            var message = new WinLossAnalysisQuery(userFixture.User);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void WinLossAnalysisExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            var message = new WinLossAnalysisExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossAnalysisExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<WinLossAnalysisExcelQuery>(message));
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossAnalysisQuery(user);

            // Act
            var actualView = controller.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }

        [Fact]
        public void Excel_QueryIsNotValid_ReturnInvalidReportExcelFile()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossAnalysisExcelQuery(user);

            // Act
            var actualView = controller.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }
    }
}