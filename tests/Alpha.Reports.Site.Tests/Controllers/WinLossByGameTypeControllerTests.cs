namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossByGameType;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossByGameTypeController controller;
        private readonly WinLossByGameTypeQuery winLossByGameTypeQuery;
        private readonly WinLossByGameTypeExcelQuery winLossByGameTypeExcelQuery;

        public WinLossByGameTypeControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new WinLossByGameTypeController(mediator);
            winLossByGameTypeQuery = new WinLossByGameTypeQuery(userFixture.User);
            winLossByGameTypeExcelQuery = new WinLossByGameTypeExcelQuery(userFixture.User);
        }

        [Fact]
        public void WinLossByGameType_Always_ReturnIndex()
        {
            // Arrange

            // Act
            ViewResult result = controller.Index(winLossByGameTypeQuery) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void WinLossByGameType_Always_CallsMediator()
        {
            // Arrange

            // Act
            controller.Index(winLossByGameTypeQuery);

            // Assert
            mediator.Received().Send(Arg.Any<WinLossByGameTypeQuery>());
        }

        [Fact]
        public void WinLossByGameType_CallsMediator_MustSendWinLossByGameTypeQuery()
        {
            // Arrange

            // Act
            controller.Index(winLossByGameTypeQuery);

            // Assert
            mediator.Received().Send(Arg.Is(winLossByGameTypeQuery));
        }

        [Fact]
        public void WinLossByGameTypeExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossByGameTypeExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(winLossByGameTypeExcelQuery);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(winLossByGameTypeExcelQuery));
        }
    }
}