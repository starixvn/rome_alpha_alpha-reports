namespace Alpha.Reports.Site.Tests.Requests.RacingCommission
{
    using Fixtures;
    using Reports.Models;
    using Site.Requests.RacingCommission;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    [Collection("UserCollection")]
    public class RacingCommissionQueryTests
    {
        private readonly IUser user;

        public RacingCommissionQueryTests(UserFixture userFixture)
        {
            user = userFixture.User;
        }

        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var query = new RacingCommissionQuery(user)
            {
                SortPropertyName = "SortPropertyName",
                IsDescendingSort = true
            };

            // Assert
            Assert.True(
                query.SortPropertyName == "SortPropertyName"
                && query.IsDescendingSort == true);
        }
    }
}
