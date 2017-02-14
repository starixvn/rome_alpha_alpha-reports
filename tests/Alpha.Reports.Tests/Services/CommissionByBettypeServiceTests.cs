namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
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
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetAllCommissionByBettypes_When_CustIdLessOrEqualZero_Returns_EmptyData(int custId)
        {
            // Arrange
            var param = new CommissionByBettypeParam(UserLevel.Agent, UserLevel.Master, custId, default(DateTime), default(DateTime), default(DateTime));
            var service = new CommissionByBettypeServiceImpl(null);

            // Act
            var commissions = service.GetAllCommissionByBettypes(param);

            // Assert
            Assert.Empty(commissions);
        }

        [Fact]
        public void GetAllCommissionByBettypes_When_UserLevelLessThanCustLevel_Returns_EmptyData()
        {
            // Arrange
            var param = new CommissionByBettypeParam(UserLevel.Agent, UserLevel.Master, 0, default(DateTime), default(DateTime), default(DateTime));
            var service = new CommissionByBettypeServiceImpl(null);

            // Act
            var commissions = service.GetAllCommissionByBettypes(param);

            // Assert
            Assert.Empty(commissions);
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

            var dateService = Substitute.For<ReportDateService>();
            dateService.GetMoveBetsDay().Returns(new DateTime());
            var repository = Substitute.For<IDynamicRepository>();
            repository.Fetch<CommissionByBettypeModel>(Arg.Any<CommissionByBettypeCriteria>()).Returns(new List<CommissionByBettypeModel> { null });

            var service = new CommissionByBettypeServiceImpl(repository);

            // Act
            var commissions = service.GetAllCommissionByBettypes(param);

            // Assert
            Assert.NotEmpty(commissions);
        }
    }
}
