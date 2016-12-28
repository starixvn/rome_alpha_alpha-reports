namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Adapters;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Services;
    using Site.Handlers;
    using Site.Helpers;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Site.ViewModels.WinLossDetails;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        private readonly Workbook<WinLossDetail> workbook;
        private readonly WinLossDetailHandler winLossDetailHandler;

        private readonly IUserSession userSession;
        private readonly ReportData<WinLossDetail> reportData;

        public WinLossDetailHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            reportData = new ReportData<WinLossDetail>(Enumerable.Empty<WinLossDetail>(), 0, 0, 0, default(WinLossDetail));
            reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            workbook = Substitute.For<Workbook<WinLossDetail>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetail>>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetail>(), Arg.Any<Mapper<WinLossDetail>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            userSession = Substitute.For<IUserSession>();
            winLossDetailHandler = new WinLossDetailHandler(productService, reportService, workbook, userSession);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            HttpAdapter.FuncServerMapPath = (path) => path;
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
            var message = new WinLossDetailQuery(userSession);
            message.UserLevelId = 4;
            message.UserSelectedProductIds = userSelectedProductIds;

            var products = Enumerable.Empty<Product>();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var routeValue = new WinLossDetailQuery(userSession);
            routeValue.UserLevelId = 4;
            routeValue.UserSelectedProductIds = userSelectedProductIds;
            routeValue.BreadcrumbLevels = "[{'CustId':0,'CustName':'','UserLevelId':4}]";

            var expectedWinLossDetailViewModel = new WinLossDetailViewModel
            {
                CustName = string.Empty,
                Language = string.Empty,
                SortPropertyName = "SuperNetWinLoss.WinLoss",
                IsDescendingSort = true,
                IsDirectReportViewAction = true,
                UserSelectedProductIds = string.Empty,
                UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate,
                HasSubmittedParams = !string.IsNullOrWhiteSpace(userSelectedProductIds),
                IsFilterVisible = true,
                Title = "Master Win Loss Detail",

                Products = Enumerable.Empty<SelectListItem>(),
                ReportData = reportData,
                BreadcrumbLevels = new List<ActionLinkViewModel>
                {
                    new ActionLinkViewModel(
                        controllerName: "WinLossDetail",
                        actionName: "Index",
                        linkText: message.CustName,
                        routeValues: routeValue)
                }
            };

            // Act
            var actualWinLossDetailViewModel = winLossDetailHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.TemplatePath == "~/App_Data/ExcelTemplates/WinLossDetail/Master.xls" &&
                tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "B" &&
                tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>("Master Win Loss Detail 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<WinLossDetail>>(reportData.DataSource),
                Arg.Any<Mapper<WinLossDetail>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Footer(Arg.Is<WinLossDetail>(reportData.TotalData), Arg.Any<Mapper<WinLossDetail>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is<string>("Master Win Loss Detail"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 3);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MemberWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 2);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        private WinLossDetailExcelQuery CreateExportExcelQuery(byte userLevel)
        {
            var result = new WinLossDetailExcelQuery(userSession);
            result.UserLevelId = userLevel;
            result.FromDate = DefaultVariables.CurrentDate;
            result.ToDate = DefaultVariables.CurrentDate;

            return result;
        }
    }
}
