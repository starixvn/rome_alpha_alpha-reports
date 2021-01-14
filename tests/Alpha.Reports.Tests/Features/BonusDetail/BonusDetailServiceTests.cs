namespace Alpha.Reports.Tests.Features.BonusDetail
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Reports.Features.BonusDetail;
    using Alpha.Reports.Features.BonusDetail.Constants;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Models;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class BonusDetailServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly BonusDetailService service;

        public BonusDetailServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            service = new BonusDetailServiceImpl(repository);
        }

        [Fact]
        public async Task GetBonusDetailAsync_Super_CallFetchAsync()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Super, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);

            // Act
            await service.GetBonusDetailAsync(param);

            // Assert
            await repository.Received().FetchAsync<BonusEntity>(Arg.Is<GetSuperBonusCriteria>(
                criteria => criteria.SuperId == param.ViewerId
                && criteria.Username == param.Username
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.StatusId == param.StatusId
                && criteria.IsComplained == param.IsComplained
                && criteria.PageIndex == param.PageIndex
                && criteria.PageSize == param.PageSize));
        }

        [Fact]
        public async Task GetBonusDetailAsync_Master_CallFetchAsync()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Master, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);

            // Act
            await service.GetBonusDetailAsync(param);

            // Assert
            await repository.Received().FetchAsync<BonusEntity>(Arg.Is<GetMasterBonusCriteria>(
                criteria => criteria.MasterId == param.ViewerId
                && criteria.Username == param.Username
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.StatusId == param.StatusId
                && criteria.IsComplained == param.IsComplained
                && criteria.PageIndex == param.PageIndex
                && criteria.PageSize == param.PageSize));
        }

        [Fact]
        public async Task GetBonusDetailAsync_Agent_CallFetchAsync()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);

            // Act
            await service.GetBonusDetailAsync(param);

            // Assert
            await repository.Received().FetchAsync<BonusEntity>(Arg.Is<GetAgentBonusCriteria>(
                criteria => criteria.AgentId == param.ViewerId
                && criteria.Username == param.Username
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.StatusId == param.StatusId
                && criteria.IsComplained == param.IsComplained
                && criteria.PageIndex == param.PageIndex
                && criteria.PageSize == param.PageSize));
        }

        [Fact]
        public async Task GetBonusDetailAsync_Agent_ReturnsCorrectData()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Super, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            repository.FetchAsync<BonusEntity>(Arg.Any<GetSuperBonusCriteria>()).Returns(FakeSuperData());

            // Act
            var reportData = await service.GetBonusDetailAsync(param);

            // Assert
            Assert.True(reportData.TotalData.TotalAmount == 550);
        }

        [Fact]
        public async Task ConfirmPaymentAsync_IsValid_CallExecuteAsync()
        {
            // Arrange
            var param = new ConfirmPaymentParam(8, 0, 123, 1001, DateTime.Today, "::1", "E");

            // Act
            await service.ConfirmPaymentAsync(param);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Is<ConfirmPaymentCriteria>(
                criteria => criteria.CustId == param.CustId
                && criteria.SubAccId == param.SubAccId
                && criteria.TransId == param.TransId
                && criteria.PaymentDate == param.PaymentDate
                && criteria.UserId == param.ViewerId
                && criteria.LoginIP == param.LoginIP
                && criteria.Language == param.Language));
        }

        private static IEnumerable<BonusEntity> FakeSuperData()
        {
            yield return new BonusEntity(new DateTime(2020, 12, 9), 1010, "AA01", "AA0101", 1, "AA0101001", 100, new DateTime(2020, 12, 16), BonusStatusIdConst.Completed, false);
            yield return new BonusEntity(new DateTime(2020, 12, 9), 1009, "AA02", "AA0201", 2, "AA0201001", 100, new DateTime(2020, 12, 15), BonusStatusIdConst.Completed, false);

            yield return new BonusEntity(new DateTime(2020, 12, 8), 1008, "AA02", "AA0202", 3, "AA0202001", 100, null, BonusStatusIdConst.Pending, false);
            yield return new BonusEntity(new DateTime(2020, 12, 8), 1007, "AA02", "AA0202", 4, "AA0202002", 100, null, BonusStatusIdConst.Pending, false);

            yield return new BonusEntity(new DateTime(2020, 11, 9), 1006, "AA01", "AA0101", 1, "AA0101001", 50, new DateTime(2020, 11, 15), BonusStatusIdConst.Completed, true);
            yield return new BonusEntity(new DateTime(2020, 11, 9), 1005, "AA02", "AA0201", 2, "AA0201001", 50, null, BonusStatusIdConst.Cancelled, false);
            yield return new BonusEntity(new DateTime(2020, 11, 9), 1002, "AA02", "AA0202", 3, "AA0202001", 50, new DateTime(2020, 11, 16), BonusStatusIdConst.Completed, false);
            yield return new BonusEntity(new DateTime(2020, 11, 9), 1001, "AA02", "AA0202", 4, "AA0202002", 50, new DateTime(2020, 11, 15), BonusStatusIdConst.Completed, false);
        }
    }
}
