namespace Alpha.Reports.Site.Tests.Handlers
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
    using Models;
    using Models.WinlossDetails;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Requests.WinLossDetails;
    using Services;
    using Site.Handlers;
    using Site.Helpers;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using ViewModels.WinLossDetails;
    using Xunit;

    public class WinLossDetailHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;

        private readonly Workbook<WinLossDetail> workbook;
        private readonly WinLossDetailHandler winLossDetailHandler;
        private readonly IUserSession userSession;

        private readonly WinLossDetailViewModel masterWinLossDetail = new WinLossDetailViewModel
        {
            UserLevel = Enumeration.FromValue<UserLevel>(4),
            FromDate = DefaultVariables.CurrentDate,
            ToDate = DefaultVariables.CurrentDate,
            ReportData = new ReportData<WinLossDetail>(Enumerable.Empty<WinLossDetail>(), 0, 0, 0, default(WinLossDetail))
        };

        public WinLossDetailHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            reportService = Substitute.For<WinLossDetailService>();
            workbook = Substitute.For<Workbook<WinLossDetail>>();
            userSession = Substitute.For<IUserSession>();
            winLossDetailHandler = new WinLossDetailHandler(productService, reportService, workbook, userSession);

            DateTimeAdapter.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            HttpAdapter.ServerMapPathFunc = (path) => path;
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

            ReportData<WinLossDetail> reportData = new ReportData<WinLossDetail>(
                Enumerable.Empty<WinLossDetail>(),
                0,
                0,
                0,
                null);
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            var routeValue = new WinLossDetailQuery(userSession);
            routeValue.UserLevelId = 4;
            routeValue.UserSelectedProductIds = userSelectedProductIds;

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
            ExportWinLossDetailQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = new ExportWinLossDetailQuery(masterWinLossDetail);

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
            var message = new ExportWinLossDetailQuery(masterWinLossDetail);

            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);

            // Act
            winLossDetailHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is<string>(s => string.Equals(s, "Master Win Loss Detail 12/02/2016 --> 12/02/2016 of Product Name")));
        }
    }
}
