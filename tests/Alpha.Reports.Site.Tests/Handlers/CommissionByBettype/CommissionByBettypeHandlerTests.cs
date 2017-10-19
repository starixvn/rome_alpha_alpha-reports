namespace Alpha.Reports.Site.Tests.Handlers.CommissionByBettype
{
    using System;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
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

        public CommissionByBettypeHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
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

            var service = Substitute.For<CommissionByBettypeService>();
            var reportService = Substitute.For<ReportDateService>();
            reportService.GetMoveBetsDay().Returns(moveBetsDay);
            var productService = Substitute.For<ProductService>();
            var handler = new CommissionByBettypeHandler(service, reportService, Substitute.For<UserPermissionService>(), productService);

            var expectedparam = new CommissionByBettypeParam(viewerLevel, custLevel, custId, fromDate, toDate, moveBetsDay);

            // Act
            handler.Handle(query);

            // Assert
            service.Received().GetAllCommissionByBettypes(Arg.Is<CommissionByBettypeParam>(n => new CompareLogic().Compare(n, expectedparam).AreEqual));
        }
    }
}
