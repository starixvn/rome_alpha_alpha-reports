namespace Alpha.Reports.Tests.Services
{
    using System.Threading.Tasks;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Tests.Extensions;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    public class CustomerServiceImplTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly CustomerService customerService;

        public CustomerServiceImplTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            customerService = new CustomerServiceImpl(dynamicRepository);
        }

        [Fact]
        public async Task GetCustomerLevels_Super_ReturnsEmptyData()
        {
            // Arrange
            CustomerLevel rawdata = null;
            dynamicRepository
                .GetAsync<CustomerLevel>(Arg.Any<GetCustomerLevelCriteria>())
                .Returns(rawdata);

            // Act
            var actualCustomerLevels = await customerService.GetCustomerLevels("super", 123456);

            // Assert
            Assert.Empty(actualCustomerLevels);
        }

        [Fact]
        public async Task GetCustomerLevels_Master_ReturnsCorrectData()
        {
            // Arrange
            var rawdata = new CustomerLevel(null, null, null, null, 3, "master", 4, "super");
            dynamicRepository
                .GetAsync<CustomerLevel>(Arg.Any<GetCustomerLevelCriteria>())
                .Returns(rawdata);
            var expectedData = new[]
            {
                new CustInfo(4, "super", 4),
                new CustInfo(3, "master", 3)
            };

            // Act
            var actualData = await customerService.GetCustomerLevels("master", 4);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetCustomerLevels_Agent_ReturnsCorrectData()
        {
            // Arrange
            var rawdata = new CustomerLevel(null, null, 2, "agent", 3, "master", 4, "super");
            dynamicRepository
                .GetAsync<CustomerLevel>(Arg.Any<GetCustomerLevelCriteria>())
                .Returns(rawdata);
            var expectedData = new[]
            {
                new CustInfo(4, "super", 4),
                new CustInfo(3, "master", 3),
                new CustInfo(2, "agent", 2)
            };

            // Act
            var actualData = await customerService.GetCustomerLevels("agent", 4);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task GetCustomerLevels_Member_ReturnsCorrectData()
        {
            // Arrange
            var rawdata = new CustomerLevel(1, "member", 2, "Agent", 3, "master", 4, "super");
            dynamicRepository
                .GetAsync<CustomerLevel>(Arg.Any<GetCustomerLevelCriteria>())
                .Returns(rawdata);
            var expectedData = new[]
            {
                new CustInfo(4, "super", 4),
                new CustInfo(3, "master", 3),
                new CustInfo(2, "Agent", 2),
                new CustInfo(1, "member", 1)
            };

            // Act
            var actualData = await customerService.GetCustomerLevels("member", 4);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}
