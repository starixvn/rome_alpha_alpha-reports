namespace Alpha.Reports.Site.Tests.Handlers.WinLossByBetType
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using RemotingRef;
    using Reports.Models.WinLossByProduct;
    using Services;
    using Site.Handlers.WinLossByProduct;
    using Site.Requests.WinLossByProduct;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeHandlerTests
    {
        private readonly WinLossByProductService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossByProductHandler winLossByProductHandler;

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<WinLossByProductService>();
            cacheRefService = Substitute.For<CacheRefService>();
            productService = Substitute.For<ProductService>();

            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            this.userFixture = userFixture;
            winLossByProductHandler = new WinLossByProductHandler(reportService, productService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossByBetTypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByProductQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandlerWinLossByBetTypeQuery_ValidMessage_ReturnCorrectReportData()
        {
            // Arrange
            var message = new WinLossByProductQuery(userFixture.User);
            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;

            var expectedReportData = Enumerable.Empty<WinLossByProductModel>();

            reportService.GetWinLossByProduct(Arg.Any<WinLossByProductParam>()).Returns(expectedReportData);

            // Act
            var actualWinLossByBetTypeViewModel = winLossByProductHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(actualWinLossByBetTypeViewModel.ReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }
      
        ////[Theory]
        ////[MemberData("WinLossByBetTypeViewModelSources", MemberType = typeof(WinLossByBetTypeViewModelTestCases))]
        ////public void AccumulateGrandTotal_ValidReportData_ReturnsCorrectGrandTotal(
        ////IEnumerable<WinLossByProductModel> data,
        ////WinLossByBetTypeModel expectedGrandTotal)
        ////{
        ////    // Arrange

        ////    // Act
        ////    var actualGrandTotal = winLossByBetTypeHandler.AccumulateGrandTotal(data);

        ////    // Assert
        ////    var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
        ////    Assert.True(result.AreEqual);
        ////}
    }
}