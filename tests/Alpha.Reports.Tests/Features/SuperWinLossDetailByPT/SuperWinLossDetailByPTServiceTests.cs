namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT
{
    using System;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Criteria;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Params;
    using Alpha.Reports.Services;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailByPTServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService refService;
        private readonly PartialUMService partialUMService;
        private readonly SuperWinLossDetailByPTService service;

        public SuperWinLossDetailByPTServiceTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            reportDateService = Substitute.For<ReportDateService>();
            refService = Substitute.For<CacheRefService>();
            partialUMService = Substitute.For<PartialUMService>();
            service = new SuperWinLossDetailByPTServiceImpl(repository, reportDateService, refService, partialUMService);
        }

        [Fact]
        public void GetSuperWinLossDetailByProducts_Always_CallCheckPartialUM()
        {
            // Arrange
            var param = new SuperWinLossDetailByPTParam(
                123456,
                DateTime.Now,
                DateTime.Now,
                new[] { 1 },
                new[] { 1 },
                string.Empty,
                DateTime.Now);

            // Act
            service.GetSuperWinLossDetailByProducts(param);

            // Assert
            partialUMService.Received().CheckPartialUM("Age_Rpt_WinLossDetailByPT_Super");
        }

        [Fact]
        public void GetSuperWinLossDetailByProducts_Always_CallFetch()
        {
            // Arrange
            var param = new SuperWinLossDetailByPTParam(
                123456,
                DateTime.Now,
                DateTime.Now,
                new[] { 1 },
                new[] { 1 },
                string.Empty,
                DateTime.Now);

            // Act
            service.GetSuperWinLossDetailByProducts(param);

            // Assert
            repository
                .Received()
                .Fetch<SuperWinLossDetailByProductModel>(Arg.Any<SuperWinLossDetailByPTCriteria>());
        }
    }
}
