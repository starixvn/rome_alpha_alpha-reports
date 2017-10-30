namespace Alpha.Reports.Tests.DataAccesses.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria.MatchWinLossDetail;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("ObjectDbCollection")]
    public class MatchWinLossDetailDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly MatchWinLossDetailDataAccessImpl matchWinLossDetailDataAccessImpl;

        public MatchWinLossDetailDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            matchWinLossDetailDataAccessImpl = new MatchWinLossDetailDataAccessImpl();
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossDetailCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossDetailDataAccessImpl.GetData(nullCriteria));
        }

        [Theory]
        [MemberData("ReportByLevel", MemberType = typeof(MatchWinLossDetailDataAccessImplTestCases))]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void GetData_WinLossReportHasData_ReturnsDataFromObjectDb(
            UserLevel level,
            DataSet dataSet)
        {
            // Arrange
            var criteria = new MatchWinLossDetailCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                level,
                Enumerable.Empty<string>());

            var reportData = dataSet.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<MatchWinLossDetailCriteria>()).Returns(reportData);

            var expectedData = new List<MatchWinLossDetailModel>
            {
                new MatchWinLossDetailModel(
                    isParlay: true,
                    matchId: 1,
                    homeId: 2,
                    awayId: 4,
                    betType: 13,
                    liveIndicator: false,
                    leagueId: 5,
                    race: 6,
                    matchCode: "Match Code 1",
                    isNeutral: false,
                    showTime: "Show Time 1",
                    turnover: 1,
                    grossComm: 2,
                    memberNetWinLoss: new NetWinLoss(3, 4),
                    agentNetWinLoss: new NetWinLoss(5, 6),
                    masterNetWinLoss: level.IsAgentLevel() ? null : new NetWinLoss(7, 8),
                    superNetWinLoss: level.IsSuperLevel() ? new NetWinLoss(9, 10) : null),
                new MatchWinLossDetailModel(
                    isParlay: false,
                    matchId: 11,
                    homeId: 12,
                    awayId: 14,
                    betType: 113,
                    liveIndicator: true,
                    leagueId: 15,
                    race: 16,
                    matchCode: "Match Code 11",
                    isNeutral: true,
                    showTime: "Show Time 11",
                    turnover: 11,
                    grossComm: 12,
                    memberNetWinLoss: new NetWinLoss(13, 14),
                    agentNetWinLoss: new NetWinLoss(15, 16),
                    masterNetWinLoss: level.IsAgentLevel() ? null : new NetWinLoss(17, 18),
                    superNetWinLoss: level.IsSuperLevel() ? new NetWinLoss(19, 110) : null)
            };

            // Act
            var actualData = matchWinLossDetailDataAccessImpl.GetData(criteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_NullDataReturnFromDb_ReturnsEmptyList()
        {
            // Arrange
            var criteria = new MatchWinLossDetailCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                UserLevel.Super,
                Enumerable.Empty<string>());

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<MatchWinLossDetailCriteria>()).Returns(dataReader);

            // Act
            var actualData = matchWinLossDetailDataAccessImpl.GetData(criteria);

            // Assert
            Assert.Empty(actualData);
        }
    }
}
