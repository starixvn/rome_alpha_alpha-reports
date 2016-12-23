namespace Alpha.Reports.Site.Tests.Configurations
{
    using System;
    using System.Collections.Specialized;
    using Site.Configurations;
    using Xunit;

    public class SiteSettingsTests
    {
        [Fact]
        public void AppSettings_Always_ReturnsSetValue()
        {
            // Arrange
            var expectedAppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath",
                ["StoreProcedurePath"] = "StoreProcedurePath",
                ["AssetAppPath"] = "AssetAppPath",
                ["AccessDeniedPage"] = "AccessDeniedPage"
            };
            SiteSettings.AppSettings = expectedAppSettings;

            // Act
            var actualAppSettings = SiteSettings.AppSettings;

            // Assert
            Assert.Equal(expectedAppSettings, actualAppSettings);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Verify_NullOrWhiteSpaceOldMainAppPath_ThrowsInvalidOperationException(string oldMainAppPath)
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = oldMainAppPath
            };

            // Act & Assert
            Exception ex = Assert.Throws<InvalidOperationException>(() => SiteSettings.Verify());
            Assert.Equal("path is null or whitespace", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Verify_NullOrWhiteSpaceAccessDeniedPage_ThrowsInvalidOperationException(string storeProcedurePath)
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath",
                ["StoreProcedurePath"] = storeProcedurePath
            };

            // Act & Assert
            Exception ex = Assert.Throws<InvalidOperationException>(() => SiteSettings.Verify());
            Assert.Equal("path is null or whitespace", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Verify_NullOrWhiteSpaceAssetAppPath_ThrowsInvalidOperationException(string assetAppPath)
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath",
                ["StoreProcedurePath"] = "StoreProcedurePath",
                ["AssetAppPath"] = assetAppPath
            };

            // Act & Assert
            Exception ex = Assert.Throws<InvalidOperationException>(() => SiteSettings.Verify());
            Assert.Equal("path is null or whitespace", ex.Message);
        }

        [Fact]
        public void Verify_HasRequiredSettings_NotThrowsInvalidOperationException()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath",
                ["StoreProcedurePath"] = "StoreProcedurePath",
                ["AssetAppPath"] = "AssetAppPath"
            };

            // Act
            var actualHasException = VerifyHasException();

            // Assert
            Assert.False(actualHasException);
        }

        [Fact]
        public void Verify_NotHasRequiredSettings_ThrowsInvalidOperationException()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = string.Empty,
                ["StoreProcedurePath"] = string.Empty,
                ["AssetAppPath"] = string.Empty
            };

            // Act
            var actualHasException = VerifyHasException();

            // Assert
            Assert.True(actualHasException);
        }

        [Fact]
        public void AccessDeniedPage_Always_ReturnsSetValue()
        {
            // Arrange
            var expectedSettings = "AccessDeniedPage";
            var appSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath",
                ["StoreProcedurePath"] = "StoreProcedurePath",
                ["AssetAppPath"] = "AssetAppPath",
                ["AccessDeniedPage"] = expectedSettings
            };

            SiteSettings.AppSettings = appSettings;

            // Act
            var actualSettings = SiteSettings.AccessDeniedPage;

            // Assert
            Assert.Equal(expectedSettings, actualSettings);
        }

        private bool VerifyHasException()
        {
            try
            {
                SiteSettings.Verify();
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
