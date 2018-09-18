namespace Alpha.Reports.Site.Tests.Handlers.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Services;
    using Site.Handlers.PokerGamesWinLossDetail;
    using Site.Requests.Consolidation;
    using Site.Requests.PokerGamesWinLossDetail;
    using Utils;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    [Collection("UserCollection")]
    public class PokerGamesWinLossDetailExcelHandlerTests
    {
        private readonly Workbook<PokerGamesWinLossDetailModel> workbook;
        private readonly PokerGamesWinLossDetailExcelHandler pokerGamesWinLossDetailExcelHandler;
        private readonly ReportData<PokerGamesWinLossDetailModel> reportData;
        private readonly UserFixture userFixture;

        public PokerGamesWinLossDetailExcelHandlerTests(UserFixture userFixture)
        {
            reportData = new PagedReportData<PokerGamesWinLossDetailModel>(Enumerable.Empty<PokerGamesWinLossDetailModel>(), 0, 0, 0, default(PokerGamesWinLossDetailModel));
            var reportService = Substitute.For<PokerGamesWinLossDetailService>();
            reportService.GetWinLossDetail(Arg.Any<PokerGamesWinLossDetailParam>()).Returns(reportData);

            workbook = Substitute.For<Workbook<PokerGamesWinLossDetailModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<PokerGamesWinLossDetailModel>>(), Arg.Any<Mapper<PokerGamesWinLossDetailModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<PokerGamesWinLossDetailModel>(), Arg.Any<Mapper<PokerGamesWinLossDetailModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            this.userFixture = userFixture;
            pokerGamesWinLossDetailExcelHandler = Substitute.ForPartsOf<PokerGamesWinLossDetailExcelHandler>(reportService, workbook);
            pokerGamesWinLossDetailExcelHandler.MapPath = (path) => path;
            pokerGamesWinLossDetailExcelHandler.IsFileExist = (path) => true;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            PokerGamesWinLossDetailExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => pokerGamesWinLossDetailExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

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
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Is("Poker Games-Win Loss 12/02/2016 --> 12/02/2016"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is(reportData.DataSource),
                Arg.Any<Mapper<PokerGamesWinLossDetailModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectFooter()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Footer(Arg.Is(reportData.TotalData), Arg.Any<Mapper<PokerGamesWinLossDetailModel>>());
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MasterWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_AgentWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 3);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportWinLossDetailQuery_MemberWinLossDetail_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 2);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void HandleExportWinLossQuery_CallMapExcelTemplatePath(byte custLevelId, string fileName)
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: custLevelId);

            // Act
            pokerGamesWinLossDetailExcelHandler.Handle(message);

            // Assert
            pokerGamesWinLossDetailExcelHandler.Received().GetExcelTemplatePath(fileName);
        }

        [Fact]
        public void GetRowMapper_DataRow_MapColumnAWithCustName()
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new PokerGamesWinLossDetailExcelQuery(userFixture.User);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.Equal("CustName", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Fact]
        public void GetRowMapper_TotalRow_MapColumnAWithTotalLabel()
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var message = new PokerGamesWinLossDetailExcelQuery(userFixture.User);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, true);

            // Assert
            Assert.Equal("Total", (string)actualMapper.Mappings[Column.A](viewModel));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetRowMapper_Always_ReturnCorrectMapper(byte roleId)
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(roleId);
            var message = new PokerGamesWinLossDetailExcelQuery(user);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (int)actualMapper.Mappings[Column.B](viewModel) == 5 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 1 &&
                (decimal)actualMapper.Mappings[Column.G](viewModel) == 0 &&
                (decimal)actualMapper.Mappings[Column.H](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.I](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.J](viewModel) == 0 &&
                (decimal)actualMapper.Mappings[Column.K](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.L](viewModel) == 3);
        }

        [Fact]
        public void GetRowMapper_AgentLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, null, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);
            var message = new PokerGamesWinLossDetailExcelQuery(user);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.Equal(-6, (decimal)actualMapper.Mappings[Column.M](viewModel));
        }

        [Fact]
        public void GetRowMapper_MasterLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, netWinLoss, null);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)3);
            var message = new PokerGamesWinLossDetailExcelQuery(user);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.M](viewModel) == 0 &&
                (decimal)actualMapper.Mappings[Column.N](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.O](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.P](viewModel) == -9);
        }

        [Fact]
        public void GetRowMapper_SuperLevel_ReturnCorrectMapper()
        {
            // Arrange
            var netWinLoss = new PokerGamesWinLoss(1, 0, 2);
            var viewModel = new PokerGamesWinLossDetailModel(1, "CustName", 2, 3, 4, 5, netWinLoss, netWinLoss, netWinLoss, netWinLoss);
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
            var message = new PokerGamesWinLossDetailExcelQuery(user);

            // Act
            var actualMapper = pokerGamesWinLossDetailExcelHandler.GetRowMapper(message, false);

            // Assert
            Assert.True(
                (decimal)actualMapper.Mappings[Column.M](viewModel) == 0 &&
                (decimal)actualMapper.Mappings[Column.N](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.O](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.P](viewModel) == 0 &&
                (decimal)actualMapper.Mappings[Column.Q](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.R](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.S](viewModel) == -12);
        }

        private PokerGamesWinLossDetailExcelQuery CreateExportExcelQuery(byte custLevelId)
        {
            var result = new PokerGamesWinLossDetailExcelQuery(userFixture.User);
            result.CustLevelId = custLevelId;
            result.FromDate = DefaultVariablesTest.CurrentDate;
            result.ToDate = DefaultVariablesTest.CurrentDate;

            return result;
        }
    }
}