namespace Alpha.Reports.Tests.Enumerations
{
    using System;
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

        [Fact]
        public void GreaterThan_NullLeftEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = null;
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left > right);
        }

        [Fact]
        public void GreaterThan_NullRightEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left > right);
        }

        [Fact]
        public void GreaterThan_LeftGreaterThanRight_ReturnsTrue()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(2);
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act
            var actualGreaterThan = left > right;

            // Assert
            Assert.True(actualGreaterThan);
        }

        [Fact]
        public void GreaterThan_LeftNotGreaterThanRight_ReturnsFalse()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(2);

            // Act
            var actualGreaterThan = left > right;

            // Assert
            Assert.False(actualGreaterThan);
        }

        [Fact]
        public void NotLessThan_NullLeftEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = null;
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left >= right);
        }

        [Fact]
        public void NotLessThan_NullRightEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left >= right);
        }

        [Fact]
        public void NotLessThan_LeftNotLessThanRight_ReturnsTrue()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act
            var actualGreaterThan = left >= right;

            // Assert
            Assert.True(actualGreaterThan);
        }

        [Fact]
        public void NotLessThan_LeftLessThanRight_ReturnsFalse()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(2);

            // Act
            var actualGreaterThan = left >= right;

            // Assert
            Assert.False(actualGreaterThan);
        }

        [Fact]
        public void LessThan_NullLeftEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = null;
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left < right);
        }

        [Fact]
        public void LessThan_NullRightEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left < right);
        }

        [Fact]
        public void LessThan_LeftLessThanRight_ReturnsTrue()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(2);

            // Act
            var actualGreaterThan = left < right;

            // Assert
            Assert.True(actualGreaterThan);
        }

        [Fact]
        public void LessThan_LeftNotLessThanRight_ReturnsFalse()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act
            var actualGreaterThan = left < right;

            // Assert
            Assert.False(actualGreaterThan);
        }

        [Fact]
        public void NotGreaterThan_NullLeftEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = null;
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left <= right);
        }

        [Fact]
        public void NotGreaterThan_NullRightEnumeration_ThrowsArgumentNullException()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left <= right);
        }

        [Fact]
        public void NotGreaterThan_LeftNotGreaterThanRight_ReturnsTrue()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(1);
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act
            var actualGreaterThan = left <= right;

            // Assert
            Assert.True(actualGreaterThan);
        }

        [Fact]
        public void NotGreaterThan_LeftGreaterThanRight_ReturnsFalse()
        {
            // Arrange
            UserLevel left = Enumeration.FromValue<UserLevel>(2);
            UserLevel right = Enumeration.FromValue<UserLevel>(1);

            // Act
            var actualGreaterThan = left <= right;

            // Assert
            Assert.False(actualGreaterThan);
        }
    }
}
