namespace Alpha.Reports.Site.Tests.Handlers
{
    using Alpha.Reports.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.Reports.Site.Requests.RiskAnalysisIPMatching;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utils;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("UserCollection")]

    public class RiskAnalysisIPMatchingExcelHandlerTests
    {
        private readonly RiskAnalysisIPMatchingService reportService;
        private readonly Workbook<RiskAnalysisIPMatchingModel> workbook;
        private readonly RiskAnalysisIPMatchingExcelHandler excelHandler;
        private readonly UserFixture userFixture;

        public RiskAnalysisIPMatchingExcelHandlerTests(UserFixture userFixture)
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            reportService = Substitute.For<RiskAnalysisIPMatchingService>();
            workbook = Substitute.For<Workbook<RiskAnalysisIPMatchingModel>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<RiskAnalysisIPMatchingModel>>(), Arg.Any<Mapper<RiskAnalysisIPMatchingModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            excelHandler = Substitute.ForPartsOf<RiskAnalysisIPMatchingExcelHandler>(reportService, workbook);
            excelHandler.MapPath = Substitute.For<Func<string, string>>();
            excelHandler.IsFileExist = (path) => true;
            this.userFixture = userFixture;
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_NullMessage_ThrowsException()
        {
            // Arrange
            RiskAnalysisIPMatchingExcelQuery nullMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => excelHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_UsesCorrectTemplateInfo()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(
                tp => tp.NoInfomationText == "No information is available"));
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_ExportsCorrectBody()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);
            var enteredResultReportData = Enumerable.Empty<RiskAnalysisIPMatchingModel>();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<RiskAnalysisIPMatchingModel>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(enteredResultReportData)),
                Arg.Any<Mapper<RiskAnalysisIPMatchingModel>>());
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_ExportsCorrectSheetName()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_ExportsExcelToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void HandleExportRiskAnalysisIPMatchingExcelQuery_CallMapExcelTemplatePath()
        {
            // Arrange
            var message = CreateExportExcelQuery(custLevelId: 4);

            // Act
            excelHandler.Handle(message);

            // Assert
            excelHandler.Received().GetExcelTemplatePath("Super");
        }

        private RiskAnalysisIPMatchingExcelQuery CreateExportExcelQuery(byte custLevelId)
        {
            var query = new RiskAnalysisIPMatchingExcelQuery(userFixture.User);
            query.CustLevelId = custLevelId;

            return query;
        }
    }
}