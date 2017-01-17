namespace Alpha.Reports.Site.Tests.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Enumerations;
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
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly UserFixture userFixture;
        private readonly ReportData<WinLossDetailModel> reportData;

        public WinLossDetailHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string> { "Product Name" });

            reportData = new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel));
            var reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            this.userFixture = userFixture;
            winLossDetailHandler = new WinLossDetailHandler(productService, reportService);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandlerWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
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

            var winLossDetailParam = new WinLossDetailParam(0, 0, 0, DefaultVariables.CurrentDate, DefaultVariables.CurrentDate, Enumerable.Empty<int>(), message.CustLevel, "CustName", false, false);
            var expectedWinLossDetailViewModel = new WinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                new List<CustInfo>
                {
                    new CustInfo(0, string.Empty, 4)
                },
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }
    }
}