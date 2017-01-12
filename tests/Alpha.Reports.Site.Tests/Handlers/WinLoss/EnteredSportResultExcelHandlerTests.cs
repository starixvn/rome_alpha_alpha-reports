namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Services;
    using Site.Handlers.WinLoss;
    using Site.Requests.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredSportResultExcelHandlerTests
    {
        private readonly WinLossService reportService;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;
        private readonly EnteredSportResultExcelHandler enteredSportResultExcelHandler;
        private readonly IUser user;

        public EnteredSportResultExcelHandlerTests()
        {
            reportService = Substitute.For<WinLossService>();
            enteredSportResultWorkbook = Substitute.For<Workbook<EnteredSportResult>>();
            enteredSportResultWorkbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Body(Arg.Any<IEnumerable<EnteredSportResult>>(), Arg.Any<Mapper<EnteredSportResult>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Sheet(Arg.Any<string>()).Returns(enteredSportResultWorkbook);

            enteredSportResultExcelHandler = Substitute.ForPartsOf<EnteredSportResultExcelHandler>(reportService, enteredSportResultWorkbook);
            enteredSportResultExcelHandler.MapPath = Substitute.For<Func<string, string>>();
            enteredSportResultExcelHandler.IsFileExist = (path) => true;
            user = Substitute.For<IUser>();
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

        private EnteredSportResultExcelQuery CreateExportEnteredResultsExcelQuery(byte custLevelId)
        {
            var exportEnteredResultQuery = new EnteredSportResultExcelQuery(user);
            exportEnteredResultQuery.CustLevelId = custLevelId;

            return exportEnteredResultQuery;
        }
    }
}