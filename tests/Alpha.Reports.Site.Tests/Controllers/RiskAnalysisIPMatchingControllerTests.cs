namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Constants;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Services;
    using Site.Controllers;
    using Site.ViewModels;
    using System.Web.Mvc;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("UserCollection")]
    public class RiskAnalysisIPMatchingControllerTests
    {
        private readonly IMediator mediator;
        private readonly RiskAnalysisIPMatchingController riskAnalysisIPMatchingController;
        private readonly UserFixture userFixture;

        public RiskAnalysisIPMatchingControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            riskAnalysisIPMatchingController = new RiskAnalysisIPMatchingController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_ValidQuery_CallsMediatorSendRiskAnalysisIPMatchingQuery()
        {
            // Arrange
            var message = new RiskAnalysisIPMatchingQuery(userFixture.User);

            // Act
            riskAnalysisIPMatchingController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_ValidQuery_CallsMediatorSendRiskAnalysisIPMatchingExcelQuery()
        {
            // Arrange
            var message = new RiskAnalysisIPMatchingExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<RiskAnalysisIPMatchingExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            riskAnalysisIPMatchingController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new RiskAnalysisIPMatchingQuery(user);

            // Act
            var actualView = riskAnalysisIPMatchingController.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }

        [Fact]
        public void Excel_QueryIsNotValid_ReturnInvalidReportExcelFile()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new RiskAnalysisIPMatchingExcelQuery(user);

            // Act
            var actualView = riskAnalysisIPMatchingController.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }
    }
}
