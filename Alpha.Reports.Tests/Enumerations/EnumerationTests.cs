namespace Alpha.Reports.Tests.Enumerations
{
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Models;
    using Reports.Models.Enumerations;
    using Xunit;

    public class EnumerationTests
    {
        [Theory]
        [InlineData(4, 4, 0)]
        [InlineData(4, 3, 1)]
        [InlineData(4, 2, 2)]
        [InlineData(4, 1, 3)]
        public void AbsoluteDifference_Always_ReturnCorrectDifference(
            byte firstValue,
            byte secondValue,
            byte expectedDifference)
        {
            // Arrange
            var firstEnum = Enumeration.FromValue<UserLevel>(firstValue);
            var secondEnum = Enumeration.FromValue<UserLevel>(secondValue);

            // Act
            var actualDifference = Enumeration.AbsoluteDifference(firstEnum, secondEnum);

            // Assert
            Assert.Equal(expectedDifference, actualDifference);
        }

        [Theory]
        [InlineData("Super", 4)]
        [InlineData("Master", 3)]
        [InlineData("Agent", 2)]
        [InlineData("Member", 1)]
        public void FromDisplayName_StateUnderTest_ExpectedBehaviour(string displayName, int expectedValue)
        {
            // Act
            var actualValue = Enumeration.FromDisplayName<UserLevel>(displayName).Value;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GetAll_GetAllUserLevels_ReturnsAllUserLevels()
        {
            // Arrange
            var expectedUserLevels = new List<UserLevel>()
            {
                Enumeration.FromValue<UserLevel>(4),
                Enumeration.FromValue<UserLevel>(3),
                Enumeration.FromValue<UserLevel>(2),
                Enumeration.FromValue<UserLevel>(2)
            };

            // Act
            var actualValues = Enumeration.GetAll(typeof(UserLevel)).Cast<UserLevel>();

            // Assert
            Assert.True(actualValues.Count() == expectedUserLevels.Count() &&
                expectedUserLevels.Where(userLevel1 => actualValues.Where(userLevel2 => userLevel2.Value == userLevel1.Value).FirstOrDefault() == null)
                .FirstOrDefault() == null);
        }
    }
}
