namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("UserCollection")]
    public class MoveStatementHistoryControllerTests
    {
        private readonly IMediator mediator;
        private readonly MoveStatementHistoryController controller;
        private readonly IUser user;

        public MoveStatementHistoryControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new MoveStatementHistoryController(mediator);
            user = userFixture.User;
        }

        [Fact]
        public void Index_ValidQuery_CallsMediatorSendMoveStatementHistoryQuery()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            // Act
            controller.Index(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void Excel_ValidQuery_CallsMediatorSendMoveStatementHistoryExcelQuery()
        {
            // Arrange
            var message = new MoveStatementHistoryExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MoveStatementHistoryExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.Excel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
