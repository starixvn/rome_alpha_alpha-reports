namespace Alpha.Reports.Tests.Models.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.Fixtures;
    using System;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    [Collection("AnyValueCreatorCollection")]
    public class BetChoiceMatchingTicketModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public BetChoiceMatchingTicketModelTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Theory]
        [InlineData(3, 4, "a", 5.0, 6.0, true, 7, 8, "3_4_5_6_a_True_7_8")]
        [InlineData(3, 4, "h", 5.0, 6.0, false, 7, 8, "3_4_5_6_h_False_7_8")]
        public void EventKey_Always_ReturnsValidString(
            long matchId,
            int betType,
            string betTeam,
            decimal hdp1,
            decimal hdp2,
            bool isLive,
            int liveHomeScore,
            int liveAwayScore,
            string eventKey)
        {
            // Arrange
            var ticket = new BetChoiceMatchingTicketModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<int>(),
                matchId,
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                betType,
                betTeam,
                hdp1,
                hdp2,
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<long>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                isLive,
                liveHomeScore,
                liveAwayScore,
                anyValueCreatorFixture.Create<bool>()
                );

            // Act
            var actual = ticket.EventKey;

            // Assert
            Assert.Equal(eventKey, actual);
        }
    }
}
