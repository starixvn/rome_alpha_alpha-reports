namespace Alpha.Reports.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Enumerations;
    using Extensions;
    using Models;
    using Models.Winloss;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Reports.Extensions;
    using Reports.Tests.TestCases;
    using Services;
    using Site.Handlers;
    using Site.Handlers.WinLoss;
    using Site.Helpers;
    using Site.Requests.WinLoss;
    using Site.ViewModels;
    using Site.ViewModels.WinLoss;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredSportResultExcelHandlerTests
    {
        private readonly WinLossService reportService;
        private readonly Workbook<EnteredSportResult> enteredSportResultWorkbook;
        private readonly EnteredSportResultExcelHandler enteredSportResultExcelHandler;
        private readonly IUserSession userSession;

        public EnteredSportResultExcelHandlerTests()
        {
            reportService = Substitute.For<WinLossService>();
            enteredSportResultWorkbook = Substitute.For<Workbook<EnteredSportResult>>();
            enteredSportResultWorkbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Body(Arg.Any<IEnumerable<EnteredSportResult>>(), Arg.Any<Mapper<EnteredSportResult>>()).Returns(enteredSportResultWorkbook);
            enteredSportResultWorkbook.Sheet(Arg.Any<string>()).Returns(enteredSportResultWorkbook);

            enteredSportResultExcelHandler = new EnteredSportResultExcelHandler(reportService, enteredSportResultWorkbook);
            userSession = Substitute.For<IUserSession>();
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
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().UseTemplate(Arg.Is<ONEbook.SpreadSheets.TemplateInfo>(
                tp => tp.TemplatePath == "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls" &&
                tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);
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
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().Sheet(Arg.Is<string>("Master Win Loss"));
        }

        [Fact]
        public void HandleExportEnteredResultQuery_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportEnteredResultsExcelQuery(userLevel: 4);

            // Act
            enteredSportResultExcelHandler.Handle(message);

            // Assert
            enteredSportResultWorkbook.Received().ExportToBytes();
        }

        private EnteredSportResultExcelQuery CreateExportEnteredResultsExcelQuery(byte userLevel)
        {
            var exportEnteredResultQuery = new EnteredSportResultExcelQuery(userSession);
            exportEnteredResultQuery.UserLevelId = userLevel;

            return exportEnteredResultQuery;
        }
    }
}