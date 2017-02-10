namespace Alpha.Reports.Site.Tests.Requests
{
    using System;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Reports.Models;
    using Site.Requests;
    using Utils;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("AnyValueCreatorCollection")]
    public class LeaguesByCustomerQueryTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public LeaguesByCustomerQueryTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            SystemTime.Today = DefaultVariables.CurrentDate;
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void Create_DefaultConstructor_ReturnsQueryNotHaveData()
        {
            // Act
            var actualQuery = new LeaguesByCustomerQuery();

            // Assert
            Assert.True(actualQuery.FromDate == DateTime.MinValue
                && actualQuery.ToDate == DateTime.MinValue
                && actualQuery.Products == null
                && actualQuery.Language == null);
        }

        [Fact]
        public void Create_WithUser_ReturnsQueryHavingData()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            var custId = anyValueCreatorFixture.Create<int>();
            user.Id.Returns(custId);

            var language = anyValueCreatorFixture.Create<string>();
            user.Language.Returns(language);

            // Act
            var actualQuery = new LeaguesByCustomerQuery(user);

            // Assert
            Assert.True(actualQuery.FromDate == DefaultVariables.CurrentDate
               && actualQuery.ToDate == DefaultVariables.CurrentDate
               && actualQuery.CustId == custId
               && actualQuery.Language == language);
        }
    }
}
