namespace Alpha.Reports.Site.Tests.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Components;
    using Alpha.Web.Helpers;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Services;
    using Services.Outstanding;
    using Site.Handlers.Outstanding;
    using Site.Requests.Outstanding;
    using Site.ViewModels.Outstanding;
    using Xunit;

    [Trait("Category", "Outstanding")]
    [Collection("UserCollection")]
    public class OutstandingHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly ProductService productService;
        private readonly OutstandingService service;
        private readonly OutstandingHandler handler;
        private readonly BrowserHelper browserHelper;

        public OutstandingHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            service = Substitute.For<OutstandingService>();
            browserHelper = Substitute.For<BrowserHelper>();
            productService = Substitute.For<ProductService>();
            handler = new OutstandingHandler(service, productService, browserHelper, userFixture.User);
        }

        [Fact]
        public void Handle_Call_OustandingService_With_CorrectParameter()
        {
            // Arrange
            var viewerLevel = UserLevel.Master;
            var custLevel = UserLevel.Agent;
            var custId = 0;
            var userSelectedProductIds = string.Empty;

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(viewerLevel);
            query.CustLevel.Returns(custLevel);
            query.CustId = custId;
            query.UserSelectedProductIds = userSelectedProductIds;

            var expectedparam = new OutstandingParam(viewerLevel, custLevel, custId, userSelectedProductIds, false);

            // Act
            handler.Handle(query);

            // Assert
            service.Received().GetMemberOutstanding(Arg.Is<OutstandingParam>(n => new CompareLogic().Compare(n, expectedparam).AreEqual));
        }

        [Fact]
        public void Handle_ReturnCorrectViewModel()
        {
            // Arrange
            var viewerLevel = UserLevel.Master;
            var custLevel = UserLevel.Agent;
            var custId = 0;
            var custName = "CustName";
            var custLevelId = (byte)1;
            var userSelectedProductIds = string.Empty;

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(viewerLevel);
            query.CustLevel.Returns(custLevel);
            query.CustId = custId;
            query.UserSelectedProductIds = userSelectedProductIds;
            query.CustName = custName;
            query.CustLevelId = custLevelId;

            var outstandingService = Substitute.For<OutstandingService>();
            outstandingService.GetMemberOutstanding(Arg.Any<OutstandingParam>()).Returns(Enumerable.Empty<OutstandingModel>());

            var productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IOrderedEnumerable<int>>()).Returns(Enumerable.Empty<int>());
            productService.GetProductsForCustomer(1, "", 1);

            var handler = new OutstandingHandler(outstandingService, productService, browserHelper, userFixture.User);

            var expectedVm = new OutstandingViewModel(
                query,
                Enumerable.Empty<OutstandingModel>(),
                Enumerable.Empty<int>(),
                Enumerable.Empty<GroupProductViewModel>(),
                new List<CustInfo> { new CustInfo(custId, custName, custLevelId) },
                false);

            // Act
            var result = handler.Handle(query);

            // Assert
            Assert.True(new CompareLogic().Compare(expectedVm, result).AreEqual);
        }
    }
}