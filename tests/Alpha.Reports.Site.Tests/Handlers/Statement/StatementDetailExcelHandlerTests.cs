namespace Alpha.Reports.Site.Tests.Handlers.Statement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.Statement;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class StatementDetailExcelHandlerTests
    {
        private readonly Workbook<StatementDetailModel> workbook;
        private readonly IEnumerable<StatementDetailModel> data;
        private readonly StatementDetailExcelHandler handler;
        private readonly StatementDetailExcelQuery query;
        private readonly CompareLogic compareLogic;

        public StatementDetailExcelHandlerTests(UserFixture userFixture)
        {
            var user = userFixture.User;

            var reportService = Substitute.For<StatementService>();

            data = new List<StatementDetailModel>
            {
                new StatementDetailModel(DateTime.Now, string.Empty, string.Empty, null, string.Empty),
                new StatementDetailModel(DateTime.Now, string.Empty, string.Empty, 20, string.Empty)
            };

            reportService.StatementDetails(Arg.Any<StatementDetailsParam>()).Returns(data);

            workbook = Substitute.For<Workbook<StatementDetailModel>>();
            workbook.UseTemplate(Arg.Any<ONEbook.SpreadSheets.TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<StatementDetailModel>>(), Arg.Any<Mapper<StatementDetailModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<StatementDetailModel>(), Arg.Any<Mapper<StatementDetailModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            handler = new StatementDetailExcelHandler(reportService, workbook);
            handler.MapPath = (path) => path;
            handler.IsFileExist = (path) => true;

            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
            query = new StatementDetailExcelQuery(user)
            {
                TransferDate = SystemTime.Today
            };
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_UsesCorrectTemplate()
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
        public void HandleStatementDetailExcelQuery_Always_ExportsCorrectTitle()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().Title(Arg.Is("Transfer Details (Name): 12/02/2016"));
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_ExportsCorrectHeader()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_ExportsCorrectBody()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().Body(data, Arg.Any<Mapper<StatementDetailModel>>());
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_ExportsCorrectFooter()
        {
            // Arrange
            var total = new StatementDetailModel(DateTime.MinValue, string.Empty, string.Empty, 20, string.Empty, string.Empty);

            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().Footer(
                Arg.Is<StatementDetailModel>(data => compareLogic.Compare(data, total).AreEqual),
                Arg.Any<Mapper<StatementDetailModel>>());
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_ExportsCorrectSheetName()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().Sheet(Arg.Is("Sheet 1"));
        }

        [Fact]
        public void HandleStatementDetailExcelQuery_Always_ExportsExcelToBytes()
        {
            // Act
            handler.Handle(query);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void GetRowMapper_NoDataAmount_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new StatementDetailModel(new DateTime(2017, 10, 6), "TransType", "TransDesc", null, "Creator");

            // Act
            var actualMapper = handler.GetBodyRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Friday\r\n10/06/2017\r\n12:00:00 AM" &&
                (string)actualMapper.Mappings[Column.B](viewModel) == "TransType\r\nTransDesc" &&
                (string)actualMapper.Mappings[Column.C](viewModel) == "--");
        }

        [Fact]
        public void GetRowMapper_Always_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new StatementDetailModel(new DateTime(2017, 10, 6), "TransType", "TransDesc", 1, "Creator");

            // Act
            var actualMapper = handler.GetBodyRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Friday\r\n10/06/2017\r\n12:00:00 AM" &&
                (string)actualMapper.Mappings[Column.B](viewModel) == "TransType\r\nTransDesc" &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 1);
        }

        [Fact]
        public void GetFooterRowMapper_NoDataAmount_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new StatementDetailModel(new DateTime(2017, 10, 6), "TransType", "TransDesc", null, "Creator");

            // Act
            var actualMapper = handler.GetFooterRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Total" &&
                (string)actualMapper.Mappings[Column.C](viewModel) == "--");
        }

        [Fact]
        public void GetFooterRowMapper_Always_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new StatementDetailModel(new DateTime(2017, 10, 6), "TransType", "TransDesc", 1, "Creator");

            // Act
            var actualMapper = handler.GetFooterRowMapper();

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Total" &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 1);
        }
    }
}
