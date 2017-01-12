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
            var repository = GetFakeRepository(new List<OutstandingData>());
            var outstandingService = new OutstandingServiceImpl(repository);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(userLevel, custLevel, 1);

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
            var repository = GetFakeRepository(new List<OutstandingData> { null });
            var outstandingService = new OutstandingServiceImpl(repository);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(userLevel, custLevel, 1);

            // Assert
            Assert.True(outstandings.Any());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetMemberOutstanding_WhenCustIdLessOrEqualZero_ReturnsEmptyData(int custId)
        {
            // Arrange
            var outstandingService = new OutstandingServiceImpl(null);

            // Act
            var outstandings = outstandingService.GetMemberOutstanding(null, null, custId);

            // Assert
            Assert.True(!outstandings.Any());
        }

        private IDynamicRepository GetFakeRepository(IEnumerable<OutstandingData> outstandings)
        {
            var repository = Substitute.For<IDynamicRepository>();
            repository.Fetch<OutstandingData>(Arg.Any<OutstandingCriteria>()).Returns(outstandings);

            return repository;
        }
    }
}
