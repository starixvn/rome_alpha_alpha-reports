namespace Alpha.Reports.Site.Tests.Handlers.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using RemotingRef;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Services;
    using Site.Handlers.WinLossByProduct;
    using Site.Requests.WinLossByProduct;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductExcelHandlerTest
    {
        private readonly WinLossByProductService reportService;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;

        private readonly WinLossByProductWorkbook winLossByProductWorkbook;
        private readonly Workbook<WinLossByBetTypeModel> onebookWorkbook;
        private readonly WinLossByProductExcelHandler winLossByProductExcelHandler;

        private readonly CacheRefService cacheRefService;
        private readonly UserFixture userFixture;

        public WinLossByProductExcelHandlerTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            reportService = Substitute.For<WinLossByProductService>();
            reportDateService = Substitute.For<ReportDateService>();
            cacheRefService = Substitute.For<CacheRefService>();
            productService = Substitute.For<ProductService>();
            productService.ConcatProductNames(Arg.Any<string>()).Returns("Product Name");

            winLossByProductWorkbook = Substitute.For<WinLossByProductWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<WinLossByBetTypeModel>>();

            winLossByProductWorkbook.SetData(
                Arg.Any<IEnumerable<WinLossByProductModel>>(),
                Arg.Any<WinLossByBetTypeModel>())
                .Returns(winLossByProductWorkbook);

            winLossByProductWorkbook.SetRowMappers(
                Arg.Any<Mapper<WinLossByBetTypeModel>>(),
                Arg.Any<Mapper<WinLossByBetTypeModel>>())
                .Returns(winLossByProductWorkbook);

            winLossByProductWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Footer(Arg.Any<IEnumerable<WinLossByBetTypeModel>>(), Arg.Any<Mapper<WinLossByBetTypeModel>>()).Returns(onebookWorkbook);

            winLossByProductExcelHandler = Substitute.ForPartsOf<WinLossByProductExcelHandler>(reportService, productService, cacheRefService, winLossByProductWorkbook);
            winLossByProductExcelHandler.MapPath = (path) => path;
            winLossByProductExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            winLossByProductExcelHandler.MapPath = (path) => path;
            winLossByProductExcelHandler.IsFileExist = (path) => true;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByProductExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_NotNullMessage_SetsDataToWorkbook()
        {
            // Act
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);
            winLossByProductExcelHandler.Handle(message);

            // Assert
            winLossByProductWorkbook.Received().SetData(
                Arg.Any<IEnumerable<WinLossByProductModel>>(),
                Arg.Any<WinLossByBetTypeModel>());
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_NotNullMessage_SetRowMappers()
        {
            // Act
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);
            winLossByProductExcelHandler.Handle(message);

            // Assert
            winLossByProductWorkbook.Received().SetRowMappers(
                Arg.Any<Mapper<WinLossByBetTypeModel>>(),
                Arg.Any<Mapper<WinLossByBetTypeModel>>());
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_MasterWinLoss_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);

            // Act
            winLossByProductExcelHandler.Handle(message);

            // Assert
            winLossByProductWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_MasterWinLoss_ExportsCorrectTitle()
        {
            // Arrange
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);

            // Act
            winLossByProductExcelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Title(Arg.Is("Win Loss By Product 12/02/2016 --> 12/02/2016 of Product Name"));
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_MasterWinLoss_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);

            // Act
            winLossByProductExcelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Header(Arg.Is<Func<string, string>>(
                func => func("BetTypeName") == "Product/Bet Type" && func("WinLoss") == "Win/Loss"));
        }

        [Fact]
        public void HandleExportWinLossByProductQuery_MasterWinLoss_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportWinlossByProductExcelQuery(custLevelId: 4);

            // Act
            winLossByProductExcelHandler.Handle(message);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossByProductQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportWinlossByProductExcelQuery(custLevelId);

            // Act
            winLossByProductExcelHandler.Handle(message);

            // Assert
            winLossByProductExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithCorrectBetTypeName()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, 4, 5, 6, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossByProductExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.Equal("BetTypeName", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Fact]
        public void GetRowMapper_TotalRow_MapColumnAWithTotalLabel()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, 4, 5, 6, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossByProductExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, true);

            // Assert
            Assert.Equal("Total", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRowMapper_HasBetCount_ReturnCorrectMapper(byte roleId)
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, 4, 5, 6, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(roleId);
            var message = new WinLossByProductExcelQuery(user);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (int)actualMapper.Mappings[Column.B](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 6 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 5);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRowMapper_NotHaveBetCount_ReturnCorrectMapper(byte roleId)
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, null, 5, 6, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(roleId);
            var message = new WinLossByProductExcelQuery(user);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.B](viewModel) == "--" &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 6 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 5);
        }

        [Fact]
        public void GetRowMapper_AgentLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, null, 5, 6, netWinLoss, netWinLoss, null, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossByProductExcelQuery(user);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.J](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.K](viewModel) == -6);
        }

        [Fact]
        public void GetRowMapper_MasterLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, null, 5, 6, netWinLoss, netWinLoss, netWinLoss, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)3);
            var message = new WinLossByProductExcelQuery(user);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == -9);
        }

        [Fact]
        public void GetRowMapper_SuperLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, null, 5, 6, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var message = new WinLossByProductExcelQuery(user);

            // Act
            var actualMapper = winLossByProductExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == -12);
        }

        private WinLossByProductExcelQuery CreateExportWinlossByProductExcelQuery(byte custLevelId)
        {
            var exportWinLossByProductQuery = new WinLossByProductExcelQuery(userFixture.User);
            exportWinLossByProductQuery.CustLevelId = custLevelId;
            exportWinLossByProductQuery.UserSelectedProductIds = "1,2";
            exportWinLossByProductQuery.FromDate = DefaultVariablesTest.CurrentDate;
            exportWinLossByProductQuery.ToDate = DefaultVariablesTest.CurrentDate;

            return exportWinLossByProductQuery;
        }
    }
}