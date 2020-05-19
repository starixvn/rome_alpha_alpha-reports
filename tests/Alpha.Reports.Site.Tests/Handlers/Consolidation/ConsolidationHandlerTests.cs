namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
        private readonly List<GroupProductViewModel> expectedProductList;
        private readonly IEnumerable<SelectListItem> expectedExcludeProductList;
        private readonly List<SelectListItem> expectedCurrencyList;

        public ConsolidationHandlerTests(UserFixture userFixture) : base(userFixture)
        {
            consolidationHandler = new ConsolidationHandler(reportService, cacheRefService, winLossDetailService, viewer);

            expectedProductList = new List<GroupProductViewModel>
            {
                new GroupProductViewModel(
                    new SelectListGroup { Name = string.Empty },
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Product 1", Value = "1", Selected = true },
                        new SelectListItem { Text = "Product 2", Value = "2", Selected = false }
                    })
            };

            expectedExcludeProductList = Enumerable.Empty<SelectListItem>();

            expectedCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Text = "All Currencies", Value = "0", Selected = false },
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
                new ConsolidationByUsername(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0,  0,  0, 0, 0, 0, 1)
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
                    new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1")
                });

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleConsolidationByUsernameQuery_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = DateTime.Today.AddDays(1);
            var message = new ConsolidationByUsernameQuery(user)
            {
                UserSelectedProductIds = "1",
                UserSelectedCurrencyId = 2,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                LimitedDateToShowInfo = limitedDate
            };

            // Act
            consolidationHandler.Handle(message);

            // Assert
            reportService.Received().GetConsolidationByUsername(Arg.Is<GetConsolidationParam>(i => i.FromDate == limitedDate));
        }

        [Fact]
        public void HandleConsolidationByDateQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var data = new List<ConsolidationByDate>
            {
                new ConsolidationByDate("2016/12/2", 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
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
                        DefaultVariablesTest.CurrentDate,
                        new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
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
                new ConsolidationByProduct(1, 1, "Username", 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1)
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
                        new List<ConsolidationByUsernameViewModel> { new ConsolidationByUsernameViewModel(1, 1, "Username", 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, "Currency 1") })
                });

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleConsolidationDetailQuery_NullBreadcrumb_ReturnsCorrectViewModel()
        {
            var expectedBreadcrumbLevel = new List<CustInfo> {
                new CustInfo(0, "Consolidation", 2),
                new CustInfo(1, "Name", 4)
            };

            HandleConsolidationDetailQuery_NullBreadcrumb_ReturnsCorrectViewModel(null, expectedBreadcrumbLevel);
        }

        [Fact]
        public void HandleConsolidationDetailQuery_EmptyBreadcrumb_ReturnsCorrectViewModel()
        {
            var expectedBreadcrumbLevel = new List<CustInfo> {
                new CustInfo(0, "Consolidation", 2),
                new CustInfo(1, "Name", 4)
            };

            HandleConsolidationDetailQuery_NullBreadcrumb_ReturnsCorrectViewModel(Enumerable.Empty<CustInfo>(), expectedBreadcrumbLevel);
        }

        [Fact]
        public void HandleConsolidationDetailQuery_HasDataBreadcrumb_ReturnsCorrectViewModel()
        {
            var breadcrumbLevel = new List<CustInfo> {
                new CustInfo(1, "Name", 2),
                new CustInfo(3, "Name", 3),
                new CustInfo(4, "Name", 4)
            };

            var expectedBreadcrumbLevel = new List<CustInfo> {
                new CustInfo(0, "Consolidation", 2),
                new CustInfo(3, "Name", 3),
                new CustInfo(1, "Name", 4)
            };

            HandleConsolidationDetailQuery_NullBreadcrumb_ReturnsCorrectViewModel(breadcrumbLevel, expectedBreadcrumbLevel);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void HandleConsolidationDetailQuery_NullBreadcrumb_ReturnsCorrectViewModel(
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<CustInfo> expectedBreadcrumbLevel)
        {
            // Arrange
            var message = new ConsolidationDetailQuery(viewer)
            {
                CustId = 1,
                CustName = "Name",
                CustLevelId = 4,
                FromDate = DefaultVariablesTest.CurrentDate,
                BreadcrumbLevel = breadcrumbLevel
            };

            var dateRangePickerViewModel = new DateRangePickerViewModel(
                new DateTime(2016, 9, 1),
                DateTime.MaxValue,
                DefaultVariablesTest.CurrentDate,
                DefaultVariablesTest.CurrentDate,
                0);

            var products = new List<GroupProductViewModel>
            {
                new GroupProductViewModel(
                    new SelectListGroup
                    {
                        Name = string.Empty
                    },
                    new List<SelectListItem>
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
                    })
            };

            var productMultiSelectionViewModel = new ProductMultiSelectionViewModel(products, "1", string.Empty);

            var currencies = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "All Currencies",
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
                UserSelectedProductIds = message.UserSelectedProductIds,
                SortPropertyName = message.SortPropertyName,
                IsDescendingSort = message.IsDescendingSort,
                IsStackMode = message.IsStackMode,
                IsFilterVisible = message.IsFilterVisible,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                LimitedDateToShowInfo = DateTime.MinValue,
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
                winLossDetailQuery.LimitedDateToShowInfo,
                new List<int> { 1 },
                winLossDetailQuery.CustLevel,
                winLossDetailQuery.SortPropertyName,
                winLossDetailQuery.IsDescendingSort,
                winLossDetailQuery.IsHistoricReport);

            var winLossDetailViewModel = new WinLossDetailViewModel(
                winLossDetailQuery,
                winLossDetailParam,
                winLossDetailData,
                expectedBreadcrumbLevel,
                null,
                isGroupHeadline: true);

            var expectedViewModel = new ConsolidationDetailViewModel(
                message,
                expectedBreadcrumbLevel,
                consolidationFilterViewModel,
                winLossDetailViewModel);

            // Act
            var actualViewModel = consolidationHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }
    }
}
