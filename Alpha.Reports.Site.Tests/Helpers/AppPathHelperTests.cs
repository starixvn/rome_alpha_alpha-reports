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
        public void GetAppPath_WithAppNameAndVirtualPathAndNotHasContext_ReturnsEmpty()
        {
            // Arrange
            var validAppName = "ValidAppName";
            var validVirtualPath = "ValidVirtualPath";
            HttpAdapter.HasContext = false;

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, validVirtualPath);

            // Assert
            Assert.Empty(actualAppPath);
        }

        [Theory]
        [InlineData("~/", "/ValidAppName/")]
        [InlineData("~/ValidVirtualPath", "/ValidAppName/ValidVirtualPath")]
        [InlineData("/", "/ValidAppName/")]
        public void GetAppPath_WithAppNameAndVirtualPathAndHasContext_ReturnsCorrectPath(
            string virtualPath,
            string expectedPath)
        {
            // Arrange
            var validAppName = "/ValidAppName";

            HttpAdapter.HasContext = true;
            HttpAdapter.AppDomainAppVirtualPath = "/site-reports";
            HttpAdapter.ApplyAppPathModifierFunc = (relativePath) =>
            {
                var result = string.IsNullOrEmpty(relativePath)
                ? HttpAdapter.AppDomainAppVirtualPath
                : HttpAdapter.AppDomainAppVirtualPath + relativePath.Replace("~", string.Empty);

                return result;
            };

            // Act
            var actualAppPath = AppPathHelper.GetAppPath(validAppName, virtualPath);

            // Assert
            Assert.Equal(expectedPath, actualAppPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetRelativePath_NullOrWhiteSpaceVirtualPath_ReturnsVirtualPath(string virtualPath)
        {
            // Arrange
            var expectedPath = virtualPath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, "AbsolutePath");

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetRelativePath_NullOrWhiteSpaceAbsolutePath_ReturnsVirtualPath(string absolutePath)
        {
            // Arrange
            var virtualPath = "VirtualPath";
            var expectedPath = virtualPath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData("VirtualPath/", "")]
        [InlineData("VirtualPathA", "")]
        [InlineData("VirtualPath/SubPath", "SubPath")]
        [InlineData("VirtualPath/VirtualPath/SubPath", "VirtualPath/SubPath")]
        public void GetRelativePath_AbsolutePathStartWithVirtualPath_ReturnsCorrectRelativePath(
            string absolutePath,
            string expectedPath)
        {
            // Arrange
            var virtualPath = "VirtualPath";

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData("VirtualPath2/")]
        [InlineData("VirtualPathA")]
        [InlineData("VirtualPath2/SubPath")]
        [InlineData("VirtualPath2/VirtualPath/SubPath")]
        public void GetRelativePath_AbsolutePathNotStartWithVirtualPath_ReturnsCorrectRelativePath(string absolutePath)
        {
            // Arrange
            var virtualPath = "VirtualPath1";
            var expectedPath = absolutePath;

            // Act
            var actualPath = AppPathHelper.GetRelativePath(virtualPath, absolutePath);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }
    }
}
