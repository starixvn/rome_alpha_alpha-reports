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
    using Reports.Models;
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
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            message = CreateExportWinLossAnalysisExcelQuery(
                            custLevelId: 4,
                            reportTypeId: 2,
                            reportTypeName: "Monthly",
                            reportTitle: "Monthly Win Loss Analysis");

            viewModel = new WinLossAnalysisViewModel(
                reportData: Enumerable.Empty<WinLossAnalysisModel>(),
                grandTotal: fixture.Create<WinLossAnalysisModel>(),
                message: message,
                param: fixture.Create<WinLossAnalysisParam>(),
                products: Enumerable.Empty<SelectListItem>(),
                excludeProducts: Enumerable.Empty<SelectListItem>());

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
        [InlineData(4, 1, "Daily", "Daily Win Loss Analysis", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(4, 2, "Monthly", "Monthly Win Loss Analysis", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(4, 3, "Sport", "Sport Win Loss Analysis", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]

        [InlineData(3, 1, "Daily", "Daily Win Loss Analysis", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(3, 2, "Monthly", "Monthly Win Loss Analysis", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(3, 3, "Sport", "Sport Win Loss Analysis", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]

        [InlineData(2, 1, "Daily", "Daily Win Loss Analysis", "Daily Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(2, 2, "Monthly", "Monthly Win Loss Analysis", "Monthly Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        [InlineData(2, 3, "Sport", "Sport Win Loss Analysis", "Sport Win Loss Analysis 12/02/2016 --> 12/02/2016 of Product Name")]
        public void HandleExportWinLossAnalysisQuery_MasterWinLossAnalysis_ExportsCorrectTitle(
            byte custLevelId,
            byte reportTypeId,
            string reportTypeName,
            string reportTypeTitle,
            string expectedTitle)
        {
            // Arrange
            var query = CreateExportWinLossAnalysisExcelQuery(custLevelId, reportTypeId, reportTypeName, reportTypeTitle);

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
            winLossAnalysisWorkbook.Received().Header(Arg.Is<Func<string, string>>(
                func => func("ReportTypeName") == "Monthly" && func("WinLoss") == "Win/Loss"));
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
                            reportTypeName: "Monthly",
                            reportTitle: "Monthly Win Loss Analysis");

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
                            reportTypeName: "Monthly",
                            reportTitle: "Monthly Win Loss Analysis");

            // Act
            winLossAnalysisExcelHandler.Handle(query);

            // Assert
            winLossAnalysisExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithRollupKey()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossAnalysisExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](viewModel) == "RollupKey");
        }

        [Fact]
        public void GetRowMapper_TotalRow_MapColumnAWithTotalLabel()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossAnalysisExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, true);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](viewModel) == "Total");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRowMapper_AllLevel_ReturnCorrectMapper(byte roleId)
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, null, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(roleId);
            var message = new WinLossAnalysisExcelQuery(user);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (int)actualMapper.Mappings[Column.B](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == 3);
        }

        [Fact]
        public void GetRowMapper_AgentLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, null, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossAnalysisExcelQuery(user);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True((decimal)actualMapper.Mappings[Column.J](viewModel) == -6);
        }

        [Fact]
        public void GetRowMapper_MasterLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, netWinLoss, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)3);
            var message = new WinLossAnalysisExcelQuery(user);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.J](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.K](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.L](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.M](viewModel) == -9);
        }

        [Fact]
        public void GetRowMapper_SuperLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossAnalysisModel("RollupKey", 1, 2, 3, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var message = new WinLossAnalysisExcelQuery(user);

            // Act
            var actualMapper = winLossAnalysisExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.J](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.K](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.L](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.M](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.N](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.O](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.P](viewModel) == -12);
        }

        private WinLossAnalysisExcelQuery CreateExportWinLossAnalysisExcelQuery(byte custLevelId, byte reportTypeId, string reportTypeName, string reportTitle)
        {
            var query = new WinLossAnalysisExcelQuery(userFixture.User);
            query.CustLevelId = custLevelId;
            query.ReportTypeId = reportTypeId;
            query.ReportTypeName = reportTypeName;
            query.ReportTypeTitle = reportTitle;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate;

            return query;
        }
    }
}