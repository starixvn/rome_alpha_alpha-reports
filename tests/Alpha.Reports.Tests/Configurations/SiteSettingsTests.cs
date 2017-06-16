using System.Collections.Specialized;
using System.Configuration;
using Alpha.Reports.Configurations;
using Xunit;

namespace Alpha.Reports.Tests.Configurations
{
    public class SiteSettingsTests
    {
        [Fact]
        public void Verify_MissingStoredProcedurePath_ThrowsConfigurationErrorsException()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["ReportsAppPath"] = "site-reports"
            };

            // Act & Assert
            Assert.Throws<ConfigurationErrorsException>(() => SiteSettings.Verify());
        }

        [Fact]
        public void Verify_MissingReportsAppPath_ThrowsConfigurationErrorsException()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["StoredProcedurePath"] = "Local"
            };

            // Act & Assert
            Assert.Throws<ConfigurationErrorsException>(() => SiteSettings.Verify());
        }

        [Fact]
        public void Verify_HasEnoughSettings_PassVerify()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["StoredProcedurePath"] = "Local",
                ["ReportsAppPath"] = "site-reports"
            };
            var passVerify = false;

            // Act
            SiteSettings.Verify();
            passVerify = true;

            // Assert
            Assert.True(passVerify);
        }

        [Fact]
        public void BetListAppPath_HasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["BetListsAppPath"] = "Path"
            };

            var expectedSetting = "Path";

            // Act
            var actualSetting = SiteSettings.BetListAppPath;

            // Assert
            Assert.Equal(expectedSetting, actualSetting);
        }

        [Fact]
        public void BetListAppPath_NotHasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["ReportsAppPath"] = "Path"
            };
            var expectedSetting = "site-betlists";

            // Act
            var actualSetting = SiteSettings.BetListAppPath;

            // Assert
            Assert.Equal(expectedSetting, actualSetting);
        }

        [Fact]
        public void EnabledStaticData_NotHasConfig_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["EnabledStaticData"] = "1"
            };
            var expectedSetting = true;

            // Act
            var actualSetting = SiteSettings.EnabledStaticData;

            // Assert
            Assert.Equal(expectedSetting, actualSetting);
        }
    }
}
