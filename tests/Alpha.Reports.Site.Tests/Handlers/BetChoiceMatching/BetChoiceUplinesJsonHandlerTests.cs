namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Params.BetChoiceMatching;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Fixtures;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceUplinesJsonHandlerTests
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly BetChoiceUplinesJsonHandler handler;
        private readonly IUser user;

        public BetChoiceUplinesJsonHandlerTests(UserFixture userFixture)
        {
            user = userFixture.User;
            betChoiceMatchingService = Substitute.For<BetChoiceMatchingService>();
            handler = new BetChoiceUplinesJsonHandler(betChoiceMatchingService);
        }

        [Fact]
        public void Handle_ReturnsCorrectJsonModel()
        {
            // Arrange
            var message = new BetChoiceUplinesJsonQuery(user)
            {
                SearchAccountId = 1
            };
            var uplinesModel = new BetChoiceMatchingUplinesModel(1, "Agent 1", 2, "Master 2");
            betChoiceMatchingService.QueryUplines(Arg.Any<BetChoiceUplinesParam>()).Returns(uplinesModel);

            // Act
            var actual = handler.Handle(message);

            // Assert
            Assert.Equal(uplinesModel, actual);
        }
    }
}
