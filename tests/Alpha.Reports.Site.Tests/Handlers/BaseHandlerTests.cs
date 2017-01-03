namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Extensions;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Handlers;
    using Site.Requests;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class BaseHandlerTests
    {
        private const string ControllerName = "WinLossDetail";
        private const string ActionName = "Index";

        private readonly BaseHandler baseHandler;
        private readonly IUser user;

        public BaseHandlerTests()
        {
            baseHandler = new BaseHandler();
            user = Substitute.For<IUser>();
        }

        [Fact]
        public void BuildProductSelectListItems_NullProducts_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = null;
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(Enumerable.Empty<int>(), products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_EmptyProductList_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            var userSelectedProductIds = Enumerable.Empty<int>();
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_HasDataProductList_ReturnsCorrectSelectListItem()
        {
            // Arrange
            Func<int, string> getProductDisplayName = (id) => $"Product {id}";

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Product 1", getProductDisplayName),
                new Product(2, "Product 2", getProductDisplayName),
                new Product(3, "Product 3", getProductDisplayName)
            };

            var userSelectedProductIds = new List<int> { 1, 3 };
            IEnumerable<SelectListItem> expectedSelectListItems = new List<SelectListItem>
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
                    Value = "2",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Product 3",
                    Value = "3",
                    Selected = true
                }
            };

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products).ToList();

            // Assert
            AssertExtension.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildBreadcrumbLevels_NullQuery_ThrowsArgumentNullException()
        {
            // Arrange
            ReportBaseQuery message = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.BuildBreadcrumbLevels(
                message,
                ControllerName,
                ActionName,
                (custInfo, baseReportQuery, jsonBreadcrumbLevels) => default(WinLossDetailModel)));
        }

        [Fact]
        public void BuildBreadcrumbLevels_NullBuildActionLinkViewModelFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<CustInfo, ReportBaseQuery, string, WinLossDetailModel> buildActionLinkViewModelFunc = null;
            var message = new ReportBaseQuery();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.BuildBreadcrumbLevels(
                message,
                ControllerName,
                ActionName,
                buildActionLinkViewModelFunc));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void BuildBreadcrumbLevels_QueryHasNullOrWhiteSpaceBreadcrumbLevels_ReturnsTopLevelBreadcrumbs(
            string breadcrumbLevels)
        {
            // Arrange
            Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
                (custInfo, baseReportQuery, jsonBreadcrumbLevels) => new WinLossDetailQuery(user);

            var message = new ReportBaseQuery
            {
                BreadcrumbLevels = breadcrumbLevels
            };

            var expectedDrilldownLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel(ControllerName, ActionName, message.CustName, new WinLossDetailQuery(user))
            };

            // Act
            var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(message, ControllerName, ActionName, buildActionLinkViewModelFunc);

            // Assert
            AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        }

        [Fact]
        public void BuildBreadcrumbLevels_SubmitOnTopLevel_ReturnsTopLevelBreadcrumbs()
        {
            // Arrange
            Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
                (custInfo, baseReportQuery, jsonBreadcrumbLevels) => new WinLossDetailQuery(user);

            var message = new ReportBaseQuery
            {
                BreadcrumbLevels = "[{'CustId':1,'CustName':'SUPER1','UserLevelId':4}]",
                UserLevelId = 4
            };

            var expectedDrilldownLevels = new List<ActionLinkViewModel>
            {
                new ActionLinkViewModel(ControllerName, ActionName, message.CustName, new WinLossDetailQuery(user))
            };

            // Act
            var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(message, ControllerName, ActionName, buildActionLinkViewModelFunc);

            // Assert
            AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        }

        [Fact]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void BuildBreadcrumbLevels_GetDownLineReport_ReturnsCorrectBreadcrumbs()
        {
            // Arrange
            var message = new WinLossDetailQuery(user);
            message.BreadcrumbLevels = "[{'CustId':1,'CustName':'SUPER1','UserLevelId':4}]";
            message.UserLevelId = 3;
            message.CustId = 2;
            message.CustName = "MASTER1";

            Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
                (custInfo, baseReportQuery, jsonBreadcrumbLevels) =>
                {
                    var query = new WinLossDetailQuery(user);

                    query.CustId = custInfo.CustId;
                    query.CustName = custInfo.CustName;
                    query.PageSize = message.PageSize;
                    query.FromDate = message.FromDate;
                    query.ToDate = message.ToDate;

                    query.UserSelectedProductIds = message.UserSelectedProductIds;
                    query.SortPropertyName = message.SortPropertyName;
                    query.IsDescendingSort = message.IsDescendingSort;
                    query.IsStackMode = message.IsStackMode;
                    query.IsHistoricReport = message.IsHistoricReport;
                    query.IsFilterVisible = message.IsFilterVisible;
                    query.UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
                    query.UserLevelId = custInfo.UserLevelId;
                    query.BreadcrumbLevels = jsonBreadcrumbLevels;

                    return query;
                };

            var winLossDetailQuery = new WinLossDetailQuery(user);
            winLossDetailQuery.CustId = 1;
            winLossDetailQuery.CustName = "SUPER1";
            winLossDetailQuery.UserLevelId = 4;
            winLossDetailQuery.BreadcrumbLevels = message.BreadcrumbLevels;

            var expectedDrilldownLevels = new List<ActionLinkViewModel>();
            expectedDrilldownLevels.Add(new ActionLinkViewModel(
                    controllerName: ControllerName,
                    actionName: ActionName,
                    linkText: "SUPER1",
                    routeValues: winLossDetailQuery));

            winLossDetailQuery = new WinLossDetailQuery(user);
            winLossDetailQuery.CustId = 2;
            winLossDetailQuery.CustName = "MASTER1";
            winLossDetailQuery.UserLevelId = 3;
            winLossDetailQuery.BreadcrumbLevels = "[{'CustId':1,'CustName':'SUPER1','UserLevelId':4},{'CustId':2,'CustName':'MASTER1','UserLevelId':3}]";

            expectedDrilldownLevels.Add(new ActionLinkViewModel(
                    ControllerName,
                    ActionName,
                    linkText: message.CustName,
                    routeValues: winLossDetailQuery));

            // Act
            var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(
                message,
                ControllerName,
                ActionName,
                buildActionLinkViewModelFunc);

            // Assert
            AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void MapExcelFilePath_ReportNameIsNullOrWhiteSpace_ThrowsArgumentNullException(string reportName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.MapExcelFilePath(reportName, "filename"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void MapExcelFilePath_FileNameIsNullOrWhiteSpace_ThrowsArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.MapExcelFilePath("reportName", fileName));
        }

        [Theory]
        [InlineData("reportName", "filePath", "xls", "~/App_Data/ExcelTemplates/reportName/filePath.xls")]
        [InlineData("reportName1", "filePath1", "xls1", "~/App_Data/ExcelTemplates/reportName1/filePath1.xls1")]
        public void MapExcelFilePath_ReturnCorrectFilePathWithExtension(
            string reportName, 
            string fileName, 
            string extension, 
            string expectedPath)
        {
            // Arrange
            baseHandler.MapPathFunc = path => path;

            // Act
            var actualPath = baseHandler.MapExcelFilePath(reportName, fileName, extension);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }
    }
}