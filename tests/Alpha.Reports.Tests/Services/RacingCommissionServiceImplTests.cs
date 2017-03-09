namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria.RacingCommission;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.RacingCommission;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionServiceImplTests
    {
        [Theory]
        [InlineData(3, 1)]
        [InlineData(3, 3)]
        public void GetCustomerCommission_WhenUserLevelGreaterOrEqualCustLevel_ReturnsNonEmptyData(byte userLevelId, byte custLevelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var param = new RacingCommissionParam(userLevel, custLevel, 1, default(DateTime), default(DateTime));
            var repository = Substitute.For<IDynamicRepository>();
            repository.Fetch<RacingCommissionModel>(Arg.Any<RacingCommissionCriteria>()).Returns(new List<RacingCommissionModel> { null });
            var service = new RacingCommissionServiceImpl(repository);

            // Act
            var commissions = service.GetCustomerCommission(param);

            // Assert
            Assert.NotEmpty(commissions);
        }
    }
}
