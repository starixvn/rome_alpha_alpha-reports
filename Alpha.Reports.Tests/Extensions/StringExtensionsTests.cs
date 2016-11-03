namespace Alpha.Reports.Tests.Extensions
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Reports.Extensions;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ToListInt_StringIsNull_ReturnEmptyListInt()
        {
            // Arrange
            string s = null;

            // Act
            var expected = StringExtensions.ToListInt(s);

            // Assert
            Assert.IsEmpty(expected);
        }

        [TestCase("")]
        [TestCase("  ")]
        public void ToListInt_StringIsEmptyOrWhiteSpace_ReturnEmptyListInt(string s)
        {
            // Act
            var expected = s.ToListInt();

            // Assert
            Assert.IsEmpty(expected);
        }

        [Test, TestCaseSource("StringContainsNonNumericCharactersCases")]
        public void ToListInt_Always_IgnoreNonNumbericCharacters(string s, List<int> ids)
        {
            // Arrange

            // Act
            var expected = s.ToListInt();

            // Assert
            CollectionAssert.AreEquivalent(expected, ids);
        }

        private static object[] StringContainsNonNumericCharactersCases =
        {
            new object[] { "1, abc, 3, d, 4", new List<int> { 1, 3, 4}},
            new object[] { "1, 200, 343, 33, abc", new List<int> { 1, 200, 343, 33 }},
            new object[] { "1000000000000000000000, ab, 12", new List<int> { 12 }}
        };
    }
}
