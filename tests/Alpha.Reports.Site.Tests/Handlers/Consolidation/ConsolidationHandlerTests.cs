namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Params.Consolidation;
    using Reports.Models;
    using Reports.Models.Consolidation;
    using Site.Handlers.Consolidation;
    using Site.Requests.Consolidation;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.Components;
    using Site.ViewModels.Consolidation;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("UserCollection")]
    public class ConsolidationHandlerTests : ConsolidationTestsBaseFunctions
    {
        private readonly ConsolidationHandler consolidationHandler;
        private readonly List<SelectListItem> expectedProductList;
        private readonly List<SelectListItem> expectedCurrencyList;

        public ConsolidationHandlerTests(UserFixture userFixture) : base(userFixture)
        {
            consolidationHandler = new ConsolidationHandler(reportService, cacheRefService, winLossDetailService, viewer);
            consolidationHandler.GetCurrencyName = getCurrencyName;

            expectedProductList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Product 1", Value = "1", Selected = true },
                new SelectListItem { Text = "Product 2", Value = "2", Selected = false }
            };

            expectedCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "0", Selected = false },
                new SelectListItem { Text = "Currency 1", Value = "1", Selected = false },
                new SelectListItem { Text = "Currency 2", Value = "2", Selected = true }
            };
        }

        [Fact]
        public void HandleConsolidationByUsernameQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var data = new List<ConsolidationByUsername>
            {
                new ConsolidationByUsername("1", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
            };

            reportService.GetConsolidationByUsername(Arg.Any<GetConsolidationParam>()).Returns(data);

            var message = new ConsolidationByUsernameQuery(user)
            {
                UserSelectedProductIds = "1",
                UserSelectedCurrencyId = 2
            };

            var param = new GetConsolidationParam(
                message.CustId,
                message.UserSelectedCurrencyId,
                message.FromDate,
                message.ToDate,
                new List<int> { 1 });

            var expectedViewModel = new ConsolidationReportByUsernameViewModel(
                message,
                param,
                expectedProductList,
                expectedCurrencyList,
                new List<ConsolidationByUsernameViewModel>
                {
                    new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1")
                });

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleConsolidationByDateQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var data = new List<ConsolidationByDate>
            {
                new ConsolidationByDate("2016/12/2", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
            };

            reportService.GetConsolidationByDate(Arg.Any<GetConsolidationParam>()).Returns(data);

            var message = new ConsolidationByDateQuery(user)
            {
                UserSelectedProductIds = "1",
                UserSelectedCurrencyId = 2
            };

            var param = new GetConsolidationParam(
                message.CustId,
                message.UserSelectedCurrencyId,
                message.FromDate,
                message.ToDate,
                new List<int> { 1 });

            var expectedViewModel = new ConsolidationReportByDateViewModel(
                message,
                param,
                expectedProductList,
                expectedCurrencyList,
                new List<ConsolidationByDateViewModel>
                {
                    new ConsolidationByDateViewModel(
                        DefaultVariables.CurrentDate,
                        new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
                });

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleConsolidationByProductQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var data = new List<ConsolidationByProduct>
            {
                new ConsolidationByProduct("1", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
            };

            reportService.GetConsolidationByProduct(Arg.Any<GetConsolidationParam>()).Returns(data);

            var message = new ConsolidationByProductQuery(user)
            {
                UserSelectedProductIds = "1",
                UserSelectedCurrencyId = 2
            };

            var param = new GetConsolidationParam(
                message.CustId,
                message.UserSelectedCurrencyId,
                message.FromDate,
                message.ToDate,
                new List<int> { 1 });

            var expectedViewModel = new ConsolidationReportByProductViewModel(
                message,
                param,
                expectedProductList,
                expectedCurrencyList,
                new List<ConsolidationByProductViewModel>
                {
                    new ConsolidationByProductViewModel(
                        1,
                        "Product 1",
                        new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
                });

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void HandleConsolidationDetailQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new ConsolidationDetailQuery(viewer)
            {
                CustId = 1,
                CustName = "Name",
                CustLevelId = 4,
                FromDate = DefaultVariables.CurrentDate,
            };

            var dateRangePickerViewModel = new DateRangePickerViewModel(
                new DateTime(2016, 9, 1),
                DateTime.MaxValue,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                0);

            var products = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Product 1",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Product 2",
                    Value = "2"
                }
            };

            var productMultiSelectionViewModel = new ProductMultiSelectionViewModel(products, "1", string.Empty);

            var currencies = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "All",
                    Value = "0",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Currency 1",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Currency 2",
                    Value = "2"
                }
            };

            var currencySelectionViewModel = new CurrencySelectionViewModel(currencies, 0);

            var consolidationFilterViewModel = new ConsolidationFilterViewModel(
                0,
                string.Empty,
                true,
                new DateTime(2016, 9, 1),
                UserLevel.Agent,
                dateRangePickerViewModel,
                productMultiSelectionViewModel,
                currencySelectionViewModel);

            var winLossDetailQuery = new WinLossDetailQuery(viewer)
            {
                PageIndex = message.PageIndex,
                PageSize = message.PageSize,
                UserSelectedProductIds = message.DrilldownFromProductIds,
                SortPropertyName = message.SortPropertyName,
                IsDescendingSort = message.IsDescendingSort,
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,

                CustId = message.CustId,
                CustName = message.CustName,
                CustLevelId = message.CustLevelId,
                BreadcrumbLevel = message.BreadcrumbLevel
            };

            var winLossDetailParam = new WinLossDetailParam(
                winLossDetailQuery.CustId,
                winLossDetailQuery.PageIndex,
                winLossDetailQuery.PageSize,
                winLossDetailQuery.FromDate,
                winLossDetailQuery.ToDate,
                new List<int> { 1 },
                winLossDetailQuery.CustLevel,
                winLossDetailQuery.SortPropertyName,
                winLossDetailQuery.IsDescendingSort,
                winLossDetailQuery.IsHistoricReport);

            var winLossDetailViewModel = new WinLossDetailViewModel(
                winLossDetailQuery,
                winLossDetailParam,
                winLossDetailData,
                new List<CustInfo> { new CustInfo(1, "Name", 4) },
                null);

            var expectedViewModel = new ConsolidationDetailViewModel(
                message,
                new List<CustInfo> { new CustInfo(1, "Name", 4) },
                consolidationFilterViewModel,
                winLossDetailViewModel);

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
