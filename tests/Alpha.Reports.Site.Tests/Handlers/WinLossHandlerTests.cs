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
    using Models.Winloss;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Extensions;
    using Reports.Tests.TestCases;
    using Services;
    using Site.Handlers;
    using Site.Helpers;
    using Site.Requests.WinLoss;
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
        private readonly Workbook<EnteredMatchResult> enteredSportResultWorkbook;
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

            enteredSportResultWorkbook = Substitute.For<Workbook<EnteredMatchResult>>();
            enteredSportResultWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Body(Arg.Any<IEnumerable<EnteredMatchResult>>(), Arg.Any<Mapper<EnteredMatchResult>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Sheet(Arg.Any<string>()).Returns(enteredSportResultWorkbook);

            userSession = Substitute.For<IUserSession>();
            winLossHandler = new WinLossHandler(productService, reportService, enteredSportResultWorkbook, winlossWorkbook, userSession);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            HttpAdapter.FuncServerMapPath = (path) => path;
            HttpAdapter.HasContext = true;
        }

        [Fact]
        public void HandleEnteredResultQuery_NullMessage_ThrowsException()
        {
            // Arrange
            EnteredMatchResultQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleEnteredResultQuery_ValidMessage_ReturnsDefaultData()
        {
            // Arrange
            var message = new EnteredMatchResultQuery(userSession);
            message.UserLevelId = (byte)4;
            message.CustId = 1596;

            var expectedResult = new EnteredResultViewModel
            {
                UserLevel = Enumeration.FromValue<UserLevel>(4),
                FromDate = DateTimeManager.Now,
                ToDate = DateTimeManager.Now,
                Language = message.Language,
                ReportData = Enumerable.Empty<EnteredMatchResult>(),
                UserSelectedDateRangeOption = 0
            };

            // Act
            var actualResult = winLossHandler.Handle(message);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(actualResult));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_NullMessage_ThrowsException()
        {
            // Arrange
            EnteredMatchResultExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleWinLossQuery_NullMessage_ThrowsException()
        {
            // Arrange
            WinLossQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossQuery_NullMessage_ThrowsException()
        {
            // Arrange
            WinLossExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.TemplatePath == "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls" &&
                tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);
            var enteredResultReportData = Enumerable.Empty<EnteredMatchResult>();

            // Act
            winLossHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Body(
                Arg.Is<IEnumerable<EnteredMatchResult>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(enteredResultReportData)),
                Arg.Any<Mapper<EnteredMatchResult>>());
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Sheet(Arg.Is<string>("Master Win Loss"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandlerWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_UsesCorrectTemplateInfo()
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
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Title(Arg.Is<string>("Master Win Loss 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Header(Arg.Is<Func<string, string>>(ExcelHelper.HeaderTitleMapping));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);
            var winlossReportData = new List<WinLoss>();
            var winlossViewModel = new WinLossViewModel(winlossReportData);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLoss>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossViewModel.ReportData)),
                Arg.Any<Mapper<WinLoss>>());
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);
            var winlossViewModel = new WinLossViewModel(new List<WinLoss>());

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Footer(
                Arg.Is<WinLoss>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossViewModel.GrantTotal)),
                Arg.Any<Mapper<WinLoss>>());
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Sheet(Arg.Is<string>("Master Win Loss"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossQuery_AgentWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossQuery_MemberWinLoss_ExportsExcelToBytes(byte userLevel)
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel);

            // Act
            winLossHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
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
        public void HandlerWinLossQuery_AnyUserSelectedProductIds_ReturnsCorrectViewModel(
            string userSelectedProductIds)
        {
            // Arrange
            var message = new WinLossQuery(userSession);
            message.UserLevelId = 3;
            message.UserSelectedProductIds = userSelectedProductIds;

            var products = Enumerable.Empty<Product>();
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(products);

            var routeValue = new WinLossQuery(userSession);
            routeValue.UserLevelId = 3;
            routeValue.UserSelectedProductIds = userSelectedProductIds;
            routeValue.BreadcrumbLevels = "[{'CustId':0,'CustName':'','UserLevelId':3}]";

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            var expectedWinLossViewModel = new WinLossViewModel(new List<WinLoss>())
            {
                CustName = string.Empty,
                Language = string.Empty,
                IsDirectReportViewAction = true,
                UserSelectedProductIds = string.Empty,
                UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId),
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate,
                IsFilterVisible = false,
                Products = Enumerable.Empty<SelectListItem>(),
                BreadcrumbLevels = new List<ActionLinkViewModel>
                {
                    new ActionLinkViewModel(
                        controllerName: "WinLoss",
                        actionName: "WinLoss",
                        linkText: message.CustName,
                        routeValues: routeValue)
                }
            };

            // Act
            var actualWinLossViewModel = winLossHandler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedWinLossViewModel, actualWinLossViewModel);
        }

        private WinLossExcelQuery CreateExportWinlossExcelQuery(byte userLevel)
        {
            var exportWinLossQuery = new WinLossExcelQuery(userSession);
            exportWinLossQuery.UserLevelId = userLevel;
            exportWinLossQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossQuery;
        }

        private EnteredMatchResultExcelQuery CreateExportEnteredResultsExcelQuery(byte userLevel)
        {
            var exportEnteredResultQuery = new EnteredMatchResultExcelQuery(userSession);
            exportEnteredResultQuery.UserLevelId = userLevel;

            return exportEnteredResultQuery;
        }
    }
}