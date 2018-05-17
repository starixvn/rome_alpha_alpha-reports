namespace Alpha.Reports.Site.Tests.Handlers.WinLossAnalysis
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
    using Reports.Models.WinLossAnalysis;
    using Services;
    using Site.Handlers.WinLossAnalysis;
    using Site.Requests.WinLossAnalysis;
    using Site.ViewModels.WinLossAnalysis;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("UserCollection")]
    public class WinLossAnalysisHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossAnalysisService reportService;
        private readonly WinLossAnalysisHandler winLossAnalysisHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossAnalysisHandlerTests(UserFixture userFixture)
        {
            productService = Substitute.For<ProductService>();
            reportService = Substitute.For<WinLossAnalysisService>();

            this.userFixture = userFixture;
            winLossAnalysisHandler = new WinLossAnalysisHandler(productService, reportService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossAnalysisQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossAnalysisQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossAnalysisHandler.Handle(nullMessage));
        }

        [Theory]
        [MemberData("WinLossAnalysisViewModelSources", MemberType = typeof(WinLossAnalysisViewModelTestCases))]
        public void AccumulateGrandTotal_ValidReportData_ReturnsCorrectGrandTotal(
        IEnumerable<WinLossAnalysisModel> winLossAnalysisData,
        WinLossAnalysisModel expectedGrandTotal)
        {
            // Arrange

            // Act
            var actualGrandTotal = winLossAnalysisHandler.AccumulateGrandTotal(winLossAnalysisData);

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
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
        public void HandlerWinLossAnalysisQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(
            string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossAnalysisQuery(userFixture.User);
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;
            message.UserSelectedProductIds = userSelectedProductIds;

            var reportData = Enumerable.Empty<WinLossAnalysisModel>();
            var grandTotal = new WinLossAnalysisModel();
            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: SystemTime.Today,
                toDate: SystemTime.Today,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(),
                custLevel: message.CustLevel);

            reportService.GetWinLossAnalysis(Arg.Any<WinLossAnalysisParam>()).Returns(reportData);
            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var expectedWinLossAnalysisViewModel = new WinLossAnalysisViewModel(
                reportData,
                grandTotal,
                message,
                param,
                products: Enumerable.Empty<SelectListItem>());

            // Act
            var actualWinLossAnalysisViewModel = winLossAnalysisHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedWinLossAnalysisViewModel, actualWinLossAnalysisViewModel);
            Assert.True(result.AreEqual);
        }
    }
}