namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Extensions;
    using Reports.Models;
    using Site.Handlers;
    using Site.Requests;
    using Xunit;

    public class BaseHandlerTests
    {
        private const string ControllerName = "WinLossDetail";
        private const string ActionName = "Index";

        private readonly BaseHandler baseHandler;

        public BaseHandlerTests()
        {
            baseHandler = new BaseHandler();
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

        ////[Fact]
        ////public void BuildBreadcrumbLevels_NullQuery_ThrowsArgumentNullException()
        ////{
        ////    // Arrange
        ////    ReportBaseQuery message = null;

        ////    // Act & Assert
        ////    Assert.Throws<ArgumentNullException>(() => baseHandler.BuildBreadcrumbLevels(
        ////        Enumerable.Empty<CustInfo>(),
        ////        message,
        ////        ControllerName,
        ////        ActionName,
        ////        (custInfo, baseReportQuery, drillDownInfoJson) => default(WinLossDetailModel)));
        ////}

        ////[Fact]
        ////public void BuildBreadcrumbLevels_NullBuildActionLinkViewModelFunc_ThrowsArgumentNullException()
        ////{
        ////    // Arrange
        ////    Func<CustInfo, ReportBaseQuery, string, WinLossDetailModel> buildActionLinkViewModelFunc = null;
        ////    var message = new ReportBaseQuery();

        ////    // Act & Assert
        ////    Assert.Throws<ArgumentNullException>(() => baseHandler.BuildBreadcrumbLevels(
        ////        Enumerable.Empty<CustInfo>(),
        ////        message,
        ////        ControllerName,
        ////        ActionName,
        ////        buildActionLinkViewModelFunc));
        ////}

        ////[Theory]
        ////[InlineData(null)]
        ////[InlineData("")]
        ////[InlineData(" ")]
        ////[InlineData("  ")]
        ////public void BuildBreadcrumbLevels_QueryHasNullOrWhiteSpaceBreadcrumbLevels_ReturnsTopLevelBreadcrumbs(
        ////    string drillDownInfoJson)
        ////{
        ////    // Arrange
        ////    Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
        ////        (custInfo, baseReportQuery, drillDownInfoJsonByLevel) => new WinLossDetailQuery(user);

        ////    var message = new ReportBaseQuery
        ////    {
        ////        DrillDownInfoJson = drillDownInfoJson
        ////    };

        ////    var expectedDrilldownLevels = new List<ActionLinkViewModel>
        ////    {
        ////        new ActionLinkViewModel(ControllerName, ActionName, message.CustName, new WinLossDetailQuery(user))
        ////    };

        ////    // Act
        ////    var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(
        ////        baseHandler.BuildDrillDownInfo(message),
        ////        message,
        ////        ControllerName,
        ////        ActionName,
        ////        buildActionLinkViewModelFunc);

        ////    // Assert
        ////    AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        ////}

        ////[Fact]
        ////public void BuildBreadcrumbLevels_SubmitOnTopLevel_ReturnsTopLevelBreadcrumbs()
        ////{
        ////    // Arrange
        ////    Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
        ////        (custInfo, baseReportQuery, drillDownInfoJsonByLevel) => new WinLossDetailQuery(user);

        ////    var message = new ReportBaseQuery
        ////    {
        ////        DrillDownInfoJson = "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4}]",
        ////        CustLevelId = 4
        ////    };

        ////    var expectedDrilldownLevels = new List<ActionLinkViewModel>
        ////    {
        ////        new ActionLinkViewModel(ControllerName, ActionName, message.CustName, new WinLossDetailQuery(user))
        ////    };

        ////    // Act
        ////    var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(
        ////        baseHandler.BuildDrillDownInfo(message),
        ////        message,
        ////        ControllerName,
        ////        ActionName,
        ////        buildActionLinkViewModelFunc);

        ////    // Assert
        ////    AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        ////}

        ////[Fact]
        ////[SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        ////public void BuildBreadcrumbLevels_GetDownLineReport_ReturnsCorrectBreadcrumbs()
        ////{
        ////    // Arrange
        ////    var message = new WinLossDetailQuery(user);
        ////    message.DrillDownInfoJson = "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4}]";
        ////    message.CustLevelId = 3;
        ////    message.CustId = 2;
        ////    message.CustName = "MASTER1";

        ////    Func<CustInfo, ReportBaseQuery, string, WinLossDetailQuery> buildActionLinkViewModelFunc =
        ////        (custInfo, baseReportQuery, drillDownInfoJsonByLevel) =>
        ////        {
        ////            var query = new WinLossDetailQuery(user);

        ////            query.CustId = custInfo.CustId;
        ////            query.CustName = custInfo.CustName;
        ////            query.PageSize = message.PageSize;
        ////            query.FromDate = message.FromDate;
        ////            query.ToDate = message.ToDate;

        ////            query.UserSelectedProductIds = message.UserSelectedProductIds;
        ////            query.SortPropertyName = message.SortPropertyName;
        ////            query.IsDescendingSort = message.IsDescendingSort;
        ////            query.IsStackMode = message.IsStackMode;
        ////            query.IsHistoricReport = message.IsHistoricReport;
        ////            query.IsFilterVisible = message.IsFilterVisible;
        ////            query.UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
        ////            query.CustLevelId = custInfo.CustLevelId;
        ////            query.DrillDownInfoJson = drillDownInfoJsonByLevel;

        ////            return query;
        ////        };

        ////    var superWinLossDetailQuery = new WinLossDetailQuery(user);
        ////    superWinLossDetailQuery.CustId = 1;
        ////    superWinLossDetailQuery.CustName = "SUPER1";
        ////    superWinLossDetailQuery.CustLevelId = 4;
        ////    superWinLossDetailQuery.DrillDownInfoJson = "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4}]";

        ////    var masterWinLossDetailQuery = new WinLossDetailQuery(user);
        ////    masterWinLossDetailQuery.CustId = 2;
        ////    masterWinLossDetailQuery.CustName = "MASTER1";
        ////    masterWinLossDetailQuery.CustLevelId = 3;
        ////    masterWinLossDetailQuery.DrillDownInfoJson = "[{'CustId':1,'CustName':'SUPER1','CustLevelId':4},{'CustId':2,'CustName':'MASTER1','CustLevelId':3}]";

        ////    var expectedDrilldownLevels = new List<ActionLinkViewModel>
        ////    {
        ////        new ActionLinkViewModel(
        ////            ControllerName,
        ////            ActionName,
        ////            "SUPER1",
        ////            superWinLossDetailQuery),
        ////        new ActionLinkViewModel(
        ////            ControllerName,
        ////            ActionName,
        ////            "MASTER1",
        ////            masterWinLossDetailQuery)
        ////    };

        ////    // Act
        ////    var actualDrilldownLevels = baseHandler.BuildBreadcrumbLevels(
        ////        baseHandler.BuildDrillDownInfo(message),
        ////        message,
        ////        ControllerName,
        ////        ActionName,
        ////        buildActionLinkViewModelFunc);

        ////    // Assert
        ////    AssertExtension.Equal(expectedDrilldownLevels, actualDrilldownLevels);
        ////}

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetExcelTemplatePath_FileNameIsNullOrWhiteSpace_ThrowsArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.GetExcelTemplatePath(fileName));
        }

        [Theory]
        [InlineData("filePath", "xls", "~/App_Data/ExcelTemplates/Handlers/filePath.xls")]
        [InlineData("filePath1", "xls1", "~/App_Data/ExcelTemplates/Handlers/filePath1.xls1")]
        public void MapExcelTemplatePath_ReturnCorrectFilePathWithExtension(
            string fileName,
            string extension,
            string expectedPath)
        {
            // Arrange
            baseHandler.MapPath = path => path;
            baseHandler.IsFileExist = path => true;

            // Act
            var actualPath = baseHandler.GetExcelTemplatePath(fileName, extension);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Fact]
        public void MapExcelTemplatePath_TemplateFileDoesNotExist_ThrowFileNotFoundException()
        {
            // Arrange
            baseHandler.MapPath = path => path;
            baseHandler.IsFileExist = path => false;

            // Act && Assert
            Assert.Throws<FileNotFoundException>(() => baseHandler.GetExcelTemplatePath("reportName", "filename"));
        }

        [Fact]
        public void BuildBreadcrumbLevels_DrillDownInfoHasNoData_ReturnDrillDownInfoWithCurrentCust()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 1,
                CustName = "Master",
                CustLevelId = 3
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevels(query);

            // Assert
            AssertExtension.Equal(
                new List<CustInfo>
                {
                    new CustInfo(1, "Master", 3)
                },
                actualDrillDownInfo);
        }

        [Fact]
        public void BuildBreadcrumbLevels_DrillDownInfoHasData_ReturnPreviousDrillDownInfoAndCombinedWithCurrentCust()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 2,
                CustName = "Master",
                CustLevelId = 3,
                BreadcrumbLevels = new List<CustInfo>()
                {
                    new CustInfo(1, "SUPER1", 4)
                }
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevels(query);

            // Assert
            AssertExtension.Equal(
                new List<CustInfo>
                {
                    new CustInfo(1, "SUPER1", 4),
                    new CustInfo(2, "Master", 3)
                },
                actualDrillDownInfo);
        }

        [Fact]
        public void BuildBreadcrumbLevels_DrillDownInfoContainsCurrentCust_RemoveDownlineDrillDownInfo()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 1,
                CustName = "SUPER1",
                CustLevelId = 4,
                BreadcrumbLevels = new List<CustInfo>()
                {
                    new CustInfo(1, "SUPER1", 4),
                    new CustInfo(2, "Master1", 3)
                }
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevels(query);

            // Assert
            AssertExtension.Equal(
                new List<CustInfo>
                {
                    new CustInfo(1, "SUPER1", 4)
                },
                actualDrillDownInfo);
        }
    }
}