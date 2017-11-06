namespace Alpha.Reports.Site.Tests.Controllers
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Site.ViewModels;
    using Alpha.Reports.Utils;
    using MediatR;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class StatementControllerTests
    {
        private readonly IMediator mediator;
        private readonly StatementController controller;
        private readonly IUser user;

        public StatementControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new StatementController(mediator);

            this.user = userFixture.User;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void SMAStatement_ValidQuery_CallsMediatorSendSMAStatementQuery()
        {
            // Arrange
            var message = new SMAStatementQuery(user);

            // Act
            controller.SMAStatement(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void StatementDetails_ValidQuery_CallsMediatorSendStatementDetailsQuery()
        {
            // Arrange
            var message = new StatementDetailsQuery(user)
            {
                TransferDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            controller.StatementDetails(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void MemberStatement_ValidQuery_CallsMediatorSendMemberStatementQuery()
        {
            // Arrange
            var message = new MemberStatementQuery(user);

            // Act
            controller.MemberStatement(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }

        [Fact]
        public void SMAStatementExcel_ValidQuery_CallsMediatorSendSMAStatementExcelQuery()
        {
            // Arrange
            var message = new SMAStatementExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<SMAStatementExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.SMAStatementExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void StatementDetailExcel_ValidQuery_CallsMediatorSendStatementDetailExcelQuery()
        {
            // Arrange
            var message = new StatementDetailExcelQuery(user)
            {
                TransferDate = DefaultVariablesTest.CurrentDate
            };

            mediator.Send<ExcelFileViewModel>(Arg.Any<StatementDetailExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.StatementDetailExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }

        [Fact]
        public void MemberStatementExcel_ValidQuery_CallsMediatorSendMemberStatementExcelQuery()
        {
            // Arrange
            var message = new MemberStatementExcelQuery(user);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MemberStatementExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.MemberStatementExcel(message);

            // Assert
            mediator.Received().Send<ExcelFileViewModel>(Arg.Is(message));
        }
    }
}
