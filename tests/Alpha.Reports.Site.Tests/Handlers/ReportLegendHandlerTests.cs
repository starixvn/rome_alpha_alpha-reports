namespace Alpha.Reports.Site.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Services;
    using Site.Handlers;
    using Site.Requests;
    using Xunit;

    [Trait("Category", "ReportLegend")]
    [Collection("UserCollection")]
    public class ReportLegendHandlerTests
    {
        private readonly CacheRefService cacheRefService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private readonly ReportLegendHandler reportLegendHandler;
        private readonly UserFixture userFixture;

        public ReportLegendHandlerTests(UserFixture userFixture)
        {
            cacheRefService = Substitute.For<CacheRefService>();
            productDetailService = Substitute.For<ProductDetailService>();
            productService = Substitute.For<ProductService>();
            productService.GetOfferedProductsNeedDelayTimeToSumarizeData(Arg.Any<string>(), Arg.Any<int>()).Returns(new List<int> { 1, 2, 3 });
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            this.userFixture = userFixture;
            reportLegendHandler = new ReportLegendHandler(productService, productDetailService, cacheRefService, userFixture.User);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleReportLegendQuery_HaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.GetOfferedProductsNeedDelayTimeToSumarizeData(Arg.Any<string>(), Arg.Any<int>()).Returns(new List<int> { 1 });
            var expectedViewModel = new List<string> { "- Data of Product Name will be delayed up to 5 minutes after settling tickets." };

            // Act
            var actualViewModel = reportLegendHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_NotHaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.GetOfferedProductsNeedDelayTimeToSumarizeData(Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<int>());
            var expectedViewModel = Enumerable.Empty<string>();

            // Act
            var actualViewModel = reportLegendHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}
