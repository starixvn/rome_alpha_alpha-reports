namespace Alpha.Reports.Site.Tests.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Reports.Site.ViewModels.Components;
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
    using Utils;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossExcelHandlerTest
    {
        private readonly Workbook<WinLossModel> winlossWorkbook;
        private readonly WinLossExcelHandler winLossExcelHandler;
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossExcelHandlerTest(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            var productService = Substitute.For<ProductService>();
            productService
                .ConcatProductNames(Arg.Any<IEnumerable<int>>(), Arg.Any<IDictionary<int, string>>())
                .Returns("Product Name");
            var reportService = Substitute.For<WinLossService>();

            var productDetailService = Substitute.For<ProductDetailService>();
            productDetailService
                .GetProductNamesExportExcel(Arg.Any<IEnumerable<int>>())
                .Returns(new Dictionary<int, string> { [42] = "Saba.club" });

            winlossWorkbook = Substitute.For<Workbook<WinLossModel>>();
            winlossWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(winlossWorkbook);
            winlossWorkbook.Title(Arg.Any<string>()).Returns(winlossWorkbook);
            winlossWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(winlossWorkbook);
            winlossWorkbook.Body(Arg.Any<IEnumerable<WinLossModel>>(), Arg.Any<Mapper<WinLossModel>>()).Returns(winlossWorkbook);
            winlossWorkbook.Footer(Arg.Any<WinLossModel>(), Arg.Any<Mapper<WinLossModel>>()).Returns(winlossWorkbook);
            winlossWorkbook.Sheet(Arg.Any<string>()).Returns(winlossWorkbook);

            winLossExcelHandler = Substitute.ForPartsOf<WinLossExcelHandler>(productService, productDetailService, reportService, winlossWorkbook);
            winLossExcelHandler.MapPath = (path) => path;
            winLossExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

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
            winlossWorkbook.Received().Title(Arg.Is("Win Loss 12/02/2016 --> 12/02/2016 of Product Name"));
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
                fixture.Create<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>(),
                null);

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
                fixture.Create<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>(),
                null);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winlossWorkbook.Received().Footer(
                Arg.Is<WinLossModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(winlossViewModel.GrandTotal)),
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
        public void HandleExportWinLossQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportWinlossExcelQuery(custLevelId);

            // Act
            winLossExcelHandler.Handle(message);

            // Assert
            winLossExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithCustName()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossModel(1, "CustName", 2, 3, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](viewModel) == "CustName");
        }

        [Fact]
        public void GetRowMapper_TotalRow_MapColumnAWithCustName()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossModel(1, "CustName", 2, 3, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new WinLossExcelQuery(userFixture.User);

            // Act
            var actualMapper = winLossExcelHandler.GetRowMapper(message, true);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](viewModel) == "Total");
        }

        [Fact]
        public void GetRowMapper_AgentLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new NetWinLoss(1, 2);
            var viewModel = new WinLossModel(1, "CustName", 2, 3, netWinLoss, netWinLoss, null, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new WinLossExcelQuery(user);

            // Act
            var actualMapper = winLossExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.B](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.J](viewModel) == -6);
        }

        [Fact]
        public void GetRowMapper_MasterLevel_ReturnCorrectMapper()
        {
            // Arrange
            var viewModel = new WinLossModel(
                1,
                "CustName",
                2,
                3,
                new NetWinLoss(4, 5),
                new NetWinLoss(6, 7),
                new NetWinLoss(8, 9),
                null);

            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)3);
            var message = new WinLossExcelQuery(user);

            // Act
            var actualMapper = winLossExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.B](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 13 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 8 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 9 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 17 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == -39);
        }

        [Fact]
        public void GetRowMapper_SuperLevel_ReturnCorrectMapper()
        {
            // Arrange
            var viewModel = new WinLossModel(
                1,
                "CustName",
                2,
                3,
                new NetWinLoss(4, 5),
                new NetWinLoss(6, 7),
                new NetWinLoss(8, 9),
                new NetWinLoss(10, 11));

            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var message = new WinLossExcelQuery(user);

            // Act
            var actualMapper = winLossExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.B](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 17 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 10 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 11 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 21 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == -60);
        }

        private WinLossExcelQuery CreateExportWinlossExcelQuery(byte custLevelId)
        {
            var exportWinLossQuery = new WinLossExcelQuery(userFixture.User);
            exportWinLossQuery.CustLevelId = custLevelId;
            exportWinLossQuery.FromDate = DefaultVariablesTest.CurrentDate;
            exportWinLossQuery.ToDate = DefaultVariablesTest.CurrentDate;

            return exportWinLossQuery;
        }
    }
}