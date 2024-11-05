namespace Alpha.Reports.Site.Tests.Handlers.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.CommissionByBettype;
    using Services;
    using Site.Handlers.CommissionByBettype;
    using Site.Requests.CommissionByBettype;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    [Collection("UserCollection")]
    public class CommissionByBettypeExcelHandlerTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly UserFixture userFixture;

        private readonly CommissionByBettypeExcelHandler excelHandler;
        private readonly CommissionByBettypeService reportService;
        private readonly Workbook<CommissionByBettypeModel> workbook;

        public CommissionByBettypeExcelHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            workbook = Substitute.For<Workbook<CommissionByBettypeModel>>();
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<CommissionByBettypeModel>>(), Arg.Any<Mapper<CommissionByBettypeModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<CommissionByBettypeModel>(), Arg.Any<Mapper<CommissionByBettypeModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            reportService = Substitute.For<CommissionByBettypeService>();
            excelHandler = Substitute.For<CommissionByBettypeExcelHandler>(reportService, Substitute.For<ReportDateService>(), workbook);
        }

        [Fact]
        public void MustCall_TemplateInfo()
        {
            // Arrange
            var message = CreateExportExcelQuery();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().UseTemplate(Arg.Any<TemplateInfo>());
        }

        [Fact]
        public void MustCall_Title()
        {
            // Arrange
            var message = CreateExportExcelQuery();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Title(Arg.Any<string>());
        }

        [Fact]
        public void MustCall_Header()
        {
            // Arrange
            var message = CreateExportExcelQuery();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Header(Arg.Any<Func<string, string>>());
        }

        [Fact]
        public void MustCall_Body_WithCorrectParameters()
        {
            // Arrange
            var message = CreateExportExcelQuery();
            var commissions = new List<CommissionByBettypeModel> { fixture.Create<CommissionByBettypeModel>() };
            reportService.GetAllCommissionByBettypes(Arg.Any<CommissionByBettypeParam>()).Returns(commissions);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<CommissionByBettypeModel>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(commissions)),
                Arg.Any<Mapper<CommissionByBettypeModel>>());
        }

        [Fact]
        public void MustCall_Footer_WithCorrectParameters()
        {
            // Arrange
            var message = CreateExportExcelQuery();
            var commissions = new List<CommissionByBettypeModel> { fixture.Create<CommissionByBettypeModel>() };
            reportService.GetAllCommissionByBettypes(Arg.Any<CommissionByBettypeParam>()).Returns(commissions);
            var total = CommissionByBettypeModel.GetAccumulatedCommission(commissions);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Footer(
                Arg.Is<CommissionByBettypeModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(total)),
                Arg.Any<Mapper<CommissionByBettypeModel>>());
        }

        [Fact]
        public void MustCall_Sheet()
        {
            // Arrange
            var message = CreateExportExcelQuery();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Sheet(Arg.Any<string>());
        }

        [Fact]
        public void MustCall_ExportToBytes()
        {
            // Arrange
            var message = CreateExportExcelQuery();

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().ExportToBytes();
        }

        [Fact]
        public void GetRowMapper_BodyMapper_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new CommissionByBettypeModel(1, "Username", 2, 3, 4, 5, false);

            // Act
            var actualMapper = excelHandler.GetRowMapper(UserLevel.Agent, false);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Username" &&
                (decimal)actualMapper.Mappings[Column.B](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 6 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 5);
        }

        [Fact]
        public void GetRowMapper_TotalMapper_ReturnsCorrectMapper()
        {
            // Arrange
            var viewModel = new CommissionByBettypeModel(1, "Username", 2, 3, 4, 5, false);

            // Act
            var actualMapper = excelHandler.GetRowMapper(UserLevel.Agent, true);

            // Assert
            Assert.True(
                (string)actualMapper.Mappings[Column.A](viewModel) == "Total" &&
                (decimal)actualMapper.Mappings[Column.B](viewModel) == 2 &&
                (decimal)actualMapper.Mappings[Column.C](viewModel) == 3 &&
                (decimal)actualMapper.Mappings[Column.D](viewModel) == 4 &&
                (decimal)actualMapper.Mappings[Column.E](viewModel) == 6 &&
                (decimal)actualMapper.Mappings[Column.F](viewModel) == 5);
        }

        private CommissionByBettypeExcelQuery CreateExportExcelQuery(byte custLevelId = 4)
        {
            var excelQuery = new CommissionByBettypeExcelQuery(userFixture.User);
            excelQuery.CustLevelId = custLevelId;
            excelQuery.FromDate = DefaultVariablesTest.CurrentDate;
            excelQuery.ToDate = DefaultVariablesTest.CurrentDate;

            return excelQuery;
        }
    }
}