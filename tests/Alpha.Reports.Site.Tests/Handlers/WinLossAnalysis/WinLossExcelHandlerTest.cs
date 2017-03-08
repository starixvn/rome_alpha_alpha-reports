namespace Alpha.Reports.Site.Tests.Handlers.WinLossAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinLossAnalysis;
    using Services;
    using Site.Handlers.WinLossAnalysis;
    using Site.Requests.WinLossAnalysis;
    using Site.ViewModels.WinLossAnalysis;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("UserCollection")]
    public class WinLossAnalysisExcelHandlerTest
    {
        private readonly ProductService productService;
        private readonly WinLossAnalysisService reportService;
        private readonly Workbook<WinLossAnalysisModel> winLossAnalysisWorkbook;
        private readonly WinLossAnalysisExcelHandler winLossAnalysisExcelHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly WinLossAnalysisExcelQuery message;
        private readonly WinLossAnalysisViewModel viewModel;

        public WinLossAnalysisExcelHandlerTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            productService = Substitute.For<ProductService>();
            productService.ConcatProductNames(Arg.Any<string>()).Returns("Product Name");
            reportService = Substitute.For<WinLossAnalysisService>();

            winLossAnalysisWorkbook = Substitute.For<Workbook<WinLossAnalysisModel>>();
            winLossAnalysisWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winLossAnalysisWorkbook);
            winLossAnalysisWorkbook.Title(Arg.Any<string>()).Returns(winLossAnalysisWorkbook);
            winLossAnalysisWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winLossAnalysisWorkbook);
            winLossAnalysisWorkbook.Body(Arg.Any<IEnumerable<WinLossAnalysisModel>>(), Arg.Any<Mapper<WinLossAnalysisModel>>()).Returns(winLossAnalysisWorkbook);
            winLossAnalysisWorkbook.Footer(Arg.Any<WinLossAnalysisModel>(), Arg.Any<Mapper<WinLossAnalysisModel>>()).Returns(winLossAnalysisWorkbook);
            winLossAnalysisWorkbook.Sheet(Arg.Any<string>()).Returns(winLossAnalysisWorkbook);

            winLossAnalysisExcelHandler = Substitute.ForPartsOf<WinLossAnalysisExcelHandler>(productService, reportService, winLossAnalysisWorkbook);
            winLossAnalysisExcelHandler.MapPath = (path) => path;
            winLossAnalysisExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariables.CurrentDate;

            message = CreateExportWinLossAnalysisExcelQuery(
                            custLevelId: 4,
                            reportTypeId: 2,
                            reportTypeName: "Monthly");

            viewModel = new WinLossAnalysisViewModel(
                reportData: Enumerable.Empty<WinLossAnalysisModel>(),
                grandTotal: fixture.Create<WinLossAnalysisModel>(),
                message: message,
                param: fixture.Create<WinLossAnalysisParam>(),
                products: Enumerable.Empty<SelectListItem>());

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossAnalysisExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossAnalysisExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_UsesCorrectTemplateInfo()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"));
        }

        [Theory]
        [InlineData(4, 1, "Daily", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(4, 2, "Monthly", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(4, 3, "Sport", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]

        [InlineData(3, 1, "Daily", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(3, 2, "Monthly", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(3, 3, "Sport", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]

        [InlineData(2, 1, "Daily", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(2, 2, "Monthly", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(2, 3, "Sport", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectTitle(
            byte custLevelId,
            byte reportTypeId,
            string reportTypeName,
            string expectedTitle)
        {
            // Arrange
            var query = CreateExportWinLossAnalysisExcelQuery(custLevelId, reportTypeId, reportTypeName);

            // Act
            winLossAnalysisExcelHandler.Handle(query);

            // Assert
            winLossAnalysisWorkbook.Received().Title(Arg.Is(expectedTitle));
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectHeader()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectBody()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLossAnalysisModel>>(
                    dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(viewModel.ReportData)),
                Arg.Any<Mapper<WinLossAnalysisModel>>());
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectFooter()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().Footer(
                Arg.Is<WinLossAnalysisModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(viewModel.GrandTotal)),
                Arg.Any<Mapper<WinLossAnalysisModel>>());
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectSheetName()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsExcelToBytes()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossAnalysisQuery_AgentWinLossAnalysis_ExportsExcelToBytes()
        {
            // Arrange
            // Act
            winLossAnalysisExcelHandler.Handle(message);

            // Assert
            winLossAnalysisWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossAnalysisQuery_MemberWinLossAnalysis_ExportsExcelToBytes(byte custLevelId)
        {
            // Arrange
            var query = CreateExportWinLossAnalysisExcelQuery(
                            custLevelId: custLevelId,
                            reportTypeId: 2,
                            reportTypeName: "Monthly");

            // Act
            winLossAnalysisExcelHandler.Handle(query);

            // Assert
            winLossAnalysisWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossAnalysisQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var query = CreateExportWinLossAnalysisExcelQuery(
                            custLevelId: custLevelId,
                            reportTypeId: 2,
                            reportTypeName: "Monthly");

            // Act
            winLossAnalysisExcelHandler.Handle(query);

            // Assert
            winLossAnalysisExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        private WinLossAnalysisExcelQuery CreateExportWinLossAnalysisExcelQuery(byte custLevelId, byte reportTypeId, string reportTypeName)
        {
            var query = new WinLossAnalysisExcelQuery(userFixture.User);
            query.CustLevelId = custLevelId;
            query.ReportTypeId = reportTypeId;
            query.ReportTypeName = reportTypeName;
            query.FromDate = DefaultVariables.CurrentDate;
            query.ToDate = DefaultVariables.CurrentDate;

            return query;
        }
    }
}