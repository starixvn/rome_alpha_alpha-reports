namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria.MatchWinLoss;
    using Reports.Models;
    using Reports.Models.MatchWinLoss;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossServiceImplTest
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly MatchWinLossServiceImpl matchWinLossServiceImpl;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly ReportDateService reportDateService;

        public MatchWinLossServiceImplTest(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            reportDateService = Substitute.For<ReportDateService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            MatchWinLossServiceImpl.DateRangeType = DateRangeType.FromBalanceUpDay;
            matchWinLossServiceImpl = new MatchWinLossServiceImpl(reportDateService, dynamicRepository);

            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetMatchWinLoss_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossServiceImpl.GetMatchWinLoss(nullParam));
        }

        [Fact]
        public void GetMatchWinLoss_FromDateLessThanBalanceUpDay_ReturnsEmptyData()
        {
            // Arrange
            var balanceUpDay = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetBalanceUpDay().Returns(balanceUpDay);

            var fromDate = balanceUpDay.AddDays(-1);

            var param = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                fromDate,
                DefaultVariables.CurrentDate);

            // Act
            var actualData = matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            Assert.Empty(actualData);
        }

        [Fact]
        public void GetMatchWinLoss_ToDateLessThanFromDate_ReturnsEmptyData()
        {
            // Arrange
            var balanceUpDay = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetBalanceUpDay().Returns(balanceUpDay);

            var fromDate = balanceUpDay;
            var toDate = fromDate.AddDays(-1);

            var param = new MatchWinLossParam(anyValueCreatorFixture.Create<int>(), fromDate, toDate);

            // Act
            var actualData = matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            Assert.Empty(actualData);
        }

        [Fact]
        public void GetMatchWinLoss_ValidParam_CallsDynamicRepository()
        {
            // Arrange
            var param = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate);
            reportDateService.ValidateDateRange(Arg.Any<DateRange>()).Returns(true);

            // Act
            matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            dynamicRepository.Received().Fetch<MatchWinLossModel>(Arg.Is<GetMatchWinLossCriteria>(
                criteria => criteria.CustId == param.CustId
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate));
        }

        [Fact]
        public void GetMatchWinLoss_ValidParam_ReturnsDataFromDynamicRepository()
        {
            var param = new MatchWinLossParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate);
            reportDateService.ValidateDateRange(Arg.Any<DateRange>()).Returns(true);

            var expectedMatchWinLossModels = new List<MatchWinLossModel>
            {
                 anyValueCreatorFixture.Create<MatchWinLossModel>(),
                 anyValueCreatorFixture.Create<MatchWinLossModel>(),
                 anyValueCreatorFixture.Create<MatchWinLossModel>()
            };

            dynamicRepository.Fetch<MatchWinLossModel>(Arg.Any<GetMatchWinLossCriteria>()).Returns(expectedMatchWinLossModels);

            // Act
            var actualMatchWinLossModels = matchWinLossServiceImpl.GetMatchWinLoss(param);

            // Assert
            AssertExtension.Equal(expectedMatchWinLossModels, actualMatchWinLossModels);
        }
    }
}
