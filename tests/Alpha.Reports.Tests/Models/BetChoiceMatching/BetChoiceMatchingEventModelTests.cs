namespace Alpha.Reports.Tests.Models.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingEventModelTests
    {
        [Fact]
        public void Constructor_Always_InitCorrectObject()
        {
            // Arrange
            var tickets = BetChoiceMatchingTestCases.Tickets;
            var user1 = new BetChoiceMatchingUserModel(new List<BetChoiceMatchingTicketModel>
            {
                tickets[0],
                tickets[1]
            });
            var user2 = new BetChoiceMatchingUserModel(new List<BetChoiceMatchingTicketModel>
            {
                tickets[2]
            });

            // Act
            var eventModel = new BetChoiceMatchingEventModel(new List<BetChoiceMatchingUserModel> { user1, user2});

            // Assert
            Assert.Equal(8, eventModel.LeagueId);
            Assert.Equal(1, eventModel.SportType);
            Assert.Equal(7, eventModel.BetType);
            Assert.Equal("a", eventModel.BetTeam);
            Assert.Equal(DateTime.Parse("2020-02-02T16:05:00"), eventModel.GlobalShowTime);
            Assert.Equal(0.1m, eventModel.Hdp1);
            Assert.Equal(0.2m, eventModel.Hdp2);
            Assert.Equal(1, eventModel.HomeId);
            Assert.Equal(2, eventModel.AwayId);
            Assert.Equal(1, eventModel.LiveHomeScore);
            Assert.Equal(1, eventModel.LiveAwayScore);
            Assert.False(eventModel.IsLive);
        }
    }
}
