namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly WinLossHandler winLossHandler;
        private readonly UserFixture userFixture;
        private CompareLogic compareLogic;
        
        public WinLossHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });
            reportService = Substitute.For<WinLossService>();

            this.userFixture = userFixture;
            winLossHandler = new WinLossHandler(productService, reportService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            DateTimeManager.Now = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("1")]
        [InlineData("1,2")]
        [InlineData("1,2,3")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void HandlerWinLossQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(
            string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossQuery(userFixture.User);
            var custInfoData = new List<CustInfo>
                {
                    new CustInfo(1, "CustName", 3)
                };

            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;
            message.UserSelectedProductIds = userSelectedProductIds;
            message.IsDescendingSort = false;
            message.SortPropertyName = "CustName";
            message.BreadcrumbLevels = custInfoData;

            var winLossParam = new WinLossParam(
                0,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                Enumerable.Empty<int>(),
                message.CustLevel,
                false);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var expectedWinLossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                winLossParam,
                custInfoData,
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualWinLossViewModel = winLossHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedWinLossViewModel, actualWinLossViewModel);
            Assert.True(result.AreEqual);
        }
    }
}