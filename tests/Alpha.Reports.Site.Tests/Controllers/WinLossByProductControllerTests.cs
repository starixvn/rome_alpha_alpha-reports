namespace Alpha.Reports.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossByProduct;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductControllerTests
    {
        private readonly IMediator mediator;
        private readonly WinLossByProductController controller;
        private readonly WinLossByProductQuery winLossByProductQuery;
        private readonly WinLossByProductExcelQuery winLossByProductExcelQuery;
        private readonly UserFixture userFixture;

        public WinLossByProductControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            this.userFixture = userFixture;
            controller = new WinLossByProductController(mediator);
            winLossByProductQuery = new WinLossByProductQuery(this.userFixture.User);
            winLossByProductExcelQuery = new WinLossByProductExcelQuery(this.userFixture.User);
        }

        [Fact]
        public void WinLossByProduct_Always_ReturnIndex()
        {
            // Arrange

            // Act
            ViewResult result = controller.Index(winLossByProductQuery) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void WinLossByProduct_Always_CallsMediator()
        {
            // Arrange

            // Act
            controller.Index(winLossByProductQuery);

            // Assert
            mediator.Received().Send(Arg.Any<WinLossByProductQuery>());
        }

        [Fact]
        public void WinLossByProduct_CallsMediator_MustSendWinLossByProductQuery()
        {
            // Arrange

            // Act
            controller.Index(winLossByProductQuery);

            // Assert
            mediator.Received().Send(Arg.Is(winLossByProductQuery));
        }

        [Fact]
        public void WinLossByProductExcel_Always_CallsMediatorSendExportWinLossQuery()
        {
            // Arrange
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossByProductExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(winLossByProductExcelQuery);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(winLossByProductExcelQuery));
        }
    }
}