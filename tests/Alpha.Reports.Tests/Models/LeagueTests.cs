namespace Alpha.Reports.Tests.Models
{
    using Fixtures;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class LeagueTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public LeagueTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void LeagueId_Always_ReturnsIdSetFromConstructor()
        {
            // Arrange
            var leagueId = anyValueCreatorFixture.Create<int>();
            var league = new League(
                leagueId, 
                anyValueCreatorFixture.Create<int>(), 
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>());
            var expectedId = leagueId;

            // Act
            var actualId = league.LeagueId;

            // Assert
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void Name_Always_ReturnsNameSetFromConstructor()
        {
            // Arrange
            var leagueName = anyValueCreatorFixture.Create<string>();
            var league = new League(
                anyValueCreatorFixture.Create<int>(), 
                anyValueCreatorFixture.Create<int>(), 
                leagueName,
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>());
            var expectedName = leagueName;

            // Act
            var actualName = league.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public void SportTypeId_Always_ReturnsSportTypeIdSetFromConstructor()
        {
            // Arrange
            var sportTypeId = anyValueCreatorFixture.Create<int>();
            var league = new League(
                anyValueCreatorFixture.Create<int>(), 
                sportTypeId, 
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>());
            var expectedSportTypeId = sportTypeId;

            // Act
            var actualSportTypeId = league.SportTypeId;

            // Assert
            Assert.Equal(expectedSportTypeId, actualSportTypeId);
        }

        [Fact]
        public void DisplayName_Always_ReturnsDisplayNameSetFromConstructor()
        {
            // Arrange
            var displayName = anyValueCreatorFixture.Create<string>();
            var league = new League(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                displayName,
                anyValueCreatorFixture.Create<byte>(),
                anyValueCreatorFixture.Create<int>());
            var expectedDisplayName = displayName;

            // Act
            var actualDisplayName = league.DisplayName;

            // Assert
            Assert.Equal(expectedDisplayName, actualDisplayName);
        }
    }
}
