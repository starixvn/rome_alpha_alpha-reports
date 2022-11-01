namespace Alpha.Reports.Site.Tests.Handlers.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models;
    using Services;
    using Site.Handlers.CommissionByBettype;
    using Site.Requests.CommissionByBettype;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    [Collection("UserCollection")]
    public class CommissionByBettypeHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly CommissionByBettypeService service;
        private readonly ReportDateService reportService;
        private readonly ProductService productService;
        private readonly UserPermissionService userPermissionService;
        private readonly CommissionByBettypeHandler handler;
        private readonly Fixture fixtureData = new Fixture();

        public CommissionByBettypeHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            service = Substitute.For<CommissionByBettypeService>();
            reportService = Substitute.For<ReportDateService>();
            productService = Substitute.For<ProductService>();
            userPermissionService = Substitute.For<UserPermissionService>();
            handler = new CommissionByBettypeHandler(service, reportService, userPermissionService, productService);
        }

        [Fact]
        public void Handle_WhenCalled_Pass_Service_With_Correct_Parameters()
        {
            // Arrange
            var viewerLevel = UserLevel.Master;
            var custLevel = UserLevel.Agent;
            var custId = 0;
            var fromDate = DateTime.Parse("1111/1/1");
            var toDate = DateTime.Parse("2222/2/2");
            var moveBetsDay = DateTime.Parse("1212/1/1");

            var query = Substitute.For<CommissionByBettypeQuery>(userFixture.User);
            query.ViewerLevel.Returns(viewerLevel);
            query.CustLevel.Returns(custLevel);
            query.CustId = custId;
            query.FromDate = fromDate;
            query.ToDate = toDate;
            reportService.GetMoveBetsDay().Returns(moveBetsDay);
            var expectedparam = new CommissionByBettypeParam(viewerLevel, custLevel, custId, fromDate, toDate, moveBetsDay);

            // Act
            handler.Handle(query);

            // Assert
            service.Received().GetAllCommissionByBettypes(Arg.Is<CommissionByBettypeParam>(n => new CompareLogic().Compare(n, expectedparam).AreEqual));
        }

        [Fact]
        public void CommissionByBetTypeGuideQuery_EmptyProductInOthers_ReturnOnlyNote()
        {
            //// Arrange
            var noteCount = 1;
            productService.GetProductsNeedDelayTimeToSumarizeDataByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(new List<byte> { 1 });
            productService.GetProductNames(Arg.Any<IEnumerable<int>>()).Returns(new List<string>() { "Product Name" });
            var query = new CommissionByBetTypeGuideQuery(userFixture.User);

            //// Act
            var act = handler.Handle(query);

            //// Assert
            Assert.Equal(noteCount, act.Count());
        }
    }
}
