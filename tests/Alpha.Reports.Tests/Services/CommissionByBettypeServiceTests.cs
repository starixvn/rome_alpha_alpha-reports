namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria.CommissionByBettype;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.CommissionByBettype;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    public class CommissionByBettypeServiceTests
    {
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly IDynamicRepository repository;
        private readonly CommissionByBettypeService service;
        public CommissionByBettypeServiceTests()
        {
            reportDateService = Substitute.For<ReportDateService>();
            productService = Substitute.For<ProductService>();
            repository = Substitute.For<IDynamicRepository>();
            service = new CommissionByBettypeServiceImpl(repository, productService);
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(3, 3)]
        public void GetAllCommissionByBettypes_When_UserLevelGreaterOrEqualCustLevel_Returns_NonEmptyData(byte userLevelId, byte custLevelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var param = new CommissionByBettypeParam(userLevel, custLevel, 1, default(DateTime), default(DateTime), default(DateTime));
            reportDateService.GetMoveBetsDay().Returns(new DateTime());
            repository.Fetch<CommissionByBettypeModel>(Arg.Any<CommissionByBettypeCriteria>()).Returns(new List<CommissionByBettypeModel> { null });

            // Act
            var commissions = service.GetAllCommissionByBettypes(param);

            // Assert
            Assert.NotEmpty(commissions);
        }

        [Fact]
        public void GetProductsInOther_OfferBullFighting_ReturnOneProduct()
        {
            //// Arrange
            productService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product> { new Product(1, "abc", null), new Product(30, "xyz", null) });

            //// Act
            var act = service.GetProductsInOthers(Arg.Any<int>());

            //// Assert
            Assert.Equal(1, act.Count());
        }
    }
}
