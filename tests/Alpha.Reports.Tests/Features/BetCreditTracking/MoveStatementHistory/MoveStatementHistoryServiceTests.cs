namespace Alpha.Reports.Tests.Features.BetCreditTracking.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class MoveStatementHistoryServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly MoveStatementHistoryService service;

        public MoveStatementHistoryServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            service = new MoveStatementHistoryServiceImpl(repository);
        }

        [Fact]
        public void GetMoveStatementHistory_Always_CallFetch()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue);

            // Act
            service.GetMoveStatementHistory(param);

            // Assert
            repository.Received().Fetch<MoveStatementHistoryByRefNoModel>(Arg.Is<MoveStatementHistoryCriteria>(
                criteria => criteria.Username == param.Username
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.LoggedId == param.ViewerId
                ));
        }
    }
}
