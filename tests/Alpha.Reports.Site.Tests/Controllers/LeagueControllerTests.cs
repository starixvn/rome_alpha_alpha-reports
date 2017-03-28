namespace Alpha.Reports.Site.Tests.Controllers
{
    using Fixtures;
    using MediatR;
    using NSubstitute;
    using Site.Controllers;
    using Site.Requests;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("UserCollection")]
    public class LeagueControllerTests
    {
        private readonly IMediator mediator;
        private readonly LeagueController leagueController;
        private readonly UserFixture userFixture;

        public LeagueControllerTests(UserFixture userFixture)
        {
            mediator = Substitute.For<IMediator>();
            leagueController = new LeagueController(mediator);

            this.userFixture = userFixture;
        }

        [Fact]
        public void LeaguesByCustomer_Always_CallsMediatorSendLeaguesByCustomerQuery()
        {
            // Arrange
            var message = new LeaguesByCustomerQuery(userFixture.User);

            // Act
            leagueController.LeaguesByCustomer(message);

            // Assert
            mediator.Received().Send(Arg.Is(message));
        }
    }
}
