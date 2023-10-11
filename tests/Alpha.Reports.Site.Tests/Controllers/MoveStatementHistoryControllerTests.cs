namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Reports.Utils;
    using MediatR;
    using NSubstitute;
    using System;
    using System.Threading.Tasks;
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
            SystemTime.Today = DateTime.Today;
            mediator = Substitute.For<IMediator>();
            controller = new MoveStatementHistoryController(mediator);
            user = userFixture.User;
        }

        [Fact]
        public async Task Index_ValidQuery_CallsMediatorSendMoveStatementHistoryQuery()
        {
            // Arrange
            var message = new MoveStatementHistoryQuery(user);

            // Act
            await controller.Index(message);

            // Assert
            await mediator.Received().SendAsync(Arg.Is(message));
        }

        [Fact]
        public async Task Excel_ValidQuery_CallsMediatorSendMoveStatementHistoryExcelQuery()
        {
            // Arrange
            var message = new MoveStatementHistoryExcelQuery(user);
            mediator.SendAsync<ExcelFileViewModel>(Arg.Any<MoveStatementHistoryExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            await controller.Excel(message);

            // Assert
            await mediator.Received().SendAsync<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
