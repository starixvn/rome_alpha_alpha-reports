namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]

    public class EnteredSportResultExcelHandlerTests
    {
        private readonly WinLossService reportService;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;
        private readonly EnteredSportResultExcelHandler enteredSportResultExcelHandler;
        private readonly UserFixture userFixture;

        public EnteredSportResultExcelHandlerTests(UserFixture userFixture)
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            reportService = Substitute.For<WinLossService>();
            enteredSportResultWorkbook = Substitute.For<Workbook<EnteredSportResult>>();
            enteredSportResultWorkbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Body(Arg.Any<IEnumerable<EnteredSportResult>>(), Arg.Any<Mapper<EnteredSportResult>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Sheet(Arg.Any<string>()).Returns(enteredSportResultWorkbook);

            enteredSportResultExcelHandler = Substitute.ForPartsOf<EnteredSportResultExcelHandler>(reportService, enteredSportResultWorkbook);
            enteredSportResultExcelHandler.MapPath = Substitute.For<Func<string, string>>();
            enteredSportResultExcelHandler.IsFileExist = (path) => true;
            this.userFixture = userFixture;
        }

        [Fact]
        public void HandleExportEnteredResultQuery_NullMessage_ThrowsException()
        {
            // Arrange
            EnteredSportResultExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => enteredSportResultExcelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);
            var enteredResultReportData = Enumerable.Empty<EnteredSportResult>();

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Body(
                Arg.Is<IEnumerable<EnteredSportResult>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(enteredResultReportData)),
                Arg.Any<Mapper<EnteredSportResult>>());
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectHeader()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Header(Arg.Is<Func<string, string>>(func =>
            func("Yesterday") == "12/01/2016"
            && func("Today") == "12/02/2016"
            && func("WinLoss") == "Win/Loss"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportEnteredResultQuery_CallMapExcelTemplatePath()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(custLevelId: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultExcelHandler.Received().GetExcelTemplatePath("MatchResultEntered");
        }

        [Fact]
        public void GetRowMapper_HasEnteredSportResult_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new EnteredSportResult(
                1,
                "SportName",
                new DailyEnteredSport(true, new DateTime(2017, 10, 9), 2, 3, 4),
                new DailyEnteredSport(false, new DateTime(2017, 10, 8), 5, 6, 7));

            // Act
            var actualMapper = enteredSportResultExcelHandler.GetEnteredSportResultRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "SportName" &&
                (int)actualMapper.Mappings[Column.B](viewModel) == 6 &&
                (int)actualMapper.Mappings[Column.C](viewModel) == 7 &&
                (int)actualMapper.Mappings[Column.D](viewModel) == 3 &&
                (int)actualMapper.Mappings[Column.E](viewModel) == 4);
        }

        [Fact]
        public void GetRowMapper_NotHasEnteredSportResult_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new EnteredSportResult(1, "SportName", null, null);

            // Act
            var actualMapper = enteredSportResultExcelHandler.GetEnteredSportResultRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "SportName" &&
                (int)actualMapper.Mappings[Column.B](viewModel) == 0 &&
                (int)actualMapper.Mappings[Column.C](viewModel) == 0 &&
                (int)actualMapper.Mappings[Column.D](viewModel) == 0 &&
                (int)actualMapper.Mappings[Column.E](viewModel) == 0);
        }

        private EnteredSportResultExcelQuery CreateExportEnteredResultsExcelQuery(byte custLevelId)
        {
            var exportEnteredResultQuery = new EnteredSportResultExcelQuery(userFixture.User);
            exportEnteredResultQuery.CustLevelId = custLevelId;

            return exportEnteredResultQuery;
        }
    }
}