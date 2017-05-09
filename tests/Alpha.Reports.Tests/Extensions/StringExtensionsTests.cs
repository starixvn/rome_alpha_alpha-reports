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

        [Fact]
        public void IsNullOrBlank_NullInput_ReturnTrue()
        {
            // Arrange
            string input = null;

            // Act
            var expected = StringExtensions.IsNullOrBlank(input);

            // Assert
            Assert.Equal(expected, true);
        }

        [Fact]
        public void IsNullOrBlank_EmtpyInput_ReturnTrue()
        {
            // Arrange
            string input = string.Empty;

            // Act
            var expected = StringExtensions.IsNullOrBlank(input);

            // Assert
            Assert.Equal(expected, true);
        }

        [Fact]
        public void IsNullOrBlank_WhiteSpaceInput_ReturnTrue()
        {
            // Arrange
            string input = "   ";

            // Act
            var expected = StringExtensions.IsNullOrBlank(input);

            // Assert
            Assert.Equal(expected, true);
        }

        [Fact]
        public void ToShortMonthNameAndShortYear_ValueIsNull_ReturnEmpty()
        {
            // Arrange
            string value = null;

            // Act
            var expected = value.ToShortMonthNameAndShortYear();

            // Assert
            Assert.Empty(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public void ToShortMonthNameAndShortYear_ValueIsEmptyOrWhiteSpace_ReturnEmpty(string value)
        {
            // Act
            var expected = value.ToShortMonthNameAndShortYear();

            // Assert
            Assert.Empty(expected);
        }

        [Fact]
        public void ToShortMonthNameAndShortYear_ValueIsNotEmptyOrWhiteSpace_ReturnShortMonthNameAndShortYear()
        {
            // Arrange
            var value = "2017-03";
            var expectedValue = "Mar-17";

            // Act
            var actualValue = value.ToShortMonthNameAndShortYear();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}