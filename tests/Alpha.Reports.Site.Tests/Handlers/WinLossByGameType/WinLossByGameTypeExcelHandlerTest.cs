namespace Alpha.Reports.Site.Tests.Handlers.WinLossByGameType
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
    using Reports.Models.WinlossByGameType;
    using Services;
    using Site.Handlers.WinLossByGameType;
    using Site.Requests.WinLossByGameType;
    using Site.ViewModels.WinLossByGameType;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeExcelHandlerTest
    {
        private readonly WinLossByGameTypeService reportService;
        private readonly Workbook<WinLossByGameTypeModel> winlossByGameTypeWorkbook;
        private readonly WinLossByGameTypeExcelHandler winLossByGameTypeExcelHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossByGameTypeExcelHandlerTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            reportService = Substitute.For<WinLossByGameTypeService>();

            winlossByGameTypeWorkbook = Substitute.For<Workbook<WinLossByGameTypeModel>>();
            winlossByGameTypeWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winlossByGameTypeWorkbook);
            winlossByGameTypeWorkbook.Title(Arg.Any<string>()).Returns(winlossByGameTypeWorkbook);
            winlossByGameTypeWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winlossByGameTypeWorkbook);
            winlossByGameTypeWorkbook.Body(Arg.Any<IEnumerable<WinLossByGameTypeModel>>(), Arg.Any<Mapper<WinLossByGameTypeModel>>()).Returns(winlossByGameTypeWorkbook);
            winlossByGameTypeWorkbook.Footer(Arg.Any<WinLossByGameTypeModel>(), Arg.Any<Mapper<WinLossByGameTypeModel>>()).Returns(winlossByGameTypeWorkbook);
            winlossByGameTypeWorkbook.Sheet(Arg.Any<string>()).Returns(winlossByGameTypeWorkbook);

            winLossByGameTypeExcelHandler = Substitute.ForPartsOf<WinLossByGameTypeExcelHandler>(reportService, winlossByGameTypeWorkbook);
            winLossByGameTypeExcelHandler.MapPath = (path) => path;
            winLossByGameTypeExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariables.CurrentDate;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByGameTypeExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByGameTypeExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"));
        }

        ////[Fact]
        ////public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsCorrectTitle()
        ////{
        ////    // Arrange
        ////    var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

        ////    // Act
        ////    winLossByGameTypeExcelHandler.Handle(message);

        ////    // Assert
        ////    winlossByGameTypeWorkbook.Received().Title(Arg.Is("Win Loss By Bet Type 12/02/2016 --> 12/02/2016"));
        ////}

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            var winlossByGameTypeViewModel = new WinLossByGameTypeViewModel(
                Enumerable.Empty<WinLossByGameTypeModel>(),
                fixture.Create<WinLossByGameTypeModel>(),
                message,
                fixture.Create<WinLossByGameTypeParam>());

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().Body(
                Arg.Is<IEnumerable<WinLossByGameTypeModel>>(dataSource => 
                JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossByGameTypeViewModel.ReportData)),
                Arg.Any<Mapper<WinLossByGameTypeModel>>());
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);
            var winlossByGameTypeViewModel = new WinLossByGameTypeViewModel(
                Enumerable.Empty<WinLossByGameTypeModel>(),
                new WinLossByGameTypeModel(),
                message,
                fixture.Create<WinLossByGameTypeParam>());

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().Footer(
                Arg.Is<WinLossByGameTypeModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossByGameTypeViewModel.GrandTotal)),
                Arg.Any<Mapper<WinLossByGameTypeModel>>());
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_MasterWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossByGameTypeQuery_AgentWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId: 4);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossByGameTypeQuery_MemberWinLoss_ExportsExcelToBytes(byte custLevelId)
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winlossByGameTypeWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossByGameTypeQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportWinlossByGameTypeExcelQuery(custLevelId);

            // Act
            winLossByGameTypeExcelHandler.Handle(message);

            // Assert
            winLossByGameTypeExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        private WinLossByGameTypeExcelQuery CreateExportWinlossByGameTypeExcelQuery(byte custLevelId)
        {
            var exportWinLossByGameTypeQuery = new WinLossByGameTypeExcelQuery(userFixture.User);
            exportWinLossByGameTypeQuery.CustLevelId = custLevelId;
            exportWinLossByGameTypeQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossByGameTypeQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossByGameTypeQuery;
        }
    }
}