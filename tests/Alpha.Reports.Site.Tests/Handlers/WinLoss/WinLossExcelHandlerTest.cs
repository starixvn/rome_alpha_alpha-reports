namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
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
    public class WinLossExcelHandlerTest
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;

        private readonly Workbook<WinLoss> winlossWorkbook;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly IUserSession userSession;

        public WinLossExcelHandlerTest()
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

            userSession = Substitute.For<IUserSession>();
            winLossExcelHandler = new WinLossExcelHandler(productService, reportService, winlossWorkbook, userSession);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossExcelHandler.Handle(message);

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
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Title(Arg.Is<string>("Master Win Loss 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);
            var winlossReportData = new List<WinLoss>();
            var winlossViewModel = new WinLossViewModel(winlossReportData);

            // Act
            winLossExcelHandler.Handle(message);

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
            winLossExcelHandler.Handle(message);

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
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Sheet(Arg.Is<string>("Master Win Loss"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossQuery_AgentWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);

            // Act
            winLossExcelHandler.Handle(message);

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
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        private WinLossExcelQuery CreateExportWinlossExcelQuery(byte userLevel)
        {
            var exportWinLossQuery = new WinLossExcelQuery(userSession);
            exportWinLossQuery.UserLevelId = userLevel;
            exportWinLossQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossQuery;
        }

    }
}