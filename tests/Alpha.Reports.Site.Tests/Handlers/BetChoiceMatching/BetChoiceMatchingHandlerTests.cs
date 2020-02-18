namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Alpha.Reports.Site.Requests.BetChoiceMatching;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using System;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("UserCollection")]
    public class BetChoiceMatchingHandlerTests
    {
        private readonly BetChoiceMatchingHandler handler;
        private readonly IUser user;

        public BetChoiceMatchingHandlerTests(UserFixture userFixture)
        {
            handler = new BetChoiceMatchingHandler();
            user = userFixture.User;
        }

        [Fact]
        public void Handle_ReturnsCorrectViewModel()
        {
            // Arrange
            SystemTime.Today = DateTime.Parse("2019-09-09");
            var message = new BetChoiceMatchingQuery(user);

            // Act
            var actual = handler.Handle(message);

            // Assert
            Assert.Equal(DateTime.Parse("2019-09-09"), actual.WinlostDate);
            Assert.Equal(2, actual.BetCount);
            Assert.Equal("2,3,4,5,8,10,15,20,30,50,100", string.Join(",", actual.BetCounts));
        }
    }
}
