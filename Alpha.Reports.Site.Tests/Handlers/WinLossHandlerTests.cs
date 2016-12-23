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
    using Models.Winloss;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Services;
    using Site.Handlers;
    using Site.Helpers;
    using Site.Requests.WinLoss;
    using Site.Requests.WinLossDetails;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;

        private readonly Workbook<WinLoss> winlossWorkbook;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;
        private readonly WinLossHandler winLossHandler;

        private readonly IUserSession userSession;

        public WinLossHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            reportService = Substitute.For<WinLossService>();

            winlossWorkbook = Substitute.For<Workbook<WinLoss>>();
            winlossWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winlossWorkbook);
            winlossWorkbook.Title(Arg.Any<string>()).Returns(winlossWorkbook);
            winlossWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winlossWorkbook);
            winlossWorkbook.Body(Arg.Any<IEnumerable<WinLoss>>(), Arg.Any<Mapper<WinLoss>>()).Returns(winlossWorkbook);
            winlossWorkbook.Footer(Arg.Any<WinLoss>(), Arg.Any<Mapper<WinLoss>>()).Returns(winlossWorkbook);
            winlossWorkbook.Sheet(Arg.Any<string>()).Returns(winlossWorkbook);

            enteredSportResultWorkbook = Substitute.For<Workbook<EnteredSportResult>>();

            userSession = Substitute.For<IUserSession>();
            winLossHandler = new WinLossHandler(productService, reportService, winlossWorkbook, enteredSportResultWorkbook, userSession);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            HttpAdapter.ServerMapPathFunc = (path) => path;
            HttpAdapter.HasContext = true;
        }

        [Fact]
        public void HandlerWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        ////[Theory]
        ////[InlineData(null)]
        ////[InlineData("")]
        ////[InlineData(" ")]
        ////[InlineData("  ")]
        ////[InlineData("1")]
        ////[InlineData("1,2")]
        ////[InlineData("1,2,3")]
        ////[SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        ////public void HandlerWinLossQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(
        ////    string userSelectedProductIds)
        ////{
        ////    // Arrange
        ////    var message = new WinLossQuery(userSession);
        ////    message.UserLevelId = 4;
        ////    message.UserSelectedProductIds = userSelectedProductIds;

        ////    var products = Enumerable.Empty<Product>();
        ////    productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

        ////    var validUserSelectedProductIds = Enumerable.Empty<int>();
        ////    productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

        ////    var reportData = Enumerable.Empty<WinLoss>();

        ////    reportService.GetWinLoss(Arg.Any<WinLossParam>()).Returns(reportData);

        ////    var routeValue = new WinLossQuery(userSession);
        ////    routeValue.UserLevelId = 4;
        ////    routeValue.UserSelectedProductIds = userSelectedProductIds;

        ////    var expectedWinLossDetailViewModel = new WinLossViewModel(reportData.ToList())
        ////    {
        ////        CustName = string.Empty,
        ////        Language = string.Empty,
        ////        IsDirectReportViewAction = true,
        ////        UserSelectedProductIds = string.Empty,
        ////        UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId),
        ////        FromDate = DefaultVariables.CurrentDate,
        ////        ToDate = DefaultVariables.CurrentDate,
        ////        Products = Enumerable.Empty<SelectListItem>(),
        ////        ReportData = reportData.ToList(),
        ////        BreadcrumbLevels = new List<ActionLinkViewModel>
        ////        {
        ////            new ActionLinkViewModel(
        ////                controllerName: "WinLoss",
        ////                actionName: "WinLoss",
        ////                linkText: message.CustName,
        ////                routeValues: routeValue)
        ////        }
        ////    };

        ////    // Act
        ////    var actualWinLossDetailViewModel = winLossHandler.Handle(message);

        ////    // Assert
        ////    AssertExtension.Equal(expectedWinLossDetailViewModel, actualWinLossDetailViewModel);
        ////}

        [Fact]
        public void HandleExportWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            ExportWinLossQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.TemplatePath == "~/App_Data/ExcelTemplates/WinLoss/Master.xls" &&
                tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Title(Arg.Is<string>(s => string.Equals(s, "Master Win Loss 12/02/2016 --> 12/02/2016 of Product Name")));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Header(Arg.Is<Func<string, string>>(func => func == ExcelHelper.HeaderTitleMapping));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLoss>>(dataSource => dataSource == message.ReportData),
                Arg.Any<Mapper<WinLoss>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Footer(Arg.Is<WinLoss>(total => total == message.GrantTotal), Arg.Any<Mapper<WinLoss>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Sheet(Arg.Is<string>(sheetName => sheetName == message.Title));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 3);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MemberWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 2);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        private ExportWinLossQuery CreateExportWinlossExcelQuery(byte userLevel)
        {
            var viewModel = new WinLossViewModel(new List<WinLoss>())
            {
                UserLevel = Enumeration.FromValue<UserLevel>(userLevel),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            return new ExportWinLossQuery(viewModel);
        }
    }
}
