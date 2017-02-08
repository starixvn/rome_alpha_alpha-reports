namespace Alpha.Reports.Site.Tests.Handlers.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinlossByBetType;
    using Services;
    using Site.Handlers.WinLossByBetType;
    using Site.Requests.WinLossByBetType;
    using Site.ViewModels.WinLossByBetType;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeHandlerTestsExcelHandlerTest
    {
        private readonly WinLossByBetTypeService reportService;
        private readonly Workbook<WinLossByBetTypeModel> winlossByBetTypeWorkbook;
        private readonly WinLossByBetTypeExcelHandler winLossByBetTypeExcelHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossByBetTypeHandlerTestsExcelHandlerTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            reportService = Substitute.For<WinLossByBetTypeService>();

            winlossByBetTypeWorkbook = Substitute.For<Workbook<WinLossByBetTypeModel>>();
            winlossByBetTypeWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winlossByBetTypeWorkbook);
            winlossByBetTypeWorkbook.Title(Arg.Any<string>()).Returns(winlossByBetTypeWorkbook);
            winlossByBetTypeWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winlossByBetTypeWorkbook);
            winlossByBetTypeWorkbook.Body(Arg.Any<IEnumerable<WinLossByBetTypeModel>>(), Arg.Any<Mapper<WinLossByBetTypeModel>>()).Returns(winlossByBetTypeWorkbook);
            winlossByBetTypeWorkbook.Footer(Arg.Any<WinLossByBetTypeModel>(), Arg.Any<Mapper<WinLossByBetTypeModel>>()).Returns(winlossByBetTypeWorkbook);
            winlossByBetTypeWorkbook.Sheet(Arg.Any<string>()).Returns(winlossByBetTypeWorkbook);

            winLossByBetTypeExcelHandler = Substitute.ForPartsOf<WinLossByBetTypeExcelHandler>(reportService, winlossByBetTypeWorkbook);
            winLossByBetTypeExcelHandler.MapPath = (path) => path;
            winLossByBetTypeExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariables.CurrentDate;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByBetTypeExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBetTypeExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().Title(Arg.Is("Win Loss By Bet Type 12/02/2016 --> 12/02/2016"));
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            var winlossByBetTypeViewModel = new WinLossByBetTypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLossByBetTypeModel>>(dataSource => 
                JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossByBetTypeViewModel.ReportData)),
                Arg.Any<Mapper<WinLossByBetTypeModel>>());
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);
            var winlossByBetTypeViewModel = new WinLossByBetTypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                new WinLossByBetTypeModel(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().Footer(
                Arg.Is<WinLossByBetTypeModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossByBetTypeViewModel.GrandTotal)),
                Arg.Any<Mapper<WinLossByBetTypeModel>>());
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_MasterWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossByBetTypeQuery_AgentWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossByBetTypeQuery_MemberWinLoss_ExportsExcelToBytes(byte custLevelId)
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winlossByBetTypeWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossByBetTypeQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportWinlossByBetTypeExcelQuery(custLevelId);

            // Act
            winLossByBetTypeExcelHandler.Handle(message);

            // Assert
            winLossByBetTypeExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        private WinLossByBetTypeExcelQuery CreateExportWinlossByBetTypeExcelQuery(byte custLevelId)
        {
            var exportWinLossByBetTypeQuery = new WinLossByBetTypeExcelQuery(userFixture.User);
            exportWinLossByBetTypeQuery.CustLevelId = custLevelId;
            exportWinLossByBetTypeQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossByBetTypeQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossByBetTypeQuery;
        }
    }
}