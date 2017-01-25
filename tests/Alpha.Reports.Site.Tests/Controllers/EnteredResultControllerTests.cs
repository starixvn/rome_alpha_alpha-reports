namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class EnteredResultControllerTests
    {
        private readonly IMediator mediator;
        private readonly EnteredResultController enteredResultController;
        private readonly UserFixture userFixture;

        public EnteredResultControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            enteredResultController = new EnteredResultController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void EnteredResult_Always_CallsMediatorSendEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredSportResultQuery(userFixture.User);

            // Act
            enteredResultController.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void EnteredResultExcel_Always_CallsMediatorSendExportEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredSportResultExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<EnteredSportResultExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            enteredResultController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<EnteredSportResultExcelQuery>(message));
        }
    }
}