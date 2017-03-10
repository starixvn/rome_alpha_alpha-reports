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
        public void Master_Always_CallsMediatorSendMasterPlayForFunQuery()
        {
            // Arrange
            var message = new MasterPlayForFunQuery(userFixture.User);

            // Act
            controller.Master(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void Agent_Always_CallsMediatorSendAgentPlayForFunQuery()
        {
            // Arrange
            var message = new AgentPlayForFunQuery(userFixture.User);

            // Act
            controller.Agent(message);

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
        public void MasterExcel_Always_CallsMediatorSendMasterPlayForFunExcelQuery()
        {
            // Arrange
            var message = new MasterPlayForFunExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<MasterPlayForFunExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.MasterExcel(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void AgentExcel_Always_CallsMediatorSendAgentPlayForFunExcelQuery()
        {
            // Arrange
            var message = new AgentPlayForFunExcelQuery(userFixture.User);
            mediator.Send<ExcelFileViewModel>(Arg.Any<AgentPlayForFunExcelQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));

            // Act
            controller.AgentExcel(message);

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
