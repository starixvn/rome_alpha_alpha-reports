namespace Alpha.Reports.Tests.Services
{
    using System;
    using Reports.Models;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionServiceImplTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetCustomerCommission_WhenCustIdLessOrEqualZero_ReturnsEmptyData(int custId)
        {
            // Arrange
            var param = new RacingCommissionParam(UserLevel.Agent, UserLevel.Master, custId, default(DateTime), default(DateTime));
            var service = new RacingCommissionServiceImpl();

            // Act
            var commissions = service.GetCustomerCommission(param);

            // Assert
            Assert.Empty(commissions);
        }

        [Fact]
        public void GetCustomerCommission_WhenUserLevelLessThanCustLevel_ReturnsEmptyData()
        {
            // Arrange
            var param = new RacingCommissionParam(UserLevel.Agent, UserLevel.Master, 0, default(DateTime), default(DateTime));
            var service = new RacingCommissionServiceImpl();

            // Act
            var commissions = service.GetCustomerCommission(param);

            // Assert
            Assert.Empty(commissions);
        }
    }
}
