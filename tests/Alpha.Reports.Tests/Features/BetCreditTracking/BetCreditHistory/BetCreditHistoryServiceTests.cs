namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Tests.Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class BetCreditHistoryServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly BetCreditHistoryService service;
        private readonly IPartialUMService partialUMService;
        private readonly ICacheService cacheService;

        public BetCreditHistoryServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            partialUMService = Substitute.For<IPartialUMService>();
            cacheService = Substitute.For<ICacheService>();
            service = new BetCreditHistoryServiceImpl(repository, partialUMService, cacheService);
        }

        [Fact]
        public void GetBetCreditHistory_Always_CallThrowIfUM()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1, 100);

            // Act
            service.GetBetCreditHistory(param);

            // Assert
            partialUMService.Received().ThrowIfUM("Age_Rpt_BetCreditHistory");
        }

        [Fact]
        public void GetBetCreditHistory_Always_CallFetchBetCreditHistory()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1, 100);

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

        [Fact]
        public void GetBetCreditHistory_WhenActionIsFromUserLog_CallFetchLogActions()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1, 100);
            var reportData = new List<BetCreditHistoryModel>
            {
                new BetCreditHistoryModel(DateTime.Now, new DateTime(2022, 3, 3, 23, 59, 00), 123456, 10, 10, 10, 10, 10, 10, 10, 1, "Bet Updated", 1)
            };
            repository.Fetch<BetCreditHistoryModel>(Arg.Any<BetCreditHistoryCriteria>()).Returns(reportData);

            // Act
            service.GetBetCreditHistory(param);

            // Assert
            repository.Received().Fetch<LogAction>(Arg.Any<GetLogActionsCriteria>());
        }

        [Fact]
        public void GetBetCreditHistory_Always_ReturnCorrectBetCreditHistoryResultModel()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1, 100);
            var expected = new List<string>
            {
                // ActionId = -1
                "Opening Credit",
                // ActionId = 0
                "Bet updated, LOSE to WON",
                "Bet updated, VOID to running",
                "Bet updated, WON to LOSE",
                "Bet updated",
                "Bet updated, running to WON",
                // ActionId = 10
                "Bet placed",
                // ActionId = 11
                "Bet placed",
                // ActionId = 12
                "Bet placed",
                "Bet placed",
                // ActionId = 20
                "Bet rejected",
                // ActionId = 29
                "Cash Out, LOSE to WON",
                // ActionId = 30
                "Bet settled, running to WON",
                "Bet settled, running to WON",
                "Bet settled, WON to LOSE",
                "Bet settled, running to WON",
                // ActionId = 31
                "Bet updated, LOSE to WON",
                "Bet updated, WON to LOSE",
                "Bet updated",
                "Bet updated, LOSE to WON",
                "Bet updated",
                "Bet updated, WON to DRAW",
                // ActionId = 32
                "Bet updated, running to WON",
                "Bet updated, running to running",
                "Bet updated, WON to running",
                "Bet updated",
                "Bet updated, running to WON",
                "Bet updated",
                // ActionId = 32
                "Cancel Cash Out, running to running",
                // ActionId = 50
                "Bet updated, running to VOID",
                "Bet updated, running to VOID",
                "Bet updated, running to VOID",
                "Bet updated, running to VOID",
                "Bet updated, running to VOID",
                "Bet updated, running to VOID",
                // ActionId = 55
                "Bet updated, LOSE to WON",
                "Bet updated, VOID to running",
                "Bet updated, VOID to running",
                // ActionId = 110
                "Transfer submitted (123456)"
            };
            var reportData = new List<BetCreditHistoryModel>
            {
                CreateBetCreditHistoryModel(-1, "Opening Credit"),
                CreateBetCreditHistoryModel(0, "Bet updated, LOSE to WON"),
                CreateBetCreditHistoryModel(0, "TransLog settled, VOID to running"),
                CreateBetCreditHistoryModel(0, "LC (IsSUM : 0) settled, WON to LOSE"),
                CreateBetCreditHistoryModel(0, "LC (IsSUM : 0 to 1) settled updated"),
                CreateBetCreditHistoryModel(0, "RNG (IsSUM : 0) settled, running to WON"),
                CreateBetCreditHistoryModel(10, "Bet inserted as running"),
                CreateBetCreditHistoryModel(11, "TransLog inserted as WON"),
                CreateBetCreditHistoryModel(12, "LC (IsSUM : 0) inserted as running"),
                CreateBetCreditHistoryModel(12, "RNG (IsSUM : 0) inserted as running"),
                CreateBetCreditHistoryModel(20, "Bet rejected"),
                CreateBetCreditHistoryModel(29, "Bet Cash Out, LOSE to WON"),
                CreateBetCreditHistoryModel(30, "Bet settled, running to WON"),
                CreateBetCreditHistoryModel(30, "TransLog settled, running to WON"),
                CreateBetCreditHistoryModel(30, "LC (IsSUM : 0) settled, WON to LOSE"),
                CreateBetCreditHistoryModel(30, "RNG (IsSUM : 0) settled, running to WON"),
                CreateBetCreditHistoryModel(31, "Bet updated, LOSE to WON"),
                CreateBetCreditHistoryModel(31, "TransLog updated, WON to LOSE"),
                CreateBetCreditHistoryModel(31, "LC (IsSUM : 1 to 0) 110590276645 updated"),
                CreateBetCreditHistoryModel(31, "LC (IsSUM : 0) updated, LOSE to WON"),
                CreateBetCreditHistoryModel(31, "RNG (IsSUM : 1 to 0) 110590276645 updated"),
                CreateBetCreditHistoryModel(31, "RNG (IsSUM : 0) 110590276645 updated, WON to DRAW"),
                CreateBetCreditHistoryModel(32, "TransLog updated, running to WON"),
                CreateBetCreditHistoryModel(32, "Bet Updated, running to running"),
                CreateBetCreditHistoryModel(32, "LC (IsSUM : 0) updated, WON to running"),
                CreateBetCreditHistoryModel(32, "LC (IsSUM : 1 to 0) 110590276645 updated"),
                CreateBetCreditHistoryModel(32, "RNG (IsSUM : 0) 110590276645 updated, running to WON"),
                CreateBetCreditHistoryModel(32, "RNG (IsSUM : 1 to 0) 110590276645 updated"),
                CreateBetCreditHistoryModel(34, "Bet Cancel Cash Out, running to running"),
                CreateBetCreditHistoryModel(50, "Bet Updated, running to VOID"),
                CreateBetCreditHistoryModel(50, "Bet voided"),
                CreateBetCreditHistoryModel(50, "TransLog updated, running to VOID"),
                CreateBetCreditHistoryModel(50, "Translog voided"),
                CreateBetCreditHistoryModel(50, "LC (IsSUM : [IsSUM]) updated, running to VOID"),
                CreateBetCreditHistoryModel(50, "LC (IsSUM : [IsSUM]) voided"),
                CreateBetCreditHistoryModel(55, "TransLog updated, LOSE to WON"),
                CreateBetCreditHistoryModel(55, "LC (IsSUM : 0) updated, void to running"),
                CreateBetCreditHistoryModel(55, "RNG (IsSUM : 0) updated, void to running"),
                CreateBetCreditHistoryModel(110, "Transfer inserted (123456)")
            };
            repository.Fetch<BetCreditHistoryModel>(Arg.Any<BetCreditHistoryCriteria>()).Returns(reportData);

            // Act
            var result = service.GetBetCreditHistory(param);

            // Assert
            AssertExtension.Equal(expected, result.Data.Select(r => r.Action).ToList());
        }

        private BetCreditHistoryModel CreateBetCreditHistoryModel(int actionId, string action)
            => new BetCreditHistoryModel(DateTime.Now, DateTime.Now, 123456, 10, 10, 10, 10, 10, 10, 10, actionId, action, 0);
    }
}