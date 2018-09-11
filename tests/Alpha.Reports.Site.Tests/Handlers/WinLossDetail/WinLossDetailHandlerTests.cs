namespace Alpha.Reports.Site.Tests.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Services;
    using Site.Handlers.WinLossDetail;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.WinLossDetail;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly UserFixture userFixture;
        private readonly PagedReportData<WinLossDetailModel> reportData;

        public WinLossDetailHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string> { "Product Name" });

            reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel));
            var reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            this.userFixture = userFixture;
            winLossDetailHandler = new WinLossDetailHandler(productService, reportService, userFixture.User);
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailHandler.Handle(nullMessage));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("1")]
        [InlineData("1,2")]
        [InlineData("1,2,3")]
        public void HandlerWinLossDetailQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);
            message.CustLevelId = 4;
            message.UserSelectedProductIds = userSelectedProductIds;

            var products = Enumerable.Empty<Product>();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var winLossDetailParam = new WinLossDetailParam(
                10,
                0,
                0,
                DefaultVariablesTest.CurrentDate,
                DefaultVariablesTest.CurrentDate,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                message.CustLevel,
                "CustName",
                false,
                false);

            var expectedWinLossDetailViewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                new List<CustInfo> { new CustInfo(10, "Name", 4) },
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsNotContainAllBet_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainAllBet_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(21, string.Empty, null)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For AllBet, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_HaveProductsNeedDelayTimeToSumarizeData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(new List<byte> { 1 });
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(21, string.Empty, null)
            });

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For AllBet, Net Turnover is total valid bets that we use to compute commission.",
                "- Data of Product Name will be delayed up to 5 minutes after settling tickets."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_HavePokerGames_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery();
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer().Returns(new List<byte> { 1 });
            productService.IsShowProduct(10, 33).Returns(true);
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(21, string.Empty, null)
            });

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For AllBet, Net Turnover is total valid bets that we use to compute commission.",
                "- Data of Product Name will be delayed up to 5 minutes after settling tickets.",
                "- Turnover and Net Turnover of Poker Games are the buy-in/top up amount on table."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}