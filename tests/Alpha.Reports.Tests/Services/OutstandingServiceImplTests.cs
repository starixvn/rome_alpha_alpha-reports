namespace Alpha.Reports.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria.Outstanding;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Reports.Params;
    using Reports.Services.Outstanding;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingServiceImplTests
    {
        [Fact]
        public void GetMemberOutstanding_WhenUserLevelLessThanCustLevel_ReturnsEmptyData()
        {
            // Arrange
            UserLevel userLevel = UserLevel.Agent;
            UserLevel custLevel = UserLevel.Master;
            var repository = GetFakeRepository(new List<OutstandingModel>());
            var param = new OutstandingParam(userLevel, custLevel, 1, null);
            var outstandingService = new OutstandingServiceImpl(repository);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(param);

            // Assert
            Assert.True(!outstandings.Any());
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(3, 3)]
        public void GetMemberOutstanding_WhenUserLevelGreaterOrEqualCustLevel_ReturnsNonEmptyData(byte userLevelId, byte custLevelId)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var repository = GetFakeRepository(new List<OutstandingModel> { null });
            var param = new OutstandingParam(userLevel, custLevel, 1, null);
            var outstandingService = new OutstandingServiceImpl(repository);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(param);

            // Assert
            Assert.True(outstandings.Any());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetMemberOutstanding_WhenCustIdLessOrEqualZero_ReturnsEmptyData(int custId)
        {
            // Arrange
            var param = new OutstandingParam(null, null, custId, null);
            var outstandingService = new OutstandingServiceImpl(null);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(param);

            // Assert
            Assert.True(!outstandings.Any());
        }

        private IDynamicRepository GetFakeRepository(IEnumerable<OutstandingModel> outstandings)
        {
            var repository = Substitute.For<IDynamicRepository>();
            repository.Fetch<OutstandingModel>(Arg.Any<OutstandingCriteria>()).Returns(outstandings);

            return repository;
        }
    }
}
