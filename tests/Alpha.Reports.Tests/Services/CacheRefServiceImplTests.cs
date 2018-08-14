namespace Alpha.Reports.Tests.Services
{
    using Fanex.RemotingRef;
    using NSubstitute;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class CacheRefServiceImplTests
    {
        private const string DefaultLanguage = "en-US";
        private readonly IRemotingRefService remotingRefService;
        private readonly CacheRefService cacheRefService;

        public CacheRefServiceImplTests()
        {
            remotingRefService = Substitute.For<IRemotingRefService>();
            cacheRefService = new CacheRefServiceImpl(remotingRefService);
        }

        [Fact]
        public void GetSportName_ValidSportTypeId_ReturnsCorrectSportName()
        {
            // Arrange
            var expectedResult = "Virtual Sports";

            remotingRefService
                .GetSportName("5", DefaultLanguage)
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetSportName(5);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetBetType_ValidBetTypeId_ReturnsCorrectBetTypeName()
        {
            // Arrange
            var expectedResult = "VIP Baccarat";

            remotingRefService
                .GetBetTypeName("2402", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetBetType(2402);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetBetTypeName_ValidBetTypeId_ReturnsCorrectBetTypeName()
        {
            // Arrange
            var expectedResult = "VIP Baccarat";

            remotingRefService
                .GetBetTypeName("2402", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetBetTypeName(2402);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetLeagueName_ValidLeagueId_ReturnsCorrectLeagueName()
        {
            // Arrange
            var expectedResult = "Virtual Tennis";

            remotingRefService
                .GetLeagueName("27129", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetLeagueName(27129);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetLeagueName3rd_ValidLeague3rdId_ReturnsCorrectLeagueName3rd()
        {
            // Arrange
            var expectedResult = "YeeNai Phatthalung";

            remotingRefService
                .GetLeagueName3rd("4", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetLeagueName3rd(4);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetTeamName_ValidTeamId_ReturnsCorrectTeamName()
        {
            // Arrange
            var expectedResult = "Manchester United";

            remotingRefService
                .GetTeamName("101", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetTeamName(101);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetTeamName3rd_ValidTeam3rdId_ReturnsCorrectTeamName3rd()
        {
            // Arrange
            var expectedResult = "YeeNai Phatthalung";

            remotingRefService
                .GetTeamName3rd("2", "en-US")
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetTeamName3rd(2);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetProductName_ValidProductId_ReturnsCorrectProductName()
        {
            // Arrange
            var expectedResult = "Bullfighting";

            remotingRefService
                .GetResourceDataByRefIdAndResourceId("Product", "30", DefaultLanguage, string.Empty)
                .Returns(expectedResult);

            // Act
            var actualResult = cacheRefService.GetProductName(30);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
