namespace Alpha.Reports.Site.Tests.Handlers.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Services;
    using Site.Handlers.WinLossByProduct;
    using Site.Requests.WinLossByProduct;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductHandlerTests
    {
        private readonly WinLossByProductService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private readonly WinLossByProductHandler winLossByProductHandler;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossByProductHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            compareLogic = new CompareLogic();

            reportService = Substitute.For<WinLossByProductService>();
            cacheRefService = Substitute.For<CacheRefService>();
            productService = Substitute.For<ProductService>();
            productDetailService = Substitute.For<ProductDetailService>();

            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            winLossByProductHandler = new WinLossByProductHandler(reportService, productService, productDetailService, cacheRefService);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void HandlerWinLossByProductQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByProductQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandlerWinLossByProductQuery_ValidMessage_ReturnCorrectReportData()
        {
            // Arrange
            var message = new WinLossByProductQuery(userFixture.User);
            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;

            var expectedReportData = Enumerable.Empty<WinLossByProductModel>();

            reportService.GetWinLossByProduct(Arg.Any<WinLossByProductParam>()).Returns(expectedReportData);

            // Act
            var actualWinLossByProductViewModel = winLossByProductHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(actualWinLossByProductViewModel.ReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [MemberData("WinLossByProductViewModelSources", MemberType = typeof(WinLossByProductViewModelTestCases))]
        public void AccumulateGrandTotal_ValidReportData_ReturnsCorrectGrandTotal(
            IEnumerable<WinLossByProductModel> data,
            WinLossByBetTypeModel expectedGrandTotal)
        {
            // Act
            var actualGrandTotal = winLossByProductHandler.AccumulateGrandTotal(data);

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }
    }
}