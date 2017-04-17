namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLossAnalysis;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.WinLossAnalysis;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinLossAnalysis;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    [Collection("ObjectDbCollection")]
    public class WinLossAnalysisDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly WinLossAnalysisDataAccessImpl winLossAnalysisDataAccessImpl;
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;

        public WinLossAnalysisDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            cacheRefService = Substitute.For<CacheRefService>();
            winLossAnalysisDataAccessImpl = new WinLossAnalysisDataAccessImpl(cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossAnalysisCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossAnalysisDataAccessImpl.GetWinLossAnalysis(nullCriteria));
        }

        [Fact]
        public void GetData_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var criteria = new WinLossAnalysisCriteria(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: null,
                reportLevel: UserLevel.Super);

            var dataSet = WinLossAnalysisDatabase.MasterWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

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
                custId: 0,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: null,
                reportLevel: UserLevel.Master);

            var dataSet = WinLossAnalysisDatabase.AgentWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

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
                custId: 0,
                reportType: 2,
                fromDate: DateTime.Now,
                toDate: DateTime.Now,
                balanceUpDay: DateTime.Now,
                moveBetsDay: DateTime.Now,
                productIds: null,
                reportLevel: UserLevel.Agent);

            var dataSet = WinLossAnalysisDatabase.MemberWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataSet);

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
                productIds: null,
                reportLevel: UserLevel.Super);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossAnalysisCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossAnalysisModel>();

            // Act
            var actualData = winLossAnalysisDataAccessImpl.GetWinLossAnalysis(criteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}