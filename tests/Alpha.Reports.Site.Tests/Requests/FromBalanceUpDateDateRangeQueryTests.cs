namespace Alpha.Reports.Site.Tests.Requests
{
    using System;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Reports.Models;
    using Services;
    using Site.Requests;
    using Utils;
    using Xunit;

    [Trait("Category", "Site.Request")]
    [Collection("UserCollection")]
    public class FromBalanceUpDateDateRangeQueryTests
    {
        private readonly IUser user;

        public FromBalanceUpDateDateRangeQueryTests(UserFixture userFixture)
        {
            user = userFixture.User;
            SystemTime.Today = DefaultVariables.CurrentDate;

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);
        }

        [Fact]
        public void IsValid_FromDateLessThanBalanceUpDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = new DateTime(2016, 11, 27);
            var toDate = new DateTime(2017, 2, 6);

            var query = new FromBalanceUpDateDateRangeQuery(user);
            query.FromDate = fromDate;
            query.ToDate = toDate;

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var fromDate = new DateTime(2017, 2, 7);
            var toDate = new DateTime(2017, 2, 6);

            var query = new FromBalanceUpDateDateRangeQuery(user);
            query.FromDate = fromDate;
            query.ToDate = toDate;

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidQuery_ReturnsTrue()
        {
            // Arrange
            var fromDate = new DateTime(2017, 2, 7);
            var toDate = new DateTime(2017, 2, 8);

            var query = new FromBalanceUpDateDateRangeQuery(user);
            query.FromDate = fromDate;
            query.ToDate = toDate;

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
