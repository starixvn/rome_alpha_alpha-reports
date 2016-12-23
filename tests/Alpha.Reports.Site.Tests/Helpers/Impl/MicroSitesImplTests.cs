namespace Alpha.Reports.Site.Tests.Helpers.Impl
{
    using System.Collections.Specialized;
    using Site.Configurations;
    using Site.Helpers;
    using Site.Helpers.Impl;
    using Xunit;

    public class MicroSitesImplTests
    {
        private readonly MicroSitesImpl microSitesImpl;

        public MicroSitesImplTests()
        {
            SiteSettings.AppSettings = new NameValueCollection
            {
                ["OldMainAppPath"] = "OldMainAppPath"
            };

            HttpAdapter.HasContext = true;
            HttpAdapter.AppDomainAppVirtualPath = "/site-reports";
            HttpAdapter.ApplyAppPathModifierFunc = (relativePath) =>
            {
                var result = string.IsNullOrEmpty(relativePath)
                ? HttpAdapter.AppDomainAppVirtualPath
                : HttpAdapter.AppDomainAppVirtualPath + relativePath.Replace("~", string.Empty);

                return result;
            };

            microSitesImpl = new MicroSitesImpl();
        }

        [Theory]
        [InlineData("~/", "OldMainAppPath/")]
        public void OldMainPath_Always(string virtualPath, string expectedOldMainPath)
        {
            // Act
            var actualOldMainPath = microSitesImpl.OldMainPath(virtualPath);

            // Assert
            Assert.Equal(expectedOldMainPath, actualOldMainPath);
        }
    }
}
