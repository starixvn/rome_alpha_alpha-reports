namespace Alpha.Reports.Site.Tests.ViewModels.Outstanding
{
    using System.Collections.Generic;
    using Ploeh.AutoFixture;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Site.ViewModels;
    using Site.ViewModels.Outstanding;
    using Xunit;
    using static Reports.Models.UserLevel;

    [Trait("Category", "Outstanding")]
    public class OutstandingViewModelTests
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public void DisplayOutstandingLevels_SuperViewMasterOutstanding_Show_Member_Super_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Super };
            var viewModel = new OutstandingViewModel(
                Super,
                Master,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_SuperViewAgentOutstanding_Show_Member_Master_Super_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Master, Super };
            var viewModel = new OutstandingViewModel(
                Super,
                Agent,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_SuperViewMemberOutstanding_Show_Member_Agent_Master_Super_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Agent, Master, Super };
            var viewModel = new OutstandingViewModel(
                Super,
                Member,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_MasterViewAgentOutstanding_Show_Member_Master_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Master };
            var viewModel = new OutstandingViewModel(
                Master,
                Agent,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_MasterViewMemberOutstanding_Show_Member_Agent_Master_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Agent, Master };
            var viewModel = new OutstandingViewModel(
                Master,
                Member,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_AgentViewMemberOutstanding_Show_Member_Agent_Outstanding()
        {
            // Arrange
            var expected = new List<UserLevel> { Member, Agent };
            var viewModel = new OutstandingViewModel(
                Agent,
                Member,
                fixture.Create<IEnumerable<OutstandingData>>(),
                fixture.Create<string>(),
                fixture.Create<IEnumerable<ActionLinkViewModel>>());

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}