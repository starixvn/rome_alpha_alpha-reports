namespace Alpha.Reports.Site.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using NSubstitute;
    using Services;
    using Site.Handlers;
    using Site.Requests;
    using Xunit;

    [Trait("Category", "ReportLegend")]
    public class ReportLegendHandlerTests
    {
        private readonly ProductService productService;
        private readonly ReportLegendHandler reportLegendHandler;

        public ReportLegendHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(new List<byte> { 1, 2, 3 });
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            reportLegendHandler = new ReportLegendHandler(productService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleReportLegendQuery_HaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(new List<byte> { 1 });
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
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(Enumerable.Empty<byte>());
            var expectedViewModel = Enumerable.Empty<string>();

            // Act
            var actualViewModel = reportLegendHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}
