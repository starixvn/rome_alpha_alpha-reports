namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLossDetail
{
    using System.Collections.Generic;
    using Fixtures;
    using Reports.Constants;
    using Reports.Models.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    public class MatchWinLossDetailViewModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossDetailViewModelTests()
        {
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void RenderSubTotal_HaveMatchId_ReturnsTrue()
        {
            // Arrange
            var matchId = 1;
            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                matchId,
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                anyValueCreatorFixture.Create<MatchWinLossDetailModel>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<string>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.True(actualRenderSubTotal);
        }

        [Fact]
        public void RenderSubTotal_HaveRaceId_ReturnsTrue()
        {
            // Arrange
            var raceId = 1;
            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<int>(),
                raceId,
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                anyValueCreatorFixture.Create<MatchWinLossDetailModel>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<string>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.True(actualRenderSubTotal);
        }

        [Fact]
        public void RenderSubTotal_NotHaveMatchIdAndNotHaveRaceId_ReturnsFalse()
        {
            // Arrange
            var raceId = 0;
            var matchId = 0;

            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                matchId,
                raceId,
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                anyValueCreatorFixture.Create<MatchWinLossDetailModel>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<string>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.False(actualRenderSubTotal);
        }

        [Fact]
        public void MatchName_HorseRacingAndValidShowTime_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 1231, showTime: "01/01/2017");
            var expectedTitle = "01/01/2017 12:00:00 AM";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_HorseRacingAndInvalidShowTime_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 1231, showTime: "Show Time 1");
            var expectedTitle = string.Empty;

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData(1600)]
        [InlineData(1601)]
        [InlineData(1602)]
        public void GetGroupId_Lucky3BetType_ReturnsCorrectTitle(int bettype)
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: bettype, showTime: "Show Time 1");
            var expectedTitle = "Lucky 3 Game No 1";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_OtherBetTypeAndNeutral_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 1239, showTime: "Show Time 1", liveIndicator: false, isNeutral: true);
            var expectedTitle = "Team 1 (N) - vs - Team 1";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_OtherBetTypeAndNotNeutral_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 1239, showTime: "Show Time 1");
            var expectedTitle = "Team 1 - vs - Team 1";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_OutrightBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 10, showTime: "Show Time 1");
            var expectedTitle = string.Empty;

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_RacingBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 31, showTime: "Show Time 1");
            var expectedTitle = "Race 1";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_NumberGameBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType: 91, showTime: "Show Time 1");
            var expectedTitle = "Number Game No 1";

            // Act
            var actualTitle = matchWinLossDetailViewModel.MatchName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetLeagueTitle_RacingBetType_ReturnsCorrectLeagueTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(31, "Show Time 1", false, false, "League Name");
            var expectedTitle = "Racing - League Name";

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData("", "Number Game")]
        [InlineData("League Name", "Number Game - League Name")]
        public void GetLeagueTitle_NumberGameBetType_ReturnsCorrectLeagueTitle(string leagueName, string expectedTitle)
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(91, "Show Time 1", false, false, leagueName);

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData("01/01/2017", "League Name")]
        [InlineData("Invalid Show Time", "League Name")]
        public void GetLeagueTitle_VirtualHorseRacingBetType_ReturnsCorrectLeagueTitle(string showTime, string expectedTitle)
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(1231, showTime, false, false, "League Name");

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetLeagueTitle_OtherBetTypes_ReturnsCorrectLeagueTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(1239, "Show Time 1", false, false, "League Name");
            var expectedTitle = "League Name";

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData(1600)]
        [InlineData(1601)]
        [InlineData(1602)]
        public void GetLeagueTitle_Lucky3BetTypes_ReturnsCorrectLeagueTitle(int betType)
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(betType, "Show Time 1", false, false, "League Name");
            var expectedTitle = "Lucky3 - League Name";

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]        
        public void GetLeagueTitle_BullfightingBetTypes_ReturnsCorrectLeagueTitle()
        {
            // Arrange
            var matchWinLossDetailViewModel = CreateMatchWinLossDetailViewModel(1004, "Show Time 1", false, false, "League Name");
            var expectedTitle = "1 - League Name";

            // Act
            var actualTitle = matchWinLossDetailViewModel.LeagueName;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        private MatchWinLossDetailViewModel CreateMatchWinLossDetailViewModel(
            int betType,
            string showTime,
            bool liveIndicator = false,
            bool isNeutral = false,
            string leagueName = "")
           => new MatchWinLossDetailViewModel(
               1,
               leagueName,
               1,
               1,
               null,
               null,
               new MatchWinLossDetailModel(false, 1, 1, 1, betType, liveIndicator, 1, (byte)LeagueType.Self, 1, "1", isNeutral, showTime, 1, 1, null, null, null, null),
               "Team 1",
               "Team 1");
    }
}
