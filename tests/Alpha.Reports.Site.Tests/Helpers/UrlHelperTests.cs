namespace Alpha.Reports.Site.Tests.Helpers
{
    using System.Collections.Generic;
    using Site.Helpers;
    using Xunit;

    [Trait("Category", "Site.Helpers")]
    public class UrlHelperTests
    {
        [Fact]
        public void AddQueryStringParams_EmptyUrl_ReturnQuestionMarkAddedQueryString()
        {
            // Arrange
            var url = string.Empty;
            var routeValue = new Dictionary<string, object>
            {
                { "paramName", "paramValue" },
                { "nullValue", null },
                { "emptyValue", string.Empty }
            };

            // Act
            var actualUrl = UrlHelper.AddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("?paramname=paramvalue&nullvalue=&emptyvalue=", actualUrl);
        }

        [Fact]
        public void AddQueryStringParams_EmptyRouteValues_ReturnInputUrl()
        {
            // Arrange
            var url = "url";
            var routeValue = new Dictionary<string, object>();

            // Act
            var actualUrl = UrlHelper.AddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("url", actualUrl);
        }

        [Fact]
        public void ReplaceOrAddQueryStringParams_EmptyRouteValues_ReturnInputUrl()
        {
            // Arrange
            var url = "URL";
            var routeValue = new Dictionary<string, object>();

            // Act
            var actualUrl = UrlHelper.ReplaceOrAddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("url", actualUrl);
        }

        [Fact]
        public void ReplaceOrAddQueryStringParams_EmptyUrl_ReturnQuestionMarkAddedQueryString()
        {
            // Arrange
            var url = string.Empty;
            var routeValue = new Dictionary<string, object>
            {
                { "paramName", "paramValue" },
                { "nullValue", null },
                { "emptyValue", string.Empty }
            };

            // Act
            var actualUrl = UrlHelper.ReplaceOrAddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("?paramname=paramvalue&nullvalue=&emptyvalue=", actualUrl);
        }

        [Fact]
        public void ReplaceOrAddQueryStringParams_UrlHasParam_ReturnReplacedParamValueUrl()
        {
            // Arrange
            var url = "URL?paramName=paramValue";
            var routeValues = new Dictionary<string, object>
            {
                { "paramName", "paramValue2" }
            };

            // Act
            var actualUrl = UrlHelper.ReplaceOrAddQueryStringParams(url, routeValues);

            // Assert
            Assert.Equal("url?paramname=paramvalue2", actualUrl);
        }

        [Fact]
        public void ReplaceOrAddQueryStringParams_UrlDoesNotHaveParam_ReturnAddedParamUrl()
        {
            // Arrange
            var url = "URL?paramName123=paramValue";
            var routeValues = new Dictionary<string, object>
            {
                { "paramNameAdding", "paramValue2" }
            };

            // Act
            var actualUrl = UrlHelper.ReplaceOrAddQueryStringParams(url, routeValues);

            // Assert
            Assert.Equal("url?paramname123=paramvalue&paramnameadding=paramvalue2", actualUrl);
        }

        [Fact]
        public void ReplaceOrAddQueryStringParams_NullRouteValue_ReturnUrlWithoutNullRouteValue()
        {
            // Arrange
            var url = "url?paramName123=paramValue";
            var routeValues = new Dictionary<string, object>
            {
                { "paramNameAdding", null },
                { string.Empty, "paramValue" }
            };

            // Act
            var actualUrl = UrlHelper.ReplaceOrAddQueryStringParams(url, routeValues);

            // Assert
            Assert.Equal("url?paramname123=paramvalue", actualUrl);
        }
    }
}