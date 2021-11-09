namespace Alpha.Reports.Tests.Configurations
{
    using System.Collections.Specialized;
    using System.Configuration;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Tests.Extensions;
    using Xunit;

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
        public void EnabledStaticData_HasConfig_ReturnsCorrectSetting()
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

        [Fact]
        public void RemoteRefURL_Always_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["RemoteRefURL"] = "RemoteRefURL"
            };
            var expectedSetting = "RemoteRefURL";

            // Act
            var actualSetting = SiteSettings.RemoteRefURL;

            // Assert
            Assert.Equal(expectedSetting, actualSetting);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("0", false)]
        [InlineData("1", true)]
        public void IsOfferJackpotProductDetails_Always_ReturnsCorrectSetting(string isOfferJackpotProductDetails, bool expected)
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["IsOfferJackpotProductDetails"] = isOfferJackpotProductDetails
            };

            // Act
            var actualSetting = SiteSettings.IsOfferJackpotProductDetails;

            // Assert
            Assert.Equal(expected, actualSetting);
        }

        [Fact]
        public void OfferSabaPromotionSites_Null_ReturnsNullCollection()
        {
            // Arrange & Act
            var actualSetting = SiteSettings.OfferSabaPromotionSites;

            // Assert
            Assert.Null(actualSetting);
        }

        [Fact]
        public void OfferSabaPromotionSites_Empty_ReturnsEmptyCollection()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["EnableProduct-37:Sites"] = string.Empty
            };

            // Act
            var actualSetting = SiteSettings.OfferSabaPromotionSites;

            // Assert
            Assert.Empty(actualSetting);
        }

        [Fact]
        public void OfferSabaPromotionSites_HasData_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["EnableProduct-37:Sites"] = "1,2,3,4,5,6"
            };
            var expectedSetting = new[] { "1", "2", "3", "4", "5", "6" };

            // Act
            var actualSetting = SiteSettings.OfferSabaPromotionSites;

            // Assert
            AssertExtension.Equal(actualSetting, expectedSetting);
        }

        [Fact]
        public void OfferSabaPromotionHeadlineSites_Null_ReturnsNullCollection()
        {
            // Arrange & Act
            var actualSetting = SiteSettings.OfferSabaPromotionHeadlineSites;

            // Assert
            Assert.Null(actualSetting);
        }

        [Fact]
        public void OfferSabaPromotionHeadlineSites_Empty_ReturnsEmptyCollection()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["EnableProduct-37:HLSites"] = string.Empty
            };

            // Act
            var actualSetting = SiteSettings.OfferSabaPromotionHeadlineSites;

            // Assert
            Assert.Empty(actualSetting);
        }

        [Fact]
        public void OfferSabaPromotionHeadlineSites_HasData_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["EnableProduct-37:HLSites"] = "1,2,3,4,5,6"
            };
            var expectedSetting = new[] { "1", "2", "3", "4", "5", "6" };

            // Act
            var actualSetting = SiteSettings.OfferSabaPromotionHeadlineSites;

            // Assert
            AssertExtension.Equal(actualSetting, expectedSetting);
        }

        [Fact]
        public void AllowedSitesToViewBonusDetail_HasData_ReturnsCorrectSetting()
        {
            // Arrange
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["AllowedSitesToViewBonusDetail"] = "a,b,c,d,e,f"
            };
            var expected = new[] { "A", "B", "C", "D", "E", "F" };

            // Act
            var actualSetting = SiteSettings.AllowedSitesToViewBonusDetail;

            // Assert
            AssertExtension.Equal(actualSetting, expected);
        }
    }
}
