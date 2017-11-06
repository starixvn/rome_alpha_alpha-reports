namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.Statement;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class SMAStatementExcelHandlerTests
    {
        private readonly StatementWorkbook workbook;
        private readonly Workbook<StatementModel> onebookWorkbook;

        private readonly SMAStatementExcelHandler handler;
        private readonly SMAStatementExcelQuery query;
        private readonly IEnumerable<StatementModel> data;

        public SMAStatementExcelHandlerTests(UserFixture userFixture)
        {
            var user = userFixture.User;
            var anyValueCreatorFixture = new AnyValueCreatorFixture();

            var reportService = Substitute.For<StatementService>();

            data = new List<StatementModel>
            {
                anyValueCreatorFixture.Create<StatementModel>()
            };

            reportService.GetStatements(Arg.Any<GetStatementParam>()).Returns(data);

            workbook = Substitute.For<StatementWorkbook>();
            onebookWorkbook = Substitute.For<Workbook<StatementModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(onebookWorkbook);
            onebookWorkbook.Title(Arg.Any<string>()).Returns(onebookWorkbook);
            onebookWorkbook.Header(Arg.Any<Func<string, string>>()).Returns(onebookWorkbook);
            onebookWorkbook.Body(Arg.Any<IEnumerable<StatementModel>>(), Arg.Any<Mapper<StatementModel>>()).Returns(onebookWorkbook);
            onebookWorkbook.Sheet(Arg.Any<string>()).Returns(onebookWorkbook);

            handler = new SMAStatementExcelHandler(reportService, workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            query = new SMAStatementExcelQuery(user)
            {
                FromDate = SystemTime.Today,
                ToDate = SystemTime.Today
            };
        }

        [Fact]
        public void HandleSMAStatementExcelQuery_Always_UsesCorrectTemplate()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().UseTemplate(Arg.Is<TemplateInfo>(tp =>
                tp.NoInfomationText == "No information is available"
                && tp.TitleStartIndex == 0
                && tp.HeaderStartIndex == 1
                && tp.BodyStartIndex == 2
                && tp.BodyEndIndex == 3));
        }

        [Fact]
        public void HandleSMAStatementExcelQuery_Always_ExportsCorrectTitle()
        {
            // Act
            handler.Handle(query);

            // Assert
            onebookWorkbook.Received().Title(Arg.Is("Statement (Name) 12/02/2016 --> 12/02/2016"));
        }

        [Fact]
        public void HandleSMAStatement_Always_ExportsCorrectHeader()
        {
            // Act
            handler.Handle(query);

            // Assert
            onebookWorkbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleSMAStatement_Always_ExportsCorrectBody()
        {
            // Act
            handler.Handle(query);

            // Assert
            onebookWorkbook.Received().Body(data, null);
        }

        [Fact]
        public void HandleSMAStatement_Always_ExportsCorrectSheetName()
        {
            // Act
            handler.Handle(query);

            // Assert
            onebookWorkbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleSMAStatement_Always_ExportsExcelToBytes()
        {
            // Act
            handler.Handle(query);

            // Assert
            onebookWorkbook.Received().ExportToBytes();
        }
    }
}
