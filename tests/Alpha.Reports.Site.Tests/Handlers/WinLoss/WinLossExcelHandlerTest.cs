namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossExcelHandlerTest
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<WinLossModel> winlossWorkbook;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly IUser user;

        public WinLossExcelHandlerTest()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });
            reportService = Substitute.For<WinLossService>();

            winlossWorkbook = Substitute.For<Workbook<WinLossModel>>();
            winlossWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winlossWorkbook);
            winlossWorkbook.Title(Arg.Any<string>()).Returns(winlossWorkbook);
            winlossWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winlossWorkbook);
            winlossWorkbook.Body(Arg.Any<IEnumerable<WinLossModel>>(), Arg.Any<Mapper<WinLossModel>>()).Returns(winlossWorkbook);
            winlossWorkbook.Footer(Arg.Any<WinLossModel>(), Arg.Any<Mapper<WinLossModel>>()).Returns(winlossWorkbook);
            winlossWorkbook.Sheet(Arg.Any<string>()).Returns(winlossWorkbook);

            user = Substitute.For<IUser>();
            winLossExcelHandler = new WinLossExcelHandler(productService, reportService, winlossWorkbook);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossExcelHandler.Handle(nullMessage));
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
                tp => tp.NoInfomationText == "No information is available"));
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
            var winlossReportData = new List<WinLossModel>();
            var winlossViewModel = new WinLossViewModel(winlossReportData);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLossModel>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossViewModel.ReportData)),
                Arg.Any<Mapper<WinLossModel>>());
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(userLevel: 4);
            var winlossViewModel = new WinLossViewModel(new List<WinLossModel>());

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Footer(
                Arg.Is<WinLossModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossViewModel.GrantTotal)),
                Arg.Any<Mapper<WinLossModel>>());
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

        [Theory]
        [InlineData(4, "~/App_Data/ExcelTemplates/WinLoss/Master.xls")]
        [InlineData(3, "~/App_Data/ExcelTemplates/WinLoss/Agent.xls")]
        [InlineData(2, "~/App_Data/ExcelTemplates/WinLoss/Member.xls")]
        public void BuildExcelTemplatePath_Always_ReturnsCorrectPathByUserLevel(byte userLevelId, string expectedPath)
        {
            // Arrange
            var winLossQuery = new WinLossExcelQuery(user);
            winLossQuery.UserLevelId = userLevelId;

            // Act
            var actualPath = winLossQuery.BuildExcelTemplatePath();

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        private WinLossExcelQuery CreateExportWinlossExcelQuery(byte userLevel)
        {
            var exportWinLossQuery = new WinLossExcelQuery(user);
            exportWinLossQuery.UserLevelId = userLevel;
            exportWinLossQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossQuery;
        }
    }
}