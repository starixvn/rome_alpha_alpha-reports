namespace Alpha.Reports.Site.Tests.Requests.CommissionByBettype
{
    using Fixtures;
    using Reports.Models;
    using Site.Requests.CommissionByBettype;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    [Collection("UserCollection")]
    public class CommissionByBettypeQueryTests
    {
        private readonly IUser user;

        public CommissionByBettypeQueryTests(UserFixture userFixture)
        {
            user = userFixture.User;
        }

        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new CommissionByBettypeQuery(user)
            {
                SortPropertyName = "SortPropertyName",
                IsDescendingSort = true
            };

            // Assert
            Assert.True(
                criteria.SortPropertyName == "SortPropertyName"
                && criteria.IsDescendingSort == true);
        }
    }
}
