namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Services;
    using Site.Controllers;
    using Site.Requests.MatchWinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("UserCollection")]
    public class MatchWinLossControllerTests
    {
        private readonly IMediator mediator;
        private readonly MatchWinLossController matchWinLossController;
        private readonly UserFixture userFixture;

        public MatchWinLossControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            matchWinLossController = new MatchWinLossController(mediator);

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);

            this.userFixture = userFixture;
        }

        [Fact]
        public void Index_ValidQuery_CallsMediatorSendMatchWinLossQuery()
        {
            // Arrange
            var message = new MatchWinLossQuery(userFixture.User);

            // Act
            matchWinLossController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_ValidQuery_CallsMediatorSendMatchWinLossExcelQuery()
        {
            // Arrange
            var message = new MatchWinLossExcelQuery(userFixture.User);
            mediator.Send(Arg.Any<MatchWinLossExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            matchWinLossController.Excel(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Index_QueryIsNotValid_ReturnInvalidReportView()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new MatchWinLossQuery(user);

            // Act
            var actualView = matchWinLossController.Index(message) as ViewResult;

            // Assert
            Assert.Equal("~/Views/Shared/InvalidReport.cshtml", actualView.ViewName);
        }

        [Fact]
        public void Excel_QueryIsNotValid_ReturnInvalidReportExcelFile()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new MatchWinLossExcelQuery(user);

            // Act
            var actualView = matchWinLossController.Excel(message) as FileContentResult;

            // Assert
            Assert.Equal("InvalidReport.xls", actualView.FileDownloadName);
        }
    }
}
