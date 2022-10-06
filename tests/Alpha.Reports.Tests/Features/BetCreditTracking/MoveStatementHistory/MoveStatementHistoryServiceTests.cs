namespace Alpha.Reports.Tests.Features.BetCreditTracking.MoveStatementHistory
{
    using System;
    using Alpha.PartialUM.Services;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class MoveStatementHistoryServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly MoveStatementHistoryService service;
        private readonly IPartialUMService partialUMService;

        public MoveStatementHistoryServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<IPartialUMService>();
            service = new MoveStatementHistoryServiceImpl(repository, partialUMService);
        }

        [Fact]
        public void GetMoveStatementHistory_Always_CallThrowIfUM()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100);

            // Act
            service.GetMoveStatementHistory(param);

            // Assert
            partialUMService.Received().ThrowIfUM("Age_Rpt_StatementMovement");
        }

        [Fact]
        public void GetMoveStatementHistory_Always_CallFetch()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100);

            // Act
            service.GetMoveStatementHistory(param);

            // Assert
            repository.Received().Fetch<MoveStatementHistoryByRefNoModel>(Arg.Is<MoveStatementHistoryCriteria>(
                criteria => criteria.Username == param.Username
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.LoggedId == param.ViewerId
                && criteria.PageSize == param.PageSize
                && criteria.PageIndex == param.PageIndex
                ));
        }
    }
}
