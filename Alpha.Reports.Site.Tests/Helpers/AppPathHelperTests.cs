namespace Alpha.Reports.Site.Tests.Helpers
{
    using System;
    using System.Web;
    using NSubstitute;
    using Site.Helpers;
    using Xunit;

    public class AppPathHelperTests
    {
        [Fact]
        public void GetAppPath_WithAppNameAndVirtualPath_ReturnsEmpty()
        {
            // Arrange
            var validAppName = "ValidAppName";
            var validVirtualPath = "ValidVirtualPath";

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, validVirtualPath);

            // Assert
            Assert.Empty(actualAppPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetAppPath_WithEmptyAppName_ThrowsArgumentException(string appName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AppPathHelper.GetAppPath(appName, string.Empty));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetAppPath_WithEmptyVirtualPath_ThrowsArgumentException(string virtualPath)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => AppPathHelper.GetAppPath(string.Empty, virtualPath));
        }

        [Fact]
        public void GetAppPath_WithAppNameAndVirtualPathAndHasContext_ReturnsEmpty()
        {
            // Arrange
            var validAppName = "ValidAppName";
            var validVirtualPath = "ValidVirtualPath";

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, validVirtualPath);

            // Assert
            Assert.Empty(actualAppPath);
        }
    }
}
