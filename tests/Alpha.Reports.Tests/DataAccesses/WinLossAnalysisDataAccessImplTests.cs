namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossAnalysis;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinLossAnalysis;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("ObjectDbCollection")]
    public class WinLossAnalysisDataAccessImplTests
    {
        private readonly WinLossAnalysisDataAccessImpl winLossAnalysisDataAccessImpl;
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;
        private readonly IDynamicRepository repository;

        public WinLossAnalysisDataAccessImplTests()
        {
            cacheRefService = Substitute.For<CacheRefService>();
            repository = Substitute.For<IDynamicRepository>();
            winLossAnalysisDataAccessImpl = new WinLossAnalysisDataAccessImpl(repository, cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetData_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new WinLossAnalysisCriteria(
                custId: 1,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: "1,2",
                reportLevel: UserLevel.Super);
            var data = new List<WinLossAnalysisEntity>
            {
                new WinLossAnalysisEntity
                {
                    RollupKey = "02/2017",
                    BetCount = 10,
                    Turnover = 0,
                    MemberWinLoss = 3,
                    MemberComm = 4,
                    AgWinLoss = 5,
                    AgComm = 6,
                    MAgWinLoss = 7,
                    MAgComm = 8,
                    SAgWinLoss = 9,
                    SAgComm = 10
                },
                new WinLossAnalysisEntity
                {
                    RollupKey = "03/2017",
                    BetCount = 10,
                    Turnover = 20,
                    MemberWinLoss = 13,
                    MemberComm = 14,
                    AgWinLoss = 15,
                    AgComm = 16,
                    MAgWinLoss = 17,
                    MAgComm = 18,
                    SAgWinLoss = 19,
                    SAgComm = 20
                }
            };
            repository.Fetch<WinLossAnalysisEntity>(Arg.Any<WinLossAnalysisCriteria>()).Returns(data);

            var expectedData = new List<WinLossAnalysisModel>()
            {
                new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: new NetWinLoss(7, 8),
                 superNetWinLoss: new NetWinLoss(9, 10)),

                new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: new NetWinLoss(17, 18),
                 superNetWinLoss: new NetWinLoss(19, 20))
            };

            // Act
            var actualData = winLossAnalysisDataAccessImpl.GetWinLossAnalysis(criteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetData_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new WinLossAnalysisCriteria(
                custId: 1,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: "1,2",
                reportLevel: UserLevel.Master);

            var dataSet = new List<WinLossAnalysisEntity> { };
            repository.Fetch<WinLossAnalysisEntity>(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossAnalysisModel>()
            {
                new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: new NetWinLoss(7, 8),
                 superNetWinLoss: null),

                new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: new NetWinLoss(17, 18),
                 superNetWinLoss: null)
            };

            // Act
            var actualData = winLossAnalysisDataAccessImpl.GetWinLossAnalysis(criteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetData_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new WinLossAnalysisCriteria(
                custId: 1,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: "1,2",
                reportLevel: UserLevel.Agent);

            var dataSet = new List<WinLossAnalysisEntity> { };
            repository.Fetch<WinLossAnalysisEntity>(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossAnalysisModel>()
            {
                new WinLossAnalysisModel(
                 rollupKey: "02/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 0,
                 memberNetWinLoss: new NetWinLoss(3, 4),
                 agentNetWinLoss: new NetWinLoss(5, 6),
                 masterNetWinLoss: null,
                 superNetWinLoss: null),

               new WinLossAnalysisModel(
                 rollupKey: "03/2017",
                 rollupKeyId: null,
                 betCount: 10,
                 turnover: 20,
                 memberNetWinLoss: new NetWinLoss(13, 14),
                 agentNetWinLoss: new NetWinLoss(15, 16),
                 masterNetWinLoss: null,
                 superNetWinLoss: null)
            };

            // Act
            var actualData = winLossAnalysisDataAccessImpl.GetWinLossAnalysis(criteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetData_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var criteria = new WinLossAnalysisCriteria(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: "1,2",
                reportLevel: UserLevel.Super);

            var dataSet = Enumerable.Empty<WinLossAnalysisEntity>();
            repository.Fetch<WinLossAnalysisEntity>(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossAnalysisModel>();

            // Act
            var actualData = winLossAnalysisDataAccessImpl.GetWinLossAnalysis(criteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}