namespace Alpha.Reports.Site.Tests.Handlers.MoveStatementHistory
{
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    [Collection("UserCollection")]
    public class MoveStatementHistoryExcelHandlerTests
    {
        private readonly MoveStatementHistoryExcelHandler handler;
        private readonly MoveStatementHistoryService reportService;
        private readonly MoveStatementHistoryExcelQuery excelQuery;
        private readonly MoveStatementHistoryWorkbook workbook;
        private readonly Workbook<MoveStatementHistoryByRefNoModel> onebookWorkbook;

        public MoveStatementHistoryExcelHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<MoveStatementHistoryService>();

            workbook = Substitute.For<MoveStatementHistoryWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<MoveStatementHistoryByRefNoModel>>();

            workbook.SetData(Arg.Any<IEnumerable<MoveStatementHistoryByMemberModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<MoveStatementHistoryByRefNoModel>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNoModel>>()).Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Body(Arg.Any<IEnumerable<MoveStatementHistoryByRefNoModel>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNoModel>>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);

            var refNoRow = new List<MoveStatementHistoryByRefNoModel>
            {
                new MoveStatementHistoryByRefNoModel(1, "member1", 2, DateTime.Now.AddDays(-1), DateTime.Now, 10, 10, 10, 1),
                new MoveStatementHistoryByRefNoModel(1, "member1", 2, DateTime.Now.AddDays(-2), DateTime.Now, 10, 10, 10, 1)
            };

            var rawData = new List<MoveStatementHistoryByMemberModel>
            {
                new MoveStatementHistoryByMemberModel(1, "member1", refNoRow)
            };
            var resultModel = new MoveStatementHistoryResultModel(0, "Success", rawData);
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
        public void Handle_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MoveStatementHistoryExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_NotNullMessage_SetsRowMappers()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<MoveStatementHistoryByRefNoModel>>(), Arg.Any<Mapper<MoveStatementHistoryByRefNoModel>>());
        }

        [Fact]
        public void Handle_NotNullMessage_SetsDataToWorkbook()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<MoveStatementHistoryByMemberModel>>());
        }

        [Fact]
        public void Handle_NotNullMessage_UsesCorrectTemplate()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectSheetName()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsExcelToBytes()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectTitle()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Title(Arg.Is("Move Statement History 12/02/2016 -->  12/02/2016"));
        }

        [Fact]
        public void Handle_NotNullMessage_ExportsCorrectHeader()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void Handle_NotNullMessage_CallMapExcelTemplatePath()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            handler.Received().GetExcelTemplatePath("MoveStatementHistory");
        }

        [Fact]
        public void GetMemberMapper_Always_MapCorrectly()
        {
            // Arrange
            var model = new MoveStatementHistoryByRefNoModel(1, "member1", 2, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), 10, 10, 10, 1);

            // Act
            var actualMapper = handler.GetRowMapper(true);

            // Assert
            Assert.True((string)actualMapper.Mappings[Column.A](model) == "member1");
        }

        [Fact]
        public void GetRefNoMapper_Always_MapCorrectly()
        {
            // Arrange
            var model = new MoveStatementHistoryByRefNoModel(1, "member1", 2, new DateTime(2022, 01, 01), new DateTime(2022, 01, 02), 10, 10, 10, 1);

            // Act
            var actualMapper = handler.GetRowMapper(false);

            // Assert
            Assert.True(
                (long)actualMapper.Mappings[Column.A](model) == 2 &&
                (string)actualMapper.Mappings[Column.B](model) == "01/01/2022" &&
                (string)actualMapper.Mappings[Column.C](model) == "01/02/2022" &&
                (decimal)actualMapper.Mappings[Column.D](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.E](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.F](model) == 10 
                );
        }
    }
}
