namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Reports.Models;
    using Site.Controllers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredResultControllerTests
    {
        private readonly IMediator mediator;
        private readonly EnteredResultController enteredResultController;
        private readonly IUser user;

        public EnteredResultControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            enteredResultController = new EnteredResultController(mediator);
            user = Substitute.For<IUser>();
        }

        [Fact]
        public void EnteredResult_Always_CallsMediatorSendEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredSportResultQuery(user);

            // Act
            enteredResultController.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is<EnteredSportResultQuery>(message));
        }

        [Fact]
        public void EnteredResultExcel_Always_CallsMediatorSendExportEnteredResultQuery()
        {
            // Arrange
            var message = new EnteredSportResultExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<EnteredSportResultExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            enteredResultController.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is<EnteredSportResultExcelQuery>(message));
        }
    }
}