namespace Alpha.Reports.Tests.Features.BetCreditTracking.MoveStatementHistory
{
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class MoveStatementHistoryServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly MoveStatementHistoryService service;
        private readonly PartialUMService partialUMService;

        public MoveStatementHistoryServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<PartialUMService>();
            service = new MoveStatementHistoryServiceImpl(repository, partialUMService);
        }

        [Fact]
        public void GetMoveStatementHistory_Always_CallCheckPartialUM()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue);

            // Act
            service.GetMoveStatementHistory(param);

            // Assert
            partialUMService.Received().CheckPartialUM("Age_Rpt_StatementMovement");
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
