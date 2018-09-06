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
        private readonly ProductService productService;
        private readonly ReportLegendHandler reportLegendHandler;
        private readonly UserFixture userFixture;

        public ReportLegendHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(new List<byte> { 1, 2, 3 });
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            this.userFixture = userFixture;
            reportLegendHandler = new ReportLegendHandler(productService, userFixture.User);
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

        [Fact]
        public void HandleReportLegendQuery_HavePokerGames_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(Enumerable.Empty<byte>());
            productService.IsShowProduct(10, 33).Returns(true);

            var expectedViewModel = new List<string>
            {
                "- Turnover of Poker Games is the buy-in/top up amount on table."
            };

            // Act
            var actualViewModel = reportLegendHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_NotHavePokerGames_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(Enumerable.Empty<byte>());
            productService.IsShowProduct(10, 33).Returns(false);

            var expectedViewModel = new List<string>();

            // Act
            var actualViewModel = reportLegendHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}
