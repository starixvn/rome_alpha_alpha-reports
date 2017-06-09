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
                new Product(1, "Product 1", null, getProductDisplayName),
                new Product(2, "Product 2", null, getProductDisplayName),
                new Product(3, "Product 3", null, getProductDisplayName)
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
        public void BuildBreadcrumbLevel_DrillDownInfoHasNoData_ReturnDrillDownInfoWithCurrentCust()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 1,
                CustName = "Master",
                CustLevelId = 3
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevel(query);

            // Assert
            AssertExtension.Equal(
                new List<CustInfo>
                {
                    new CustInfo(1, "Master", 3)
                },
                actualDrillDownInfo);
        }

        [Fact]
        public void BuildBreadcrumbLevel_DrillDownInfoHasData_ReturnPreviousDrillDownInfoAndCombinedWithCurrentCust()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 2,
                CustName = "Master",
                CustLevelId = 3,
                BreadcrumbLevel = new List<CustInfo>()
                {
                    new CustInfo(1, "SUPER1", 4)
                }
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevel(query);

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
        public void BuildBreadcrumbLevel_DrillDownInfoContainsCurrentCust_RemoveDownlineDrillDownInfo()
        {
            // Arrange
            var query = new ReportBaseQuery
            {
                CustId = 1,
                CustName = "SUPER1",
                CustLevelId = 4,
                BreadcrumbLevel = new List<CustInfo>()
                {
                    new CustInfo(1, "SUPER1", 4),
                    new CustInfo(2, "Master1", 3)
                }
            };

            // Act
            var actualDrillDownInfo = baseHandler.BuildBreadcrumbLevel(query);

            // Assert
            AssertExtension.Equal(
                new List<CustInfo>
                {
                    new CustInfo(1, "SUPER1", 4)
                },
                actualDrillDownInfo);
        }

        [Fact]
        public void BuildLeagueSelectListItems_NullLeagues_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<League> leagues = null;
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildLeagueSelectListItems(Enumerable.Empty<int>(), leagues);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildLeagueSelectListItems_EmptyLeagues_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<League> leagues = Enumerable.Empty<League>();
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildLeagueSelectListItems(Enumerable.Empty<int>(), leagues);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildLeagueSelectListItems_EmptyUserSelectedLeagueIds_ReturnsSelectListItem()
        {
            // Arrange
            IEnumerable<League> leagues = new List<League> { new League(1, 1, string.Empty) };
            var expectedSelectListItems = new List<SelectListItem>
            {
                new SelectListItem { Text = string.Empty, Value = "1", Selected = false }
            };

            // Act
            var actualSelectListItems = baseHandler.BuildLeagueSelectListItems(Enumerable.Empty<int>(), leagues);

            // Assert
            AssertExtension.Equal(expectedSelectListItems, actualSelectListItems.ToList());
        }
    }
}