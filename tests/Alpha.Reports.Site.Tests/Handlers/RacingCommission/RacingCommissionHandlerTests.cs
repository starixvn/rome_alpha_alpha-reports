namespace Alpha.Reports.Site.Tests.Handlers.RacingCommission
{
    using System;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Services;
    using Site.Handlers.RacingCommission;
    using Site.Requests.RacingCommission;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionHandlerTests
    {
        [Fact]
        public void Handle_WhenCalled_Pass_Service_With_Correct_Parameters()
        {
            // Arrange
            var viewerLevel = UserLevel.Master;
            var custLevel = UserLevel.Agent;
            var custId = 0;
            var fromDate = DateTime.Parse("1111/1/1");
            var toDate = DateTime.Parse("2222/2/2");

            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)1);
            var query = Substitute.For<RacingCommissionQuery>(user);
            query.ViewerLevel.Returns(viewerLevel);
            query.CustLevel.Returns(custLevel);
            query.CustId = custId;
            query.FromDate = fromDate;
            query.ToDate = toDate;

            var service = Substitute.For<RacingCommissionService>();
            var handler = new RacingCommissionHandler(service, Substitute.For<ReportDateService>());

            var expectedparam = new RacingCommissionParam(viewerLevel, custLevel, custId, fromDate, toDate);

            // Act
            handler.Handle(query);

            // Assert
            service.Received().GetCustomerCommission(Arg.Is<RacingCommissionParam>(n => new CompareLogic().Compare(n, expectedparam).AreEqual));
        }
    }
}
