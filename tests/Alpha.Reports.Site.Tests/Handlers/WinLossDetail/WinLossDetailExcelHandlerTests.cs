namespace Alpha.Reports.Site.Tests.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Services;
    using Site.Handlers.WinLossDetail;
    using Site.Requests.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailExcelHandlerTests
    {
        private readonly Workbook<WinLossDetailModel> workbook;
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly WinLossDetailExcelHandler winLossDetailExcelHandler;
        private readonly ReportData<WinLossDetailModel> reportData;
        private readonly IUser user;

        public WinLossDetailExcelHandlerTests()
        {
            productService = Substitute.For<ProductService>();
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });

            reportData = new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel));
            reportService = Substitute.For<WinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(reportData);

            workbook = Substitute.For<Workbook<WinLossDetailModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<WinLossDetailModel>>(), Arg.Any<Mapper<WinLossDetailModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<WinLossDetailModel>(), Arg.Any<Mapper<WinLossDetailModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            user = Substitute.For<IUser>();
            winLossDetailExcelHandler = Substitute.ForPartsOf<WinLossDetailExcelHandler>(productService, reportService, workbook);
            winLossDetailExcelHandler.MapPath = (path) => path;
            winLossDetailExcelHandler.IsFileExist = (path) => true;
            DateTimeManager.Now = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available" &&
                tp.FreezeColumnName == "B" &&
                tp.FreezeRowIndex == 3));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is("Win Loss Detail 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is(reportData.DataSource),
                Arg.Any<Mapper<WinLossDetailModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Footer(Arg.Is(reportData.TotalData), Arg.Any<Mapper<WinLossDetailModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 4);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 3);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MemberWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: 2);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void BuildExcelTemplatePath_Always_ReturnsCorrectPathByUserLevel(byte userLevelId, string expectedPath)
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: userLevelId);

            // Act
            var actualPath = winLossDetailExcelHandler.GetTemplateName(message.UserLevel);

            // Assert
            Assert.Equal(expectedPath, actualPath);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossQuery_CallMapExcelTemplatePath(byte userLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportExcelQuery(userLevel: userLevelId);

            // Act
            winLossDetailExcelHandler.Handle(message);

            // Assert
            winLossDetailExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        private WinLossDetailExcelQuery CreateExportExcelQuery(byte userLevel)
        {
            var result = new WinLossDetailExcelQuery(user);
            result.UserLevelId = userLevel;
            result.FromDate = DefaultVariables.CurrentDate;
            result.ToDate = DefaultVariables.CurrentDate;

            return result;
        }
    }
}