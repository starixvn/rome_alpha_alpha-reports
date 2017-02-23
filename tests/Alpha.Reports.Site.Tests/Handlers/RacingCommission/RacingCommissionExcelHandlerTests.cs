namespace Alpha.Reports.Site.Tests.Handlers.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.RacingCommission;
    using Services;
    using Site.Handlers.RacingCommission;
    using Site.Requests.RacingCommission;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    [Collection("UserCollection")]
    public class RacingCommissionExcelHandlerTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly UserFixture userFixture;

        private readonly RacingCommissionExcelHandler excelHandler;
        private readonly RacingCommissionService service;
        private readonly Workbook<RacingCommissionModel> workbook;

        public RacingCommissionExcelHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            workbook = Substitute.For<Workbook<RacingCommissionModel>>();
            workbook.UseTemplate(Arg.Any<TemplateInfo>()).Returns(workbook);
            workbook.Title(Arg.Any<string>()).Returns(workbook);
            workbook.Header(Arg.Any<Func<string, string>>()).Returns(workbook);
            workbook.Body(Arg.Any<IEnumerable<RacingCommissionModel>>(), Arg.Any<Mapper<RacingCommissionModel>>()).Returns(workbook);
            workbook.Footer(Arg.Any<RacingCommissionModel>(), Arg.Any<Mapper<RacingCommissionModel>>()).Returns(workbook);
            workbook.Sheet(Arg.Any<string>()).Returns(workbook);

            service = Substitute.For<RacingCommissionService>();
            excelHandler = Substitute.For<RacingCommissionExcelHandler>(service, workbook);
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
            var commissions = new List<RacingCommissionModel> { fixture.Create<RacingCommissionModel>() };
            service.GetCustomerCommission(Arg.Any<RacingCommissionParam>()).Returns(commissions);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Body(
                Arg.Is<IEnumerable<RacingCommissionModel>>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(commissions)),
                Arg.Any<Mapper<RacingCommissionModel>>());
        }

        [Fact]
        public void MustCall_Footer_WithCorrectParameters()
        {
            // Arrange
            var message = CreateExportExcelQuery();
            var commissions = new List<RacingCommissionModel> { fixture.Create<RacingCommissionModel>() };
            service.GetCustomerCommission(Arg.Any<RacingCommissionParam>()).Returns(commissions);
            var total = RacingCommissionModel.GetAccumulatedCommission(commissions);

            // Act
            excelHandler.Handle(message);

            // Assert
            workbook.Received().Footer(
                Arg.Is<RacingCommissionModel>(dataSource => JsonConvert.SerializeObject(dataSource) == JsonConvert.SerializeObject(total)),
                Arg.Any<Mapper<RacingCommissionModel>>());
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

        private RacingCommissionExcelQuery CreateExportExcelQuery(byte custLevelId = 4)
        {
            var excelQuery = new RacingCommissionExcelQuery(userFixture.User);
            excelQuery.CustLevelId = custLevelId;
            excelQuery.FromDate = DefaultVariables.CurrentDate;
            excelQuery.ToDate = DefaultVariables.CurrentDate;

            return excelQuery;
        }
    }
}
