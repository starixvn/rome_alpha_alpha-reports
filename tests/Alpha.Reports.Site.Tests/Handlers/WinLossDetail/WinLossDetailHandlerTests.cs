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
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Services;
    using Site.Handlers.WinLossDetail;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly IUser user;
        private readonly ReportData<WinLossDetailModel> reportData;

        public WinLossDetailHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            reportData = new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel));
            reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            user = Substitute.For<IUser>();
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
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void HandlerWinLossDetailQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(
            string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossDetailQuery(user);
            message.CustLevelId = 4;
            message.UserSelectedProductIds = userSelectedProductIds;

            var products = Enumerable.Empty<Product>();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var expectedWinLossDetailViewModel = new WinLossDetailViewModel
            {
                CustName = string.Empty,
                Language = string.Empty,
                SortPropertyName = "CustName",
                IsDescendingSort = false,
                UserSelectedProductIds = string.Empty,
                CustLevel = Enumeration.FromValue<UserLevel>(message.CustLevelId),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate,
                IsSetDefaultStackMode = string.IsNullOrWhiteSpace(userSelectedProductIds),
                IsFilterVisible = true,
                Title = "Master Win Loss Detail",

                Products = Enumerable.Empty<SelectListItem>(),
                ReportData = reportData,
                BreadcrumbLevels = new List<CustInfo>
                {
                    new CustInfo(0, string.Empty, 4)
                },
                BreadcrumbLevelsJson = "[{'CustId':0,'CustName':'','CustLevelId':4}]"
            };

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }
    }
}