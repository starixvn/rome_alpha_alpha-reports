namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests.PlayForFun;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("UserCollection")]
    public class PlayForFunControllerTests
    {
        private readonly IMediator mediator;
        private readonly PlayForFunController controller;
        private readonly UserFixture userFixture;

        public PlayForFunControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            controller = new PlayForFunController(mediator);
            this.userFixture = userFixture;
        }

        [Fact]
        public void MasterOrAgent_Always_CallsMediatorSendMasterOrAgentPlayForFunQuery()
        {
            // Arrange
            var message = new MasterOrAgentPlayForFunQuery(userFixture.User);

            // Act
            controller.MasterOrAgent(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Member_Always_CallsMediatorSendMemberPlayForFunQuery()
        {
            // Arrange
            var message = new MemberPlayForFunQuery(userFixture.User);

            // Act
            controller.Member(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void MasterOrAgentExcel_Always_CallsMediatorSendMasterOrAgentPlayForFunExcelQuery()
        {
            // Arrange
            var message = new MasterOrAgentPlayForFunExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MasterOrAgentPlayForFunExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.MasterOrAgentExcel(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void MemberExcel_Always_CallsMediatorSendMemberExcelQuery()
        {
            // Arrange
            var message = new MemberPlayForFunExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MemberPlayForFunExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.MemberExcel(message);

            // Assert
            mediator.Received().Send(message);
        }
    }
}
