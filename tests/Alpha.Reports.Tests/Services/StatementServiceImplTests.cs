namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
    using Alpha.Reports.Services;
    using Alpha.Reports.Tests.Constants;
    using Alpha.Reports.Tests.Extensions;
    using Alpha.Reports.Tests.Fixtures;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("AnyValueCreatorCollection")]
    public class StatementServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository repository;
        private readonly StatementServiceImpl serviceImpl;
        private readonly StatementDataAccess statementRepository;
        private readonly IPartialUMService partialUMService;

        public StatementServiceImplTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;

            reportDateService = Substitute.For<ReportDateService>();
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDay);

            repository = Substitute.For<IDynamicRepository>();

            statementRepository = Substitute.For<StatementDataAccess>();
            statementRepository.GetStatements(
                Arg.Is<GetStatementCriteria>(criteria => criteria.GetSettingKey() == "Statement_Report_GetList"))
                .Returns(new List<StatementModel>
                {
                    new StatementModel(new DateTime(2017, 4, 12), 0, 20, null, null, null, null, null, null, null, null)
                });

            statementRepository.GetStatements(
                Arg.Is<GetStatementCriteria>(criteria => criteria.GetSettingKey() == "Statement_Report_GetList_14"))
                .Returns(new List<StatementModel>
                {
                    new StatementModel(new DateTime(2017, 4, 5), 0, 20, null, null, null, null, null, null, null, null),
                    new StatementModel(new DateTime(2017, 4, 6), 0, 20, null, null, null, null, null, null, null, null)
                });

            statementRepository.GetStatements(
                Arg.Is<GetStatementCriteria>(criteria => criteria.GetSettingKey() == "Statement_Report_GetList_WH"))
                .Returns(new List<StatementModel>
                {
                    new StatementModel(new DateTime(2017, 3, 30), 0, 20, null, null, null, null, null, null, null, null),
                    new StatementModel(new DateTime(2017, 3, 31), 0, 20, null, null, null, null, null, null, null, null)
                });
            partialUMService = Substitute.For<IPartialUMService>();

            serviceImpl = new StatementServiceImpl(reportDateService, repository, statementRepository, partialUMService);
        }

        [Fact]
        public void GetStatements_Always_ReturnsCorrectData()
        {
            // Arrange
            var param = new GetStatementParam(0, new DateTime(2017, 3, 29), new DateTime(2017, 4, 13), DateTime.MinValue);
            reportDateService.GetTimeRangesToGetReportDataBetweenTables(
                Arg.Is<DateTime>(date => date == new DateTime(2017, 3, 29)),
                Arg.Is<DateTime>(date => date == new DateTime(2017, 4, 13)))
                .Returns(new List<TimeRange>
                {
                    new TimeRange(new DateTime(2017, 3, 29), new DateTime(2017, 4, 4)),
                    new TimeRange(new DateTime(2017, 4, 5), new DateTime(2017, 4, 11)),
                    new TimeRange(new DateTime(2017, 4, 12), new DateTime(2017, 4, 13))
                });

            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 4, 12)))
                .Returns(string.Empty);
            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 4, 5)))
                .Returns("_14");
            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 3, 29)))
                .Returns("_WH");

            var expectedModels = new List<StatementModel>
            {
                new StatementModel(new DateTime(2017, 3, 30), 0, 20, null, null, null, null, null, null, null, null),
                new StatementModel(new DateTime(2017, 3, 31), 0, 20, null, null, null, null, null, null, null, null),
                new StatementModel(new DateTime(2017, 4, 5), 0, 20, null, null, null, null, null, null, null, null),
                new StatementModel(new DateTime(2017, 4, 6), 0, 20, null, null, null, null, null, null, null, null),
                new StatementModel(new DateTime(2017, 4, 12), 0, 20, null, null, null, null, null, null, null, null)
            };

            // Act
            var actualModels = serviceImpl.GetStatements(param);

            // Assert
            AssertExtension.Equal(expectedModels, actualModels);
        }

        [Fact]
        public void GetStatements_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var today = new DateTime(2017, 3, 29);
            var limitedDate = today.AddDays(1);
            var param = new GetStatementParam(0, today, today.AddDays(1), limitedDate);
            reportDateService.GetTimeRangesToGetReportDataBetweenTables(
                Arg.Is<DateTime>(date => date == today),
                Arg.Is<DateTime>(date => date == today.AddDays(1)))
                .Returns(new List<TimeRange>
                {
                    new TimeRange(today, today.AddDays(1)),
                    new TimeRange(new DateTime(2017, 4, 5), new DateTime(2017, 4, 11)),
                    new TimeRange(new DateTime(2017, 4, 12), new DateTime(2017, 4, 13))
                });

            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 4, 12)))
                .Returns(string.Empty);
            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 4, 5)))
                .Returns("_14");
            reportDateService.GetStoreMappingSuffix(Arg.Is<DateTime>(date => date == new DateTime(2017, 3, 29)))
                .Returns("_WH");

            // Act
            serviceImpl.GetStatements(param);

            // Assert
            statementRepository.Received().GetStatements(Arg.Is<GetStatementCriteria>(i => i.FromDate == limitedDate));
        }

        [Fact]
        public void StatementDetails_FromBalanceUpDate_ReturnsCorrectData()
        {
            // Arrange
            var param = new StatementDetailsParam(0, new DateTime(2017, 4, 13));
            var data = new List<StatementDetailModel>
            {
                anyValueCreator.Create<StatementDetailModel>()
            };

            var expectedData = data;
            repository.Fetch<StatementDetailModel>(
                Arg.Is<TransferDetailsCriteria>(criteria => criteria.GetSettingKey() == "Statement_Report_TransferDetails"))
                .Returns(data);

            // Act
            var actualData = serviceImpl.StatementDetails(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void StatementDetails_FromWarehouse_ReturnsCorrectData()
        {
            // Arrange
            var param = new StatementDetailsParam(0, new DateTime(2016, 11, 25));
            var data = new List<StatementDetailModel>
            {
                anyValueCreator.Create<StatementDetailModel>()
            };

            var expectedData = data;
            repository.Fetch<StatementDetailModel>(
                Arg.Is<TransferDetailsCriteria>(criteria => criteria.GetSettingKey() == "Statement_Report_TransferDetails_WH"))
                .Returns(data);

            // Act
            var actualData = serviceImpl.StatementDetails(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}