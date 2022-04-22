namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class BetCreditHistoryServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly BetCreditHistoryService service;
        private readonly PartialUMService partialUMService;

        public BetCreditHistoryServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<PartialUMService>();
            service = new BetCreditHistoryServiceImpl(repository, partialUMService);
        }

        [Fact]
        public void GetBetCreditHistory_Always_CallCheckPartialUM()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100);

            // Act
            service.GetBetCreditHistory(param);

            // Assert
            partialUMService.Received().CheckPartialUM("Age_Rpt_BetCreditHistory");
        }

        [Fact]
        public void GetBetCreditHistory_Always_CallFetch()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100);

            // Act
            service.GetBetCreditHistory(param);

            // Assert
            repository.Received().Fetch<BetCreditHistoryModel>(Arg.Is<BetCreditHistoryCriteria>(
                criteria => criteria.Username == param.Member
                && criteria.RefNo == param.RefNo
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.LoggedId == param.ViewerId
                && criteria.PageIndex == param.PageIndex
                && criteria.PageSize == param.PageSize
                ));
        }
    }
}
