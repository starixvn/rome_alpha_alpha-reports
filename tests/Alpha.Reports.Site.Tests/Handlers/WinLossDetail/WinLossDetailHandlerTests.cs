namespace Alpha.Reports.Site.Tests.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Reports.Site.ViewModels.Components;
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
        private readonly ProductDetailService productDetailService;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly UserFixture userFixture;
        private readonly PagedReportData<WinLossDetailModel> reportData;

        public WinLossDetailHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string> { "Product Name" });
            productDetailService= Substitute.For<ProductDetailService>();

            reportData = new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel));
            var reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            this.userFixture = userFixture;
            winLossDetailHandler = new WinLossDetailHandler(productService, productDetailService, reportService, userFixture.User);
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
            productService.GetProductsForCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(products);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var winLossDetailParam = new WinLossDetailParam(
                10,
                "",
                1,
                0,
                0,
                DefaultVariablesTest.CurrentDate,
                DefaultVariablesTest.CurrentDate,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                message.CustLevel,
                "CustName",
                false,
                false,
                string.Empty,
                false);

            var expectedWinLossDetailViewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                new List<CustInfo> { new CustInfo(10, "Name", 4) },
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>());

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsNotContainLiveCasino_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>());

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
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(21, "AllBet", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.CustId, message.Site, message.CurrencyId).Returns(Enumerable.Empty<byte>());

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
        public void HandleReportLegendQuery_ProductsContainSexyGaming_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(45, "AE Casino", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For AE Casino, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainGTCasino_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(46, "GT Casino", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For GT Casino, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainSAGaming_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(39, "SA Gaming", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For SA Gaming, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainION_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(52, "ION", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For ION, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainWN_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(51, "WM", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For WM, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainBBin_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(47, "BBIN", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(message.CustId, message.Site, message.CurrencyId).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For BBIN, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleReportLegendQuery_ProductsContainLiveCasino_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ReportLegendQuery(userFixture.User);
            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(new List<Product>
            {
                new Product(21, "AllBet", null, 0, 0),
                new Product(45, "AE Casino", null, 0, 0),
                new Product(39, "SA Gaming", null, 0, 0),
                new Product(46, "GT Casino", null, 0, 0),
                new Product(47, "BBIN", null, 0, 0),
                new Product(52, "ION", null, 0, 0),
                new Product(51, "WM", null, 0, 0)
            });

            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(1, string.Empty, 1).Returns(Enumerable.Empty<byte>());

            var expectedViewModel = new List<string>
            {
                "- For Sportbook, Net Turnover is Turnover which excludes the draw tickets and halves of the turnover of the half won, half lost tickets.",
                "- For AllBet, AE Casino, SA Gaming, GT Casino, BBIN, ION, WM, Net Turnover is total valid bets that we use to compute commission."
            };

            // Act
            var actualViewModel = winLossDetailHandler.Handle(message);

            // Assert
            Assert.Equal(expectedViewModel, actualViewModel);
        }
    }
}