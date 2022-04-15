namespace Alpha.Reports.Site.Tests.Handlers.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
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

    [Trait("Category", "BetCreditHistory")]
    [Collection("UserCollection")]
    public class BetCreditHistoryExcelHandlerTests
    {
        private readonly BetCreditHistoryExcelHandler handler;
        private readonly BetCreditHistoryService reportService;
        private readonly BetCreditHistoryExcelQuery excelQuery;
        private readonly BetCreditHistoryWorkbook workbook;
        private readonly Workbook<BetCreditHistoryModel> onebookWorkbook;

        public BetCreditHistoryExcelHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<BetCreditHistoryService>();

            workbook = Substitute.For<BetCreditHistoryWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<BetCreditHistoryModel>>();
            
            workbook.SetData(Arg.Any<IEnumerable<BetCreditHistoryModel>>()).Returns(workbook);
            workbook.SetRowMappers(Arg.Any<Mapper<BetCreditHistoryModel>>()).Returns(workbook);
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(onebookWorkbook);

            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Body(Arg.Any<IEnumerable<BetCreditHistoryModel>>(), Arg.Any<Mapper<BetCreditHistoryModel>>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);

            var rawData = new List<BetCreditHistoryModel>
            {
                new BetCreditHistoryModel(DateTime.Now.AddDays(-1), DateTime.Now, 123456, 10, 10, 10, 10, 10, 10, 10, "Bet Updated"),
                new BetCreditHistoryModel(DateTime.Now.AddDays(-2), DateTime.Now, 234567, 20, 20, 20, 20, 20, 20, 20, "Bet Updated")
            };
            var resultModel = new BetCreditHistoryResultModel(0, "Success", rawData, 100);
            reportService.GetBetCreditHistory(Arg.Any<BetCreditHistoryParam>()).Returns(resultModel);
            handler = Substitute.ForPartsOf<BetCreditHistoryExcelHandler>(reportService, workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            excelQuery = new BetCreditHistoryExcelQuery(userFixture.User);
        }

        [Fact]
        public void Handle_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            BetCreditHistoryExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => handler.Handle(nullMessage));
        }

        [Fact]
        public void Handle_NotNullMessage_SetsRowMappers()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetRowMappers(Arg.Any<Mapper<BetCreditHistoryModel>>());
        }

        [Fact]
        public void Handle_NotNullMessage_SetsDataToWorkbook()
        {
            // Act
            handler.Handle(excelQuery);

            // Assert
            workbook.Received().SetData(Arg.Any<IEnumerable<BetCreditHistoryModel>>());
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
            onebookWorkbook.Received().Title(Arg.Is("Bet Credit History 12/02/2016 -->  12/02/2016 - Member: Name"));
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
            handler.Received().GetExcelTemplatePath("BetCreditHistory");
        }

        [Fact]
        public void GetRowMapper_Always_MapCorrectly()
        {
            // Arrange
            var model = new BetCreditHistoryModel(new DateTime(2022,01,01), new DateTime(2022,01,02), 123456, 10, 10, 10, 10, 10, 10, 10, "Bet Updated");

            // Act
            var actualMapper = handler.GetRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](model) == "01/01/2022 00:00:00" &&
                (string)actualMapper.Mappings[Column.B](model) == "01/02/2022" &&
                (long)actualMapper.Mappings[Column.C](model) == 123456 &&
                (decimal)actualMapper.Mappings[Column.D](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.E](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.F](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.G](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.H](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.I](model) == 10 &&
                (decimal)actualMapper.Mappings[Column.J](model) == 10 &&
                (string)actualMapper.Mappings[Column.K](model) == "Bet Updated"
                );
        }
    }
}