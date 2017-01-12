namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
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
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_Always_CallsMediatorSendExportWinLossDetailQuery()
        {
            // Arrange
            var message = new WinLossDetailExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<WinLossDetailExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            winLossDetailController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}