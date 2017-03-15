namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria.MatchWinLossDetail;
    using Reports.DataAccesses;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossDetailServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly MatchWinLossDetailDataAccess dataAccess;
        private readonly MatchWinLossDetailServiceImpl matchWinLossDetailServiceImpl;

        public MatchWinLossDetailServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            reportDateService = Substitute.For<ReportDateService>();
            productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<MatchWinLossDetailDataAccess>();
            matchWinLossDetailServiceImpl = new MatchWinLossDetailServiceImpl(reportDateService, productService, dataAccess);
        }

        [Fact]
        public void GetMatchWinLossDetail_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossDetailParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossDetailServiceImpl.GetMatchWinLossDetail(nullParam));
        }

        [Theory]
        [InlineData("1,2,3,5", "1,2,3,5")]
        [InlineData("1,2,3,5,6", "1,2,3,5")]
        [InlineData("1,2,3,5,7,8", "1,2,3,5")]
        [InlineData("1,2,5", "1,2,5")]
        [InlineData("1", "1")]
        [InlineData("", "")]
        public void GetMatchWinLossDetail_ValidParam_CallsDynamicRepository(
            string productIds,
            string expectCalledProductIds)
        {
            // Arrange
            var param = new MatchWinLossDetailParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                anyValueCreatorFixture.Create<IEnumerable<int>>(),
                productIds.ToListInt(),
                anyValueCreatorFixture.Create<UserLevel>());

            var balanceUpDate = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetBalanceUpDay().Returns(balanceUpDate);

            var moveBetsDate = anyValueCreatorFixture.Create<DateTime>();
            reportDateService.GetMoveBetsDay().Returns(moveBetsDate);

            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(productIds.ToListInt());

            // Act
            matchWinLossDetailServiceImpl.GetMatchWinLossDetail(param);

            // Assert
            dataAccess.Received().GetData(Arg.Is<MatchWinLossDetailCriteria>(
                criteria => criteria.CustId == param.CustId
                && criteria.FromDate == param.FromDate
                && criteria.ToDate == param.ToDate
                && criteria.ProductIds == expectCalledProductIds
                && criteria.ReportLevel == param.ReportLevel
                && criteria.Leagues == param.UserSelectedLeagueIds.JoinWithComma()
                && criteria.MoveBetsDay == moveBetsDate));
        }

        [Fact]
        public void GetMatchWinLossDetail_ValidParam_ReturnsDataFromDynamicRepository()
        {
            var param = new MatchWinLossDetailParam(
                anyValueCreatorFixture.Create<int>(),
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                anyValueCreatorFixture.Create<IEnumerable<int>>(),
                anyValueCreatorFixture.Create<IEnumerable<int>>(),
                anyValueCreatorFixture.Create<UserLevel>());

            var matchWinLossDetailModel = new MatchWinLossDetailModel(false, 1, 1, 1, 1, false, 1, 1, string.Empty, false, string.Empty, 0, 0, null, null, null, null);
            var expectedMatchWinLossModels = new List<MatchWinLossDetailModel>
            {
                matchWinLossDetailModel
            };

            dataAccess.GetData(Arg.Any<MatchWinLossDetailCriteria>()).Returns(expectedMatchWinLossModels);

            // Act
            var actualMatchWinLossModels = matchWinLossDetailServiceImpl.GetMatchWinLossDetail(param);

            // Assert
            AssertExtension.Equal(expectedMatchWinLossModels, actualMatchWinLossModels);
        }

        [Fact]
        public void GetProductsForCustomer_Always_ReturnsFilteredProductsFromProductService()
        {
            // Arrange
            var expectedProducts = anyValueCreatorFixture.Create<IEnumerable<Product>>();

            productService.FilterProductsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(expectedProducts);

            var param = anyValueCreatorFixture.Create<MatchWinLossDetailParam>();

            // Act
            var actualProducts = matchWinLossDetailServiceImpl.GetProductsForCustomer(param.CustId);

            // Assert
            AssertExtension.Equal(expectedProducts, actualProducts);
        }
    }
}
