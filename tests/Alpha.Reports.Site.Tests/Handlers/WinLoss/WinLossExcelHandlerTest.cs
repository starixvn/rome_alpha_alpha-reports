namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
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
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossExcelHandlerTest
    {
        private readonly ProductService productService;
        private readonly WinLossService reportService;
        private readonly Workbook<WinLossModel> winlossWorkbook;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossExcelHandlerTest(UserFixture userFixture)
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

            this.userFixture = userFixture;
            winLossExcelHandler = Substitute.ForPartsOf<WinLossExcelHandler>(productService, reportService, winlossWorkbook);
            winLossExcelHandler.MapPath = (path) => path;
            winLossExcelHandler.IsFileExist = (path) => true;
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
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

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
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Title(Arg.Is("Master Win Loss 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());

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
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());

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
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossQuery_MasterWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossQuery_AgentWinLoss_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId: 4);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void HandleExportWinLossQuery_MemberWinLoss_ExportsExcelToBytes(byte custLevelId)
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void GetFileName_Always_ReturnsCorrectNameByCustLevelId(byte custLevelId, string expectedName)
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId);

            // Act
            var actualName = winLossExcelHandler.GetFileName(message.CustLevel);

            // Assert
            Assert.Equal(expectedName, actualName);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winLossExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        private WinLossExcelQuery CreateExportWinlossExcelQuery(byte custLevelId)
        {
            var exportWinLossQuery = new WinLossExcelQuery(userFixture.User);
            exportWinLossQuery.CustLevelId = custLevelId;
            exportWinLossQuery.FromDate = DefaultVariables.CurrentDate;
            exportWinLossQuery.ToDate = DefaultVariables.CurrentDate;

            return exportWinLossQuery;
        }
    }
}