namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Configurations;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
    using Reports.Constants;
    using Reports.Criteria.PlayForFun;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class PlayForFunServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly CacheRefService cacheRefService;
        private readonly IUser user;
        private readonly IDynamicRepository repository;
        private readonly PlayForFunServiceImpl service;

        public PlayForFunServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
            user = new UserFixture().User;

            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetProductName(Arg.Any<int>()).Returns(callInfo => "Product " + callInfo.Args()[0]);

            repository = Substitute.For<IDynamicRepository>();
            service = new PlayForFunServiceImpl(cacheRefService, user, repository);

            repository.Fetch<PlayForFunProduct>(Arg.Any<PlayForFunProductCriteria>()).Returns(new List<PlayForFunProduct>
            {
                new PlayForFunProduct(6, string.Empty, false),
                new PlayForFunProduct(22, string.Empty, false),
                new PlayForFunProduct(24, string.Empty, false)
            });
        }

        [Fact]
        public void SupportedProduct_HideFishingWorld_ReturnsCorrectList()
        {
            // Arrange
            var custId = anyValueCreatorFixture.Create<int>();
            user.GetHideProduct(ProductId.FishingWorld).Returns(true);
            var expectedProductList = new List<Product>
            {
                new Product(6, string.Empty,  cacheRefService.GetProductName),
                new Product(22, string.Empty,  cacheRefService.GetProductName)
            };

            // Act
            var actualProducts = service.SupportedProduct(custId);

            // Assert
            AssertExtension.Equal(expectedProductList, actualProducts);
        }

        [Fact]
        public void SupportedProduct_ShowFishingWorld_ReturnsCorrectList()
        {
            // Arrange
            var custId = anyValueCreatorFixture.Create<int>();
            user.GetHideProduct(ProductId.FishingWorld).Returns(false);
            var expectedProductList = new List<Product>
            {
                new Product(6, string.Empty,  cacheRefService.GetProductName),
                new Product(22, string.Empty,  cacheRefService.GetProductName),
                new Product(24, string.Empty,  cacheRefService.GetProductName)
            };

            // Act
            var actualProducts = service.SupportedProduct(custId);

            // Assert
            AssertExtension.Equal(expectedProductList, actualProducts);
        }

        [Fact]
        public void MasterPlayForFun_Always_ReturnsCorrectDataFromRepository()
        {
            // Arrange
            var param = anyValueCreatorFixture.Create<PlayForFunParam>();
            var data = new List<MasterAgentPlayForFunModel> { anyValueCreatorFixture.Create<MasterAgentPlayForFunModel>() };
            repository.Fetch<MasterAgentPlayForFunModel>(Arg.Any<MasterPlayForFunCriteria>())
                .Returns(data);
            var expectedData = data;

            // Act
            var actualData = service.MasterPlayForFun(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void AgentPlayForFun_Always_ReturnsCorrectDataFromRepository()
        {
            // Arrange
            var param = anyValueCreatorFixture.Create<PlayForFunParam>();
            var data = new List<MasterAgentPlayForFunModel> { anyValueCreatorFixture.Create<MasterAgentPlayForFunModel>() };
            repository.Fetch<MasterAgentPlayForFunModel>(Arg.Any<AgentPlayForFunCriteria>())
                .Returns(data);
            var expectedData = data;

            // Act
            var actualData = service.AgentPlayForFun(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void MemberPlayForFun_Always_ReturnsCorrectDataFromRepository()
        {
            // Arrange
            var param = anyValueCreatorFixture.Create<PlayForFunParam>();
            var data = new List<MemberPlayForFunModel> { anyValueCreatorFixture.Create<MemberPlayForFunModel>() };
            repository.Fetch<MemberPlayForFunModel>(Arg.Any<MemberPlayForFunCriteria>())
                .Returns(data);
            var expectedData = data;

            // Act
            var actualData = service.MemberPlayForFun(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}
