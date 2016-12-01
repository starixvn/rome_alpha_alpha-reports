namespace Alpha.Reports.Tests.Extensions
{
    using System.Collections.Generic;
    using Reports.Extensions;
    using TestCases;
    using Xunit;

    public class StringExtensionsTests
    {
        [Fact]
        public void ToListInt_StringIsNull_ReturnEmptyListInt()
        {
            // Arrange
            string s = null;

            // Act
            var expected = StringExtensions.ToListInt(s);

            // Assert
            Assert.Empty(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void ToListInt_StringIsEmptyOrWhiteSpace_ReturnEmptyListInt(string s)
        {
            // Act
            var expected = s.ToListInt();

            // Assert
            Assert.Empty(expected);
        }

        [Theory]
        [MemberData("StringContainsNonNumericCharactersCases", MemberType = typeof(StringExtensionsTestCases))]
        public void ToListInt_Always_IgnoreNonNumbericCharacters(string s, List<int> ids)
        {
            // Arrange

            // Act
            var expected = s.ToListInt();

            // Assert
            Assert.Equal(expected, ids);
        }
    }
}
