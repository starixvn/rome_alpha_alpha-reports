namespace Alpha.Reports.Excel.Tests.Params
{
    using System.Collections.Generic;
    using System.Linq;
    using Enumerations;
    using Excel.Params;
    using Models;
    using TestCases;
    using Xunit;

    public class ExportWinLossDetailParamTests
    {
        [Fact]
        public void UserLevel_Always_GetsUserLevelFromConstructor()
        {
            // Arrange
            var expectedUserLevel = Enumeration.FromValue<UserLevel>(4);
            var exportWinLossDetailParam = new ExportWinLossDetailParam(expectedUserLevel, string.Empty);

            // Act
            var actualUserLevel = exportWinLossDetailParam.UserLevel;

            // Assert
            Assert.Equal(expectedUserLevel, actualUserLevel);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void UserSelectedProductIds_NullOrWhiteSpaceProductIds_ReturnsEmptyList(string productIds)
        {
            // Arrange
            var exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), productIds);
            var expectedUserSelectedProductIds = Enumerable.Empty<int>();

            // Act
            var actualUserSelectedProductIds = exportWinLossDetailParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }

        [Theory]
        [MemberData("UserSelectedProductIdsCases", MemberType = typeof(ExportWinLossDetailParamTestCases))]
        public void UserSelectedProductIds_NotNullOrWhiteSpaceProductIds_ReturnsCorrectUserSelectedProductIds(
            string productIds,
            IEnumerable<int> expectedUserSelectedProductIds)
        {
            // Arrange
            var exportWinLossDetailParam = new ExportWinLossDetailParam(Enumeration.FromValue<UserLevel>(4), productIds);

            // Act
            var actualUserSelectedProductIds = exportWinLossDetailParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}
