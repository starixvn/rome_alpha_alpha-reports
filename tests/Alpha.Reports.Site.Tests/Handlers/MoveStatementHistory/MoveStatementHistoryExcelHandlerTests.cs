namespace Alpha.Reports.Site.Tests.Handlers.MoveStatementHistory
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Reports.Site.Features.BetCreditTracking;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("UserCollection")]
    public class MoveStatementHistoryExcelHandlerTests
    {
        private readonly MoveStatementHistoryExcelHandler handler;
        private readonly MoveStatementHistoryService reportService;
        private readonly MoveStatementHistoryExcelQuery excelQuery;
        private readonly MoveStatementHistoryWorkbook workbook;
        private readonly Workbook<MoveStatementHistoryByRefNo> onebookWorkbook;

        public MoveStatementHistoryExcelHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<MoveStatementHistoryService>();

            workbook = Substitute.For<MoveStatementHistoryWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<MoveStatementHistoryByRefNo>>();

            workbook.SetData(Arg.Any<IEnumerable<MoveStatementHistoryByMember>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MoveStatementHistoryByRefNo>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNo>>()).Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Body(Arg.Any<IEnumerable<MoveStatementHistoryByRefNo>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNo>>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);

            var refNoRow = new List<MoveStatementHistoryByRefNo>
            {
                new MoveStatementHistoryByRefNo(1, "member1", 2, DateTime.Now.AddDays(-1), DateTime.Now, 10, 10, 10, true),
                new MoveStatementHistoryByRefNo(1, "member1", 2, DateTime.Now.AddDays(-2), DateTime.Now, 10, 10, 10, true)
            };

            var memberMoveStatementHistoryByRefNo = new MoveStatementHistoryByRefNo
                (1, "member1", 2, DateTime.Now.AddDays(-1), DateTime.Now, 20, 20, 20, true);

            var rawData = new List<MoveStatementHistoryByMember>
            {
                new MoveStatementHistoryByMember( "member1", memberMoveStatementHistoryByRefNo, refNoRow)
            };

            var summary = new MoveStatementHistorySummary(1, 20, 20, 20, 0);
            var resultModel = new MoveStatementHistoryResult(0, "Success", rawData, summary, 2);
            reportService.GetMoveStatementHistory(Arg.Any<MoveStatementHistoryParam>()).Returns(resultModel);
            handler = Substitute.ForPartsOf<MoveStatementHistoryExcelHandler>(reportService, workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            excelQuery = new MoveStatementHistoryExcelQuery(userFixture.User);
        }

        [Fact]
        public async Task Handle_NotNullMessage_SetsRowMappers()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<MoveStatementHistoryByRefNo>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNo>>());
        }

        [Fact]
        public async Task Handle_NotNullMessage_SetsDataToWorkbook()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<MoveStatementHistoryByMember>>());
        }

        [Fact]
        public async Task Handle_NotNullMessage_UsesCorrectTemplate()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public async Task Handle_NotNullMessage_ExportsCorrectSheetName()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public async Task Handle_NotNullMessage_ExportsExcelToBytes()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public async Task Handle_NotNullMessage_ExportsCorrectTitle()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Title(Arg.Is("Move Statement History 12/02/2016 -->  12/02/2016"));
        }

        [Fact]
        public async Task Handle_NotNullMessage_ExportsCorrectHeader()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public async Task Handle_NotNullMessage_CallMapExcelTemplatePath()
        {
            // Act
            await handler.Handle(excelQuery);

            // Assert
            handler.Received().GetExcelTemplatePath("MoveStatementHistory");
        }

        [Fact]
        public void GetMemberMapper_Always_MapCorrectly()
        {
            // Arrange
            var model = new MoveStatementHistoryByRefNo(1, "member1", 2, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), 10, 10, 10, true);

            // Act
            var actualMapper = handler.GetRowMapper(true);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](model) == "member1");
        }

        [Fact]
        public void GetRefNoMapper_Always_MapCorrectly()
        {
            // Arrange
            var model = new MoveStatementHistoryByRefNo(1, "member1", 2, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), 10, 10, 10, true);

            // Act
            var actualMapper = handler.GetRowMapper(false);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](model) == "2" &&
                (string)actualMapper.Mappings[Column.B](model) == "01/01/2022" &&
                (string)actualMapper.Mappings[Column.C](model) == "01/02/2022" &&
                (decimal)actualMapper.Mappings[Column.D](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.E](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.F](model) == 10
                );
        }
    }
}
