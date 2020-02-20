namespace Alpha.Reports.Tests.Models.BetChoiceMatching
{
    using Alpha.Reports.Models.BetChoiceMatching;
    using Alpha.Reports.Tests.TestCases.BetChoiceMatching;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingUserModelTests
    {
        [Fact]
        public void Constructor_Always_SumTotal()
        {
            // Arrange
            var tickets = BetChoiceMatchingTestCases.Tickets;

            // Act
            var user = new BetChoiceMatchingUserModel(new List<BetChoiceMatchingTicketModel>
            {
                tickets[0],
                tickets[3]
            });

            // Assert
            Assert.False(user.IsRunning);
            Assert.Equal(1, user.CustId);
            Assert.Equal("NK1", user.Username);
            Assert.Equal(25, user.Stake);
            Assert.Equal(1, user.MemberWinLoss);
            Assert.Equal(1, user.SuperWinLoss);
            Assert.Equal(0, user.Company);
        }
    }
}
