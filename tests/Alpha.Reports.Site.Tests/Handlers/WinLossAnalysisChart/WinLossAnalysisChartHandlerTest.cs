namespace Alpha.Reports.Site.Tests.Handlers.WinLossAnalysisChart
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Extensions;
    using Reports.Models.WinLossAnalysisChart;
    using Services;
    using Site.Handlers.WinLossAnalysisChart;
    using Site.Requests.WinLossAnalysisChart;
    using Site.ViewModels.WinLossAnalysisChart;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    [Collection("UserCollection")]
    public class WinLossAnalysisChartHandlerTest
    {
        private readonly ProductService productService;
        private readonly WinLossAnalysisChartService reportService;
        private readonly WinLossAnalysisChartHandler winLossAnalysisChartHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossAnalysisChartHandlerTest(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            reportService = Substitute.For<WinLossAnalysisChartService>();

            this.userFixture = userFixture;
            winLossAnalysisChartHandler = new WinLossAnalysisChartHandler(productService, reportService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleWinLossAnalysisChartQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossAnalysisChartQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossAnalysisChartHandler.Handle(nullMessage));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("1")]
        [InlineData("1,2")]
        [InlineData("1,2,3")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void HandleWinLossAnalysisChartQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(string userSelectedProductIds)
        {
            // Arrange
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string> { "Product Name" });

            var message = new WinLossAnalysisChartQuery(userFixture.User)
            {
                CustId = 1,
                CustName = "CustName",
                CustLevelId = 3,
                UserSelectedProductIds = userSelectedProductIds
            };

            var reportData = Enumerable.Empty<WinLossAnalysisChartModel>();
            var param = new WinLossAnalysisChartParam(
                0,
                SystemTime.Today.AddMonths(-3).FirstDayOfMonth(),
                SystemTime.Today.AddMonths(-1).LastDayOfMonth(),
                Enumerable.Empty<int>(),
                message.CustLevel);

            reportService.GetWinLossAnalysisChart(Arg.Any<WinLossAnalysisChartParam>()).Returns(reportData);
            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var expectedWinLossAnalysisChartViewModel = new WinLossAnalysisChartViewModel(reportData, message, param, Enumerable.Empty<SelectListItem>());

            // Act
            var actualWinLossAnalysisViewModel = winLossAnalysisChartHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedWinLossAnalysisChartViewModel, actualWinLossAnalysisViewModel);
            Assert.True(result.AreEqual);
        }
    }
}