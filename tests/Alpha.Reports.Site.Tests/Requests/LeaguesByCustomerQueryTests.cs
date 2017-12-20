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
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void Create_WithUser_ReturnsQueryHavingData()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)1);

            var custId = anyValueCreatorFixture.Create<int>();
            user.Id.Returns(custId);

            var language = anyValueCreatorFixture.Create<string>();
            user.LanguageKey.Returns(language);

            // Act
            var actualQuery = new LeaguesByCustomerQuery(user);

            // Assert
            Assert.True(actualQuery.FromDate == DefaultVariablesTest.CurrentDate
               && actualQuery.ToDate == DefaultVariablesTest.CurrentDate
               && actualQuery.CustId == custId
               && actualQuery.LanguageKey == language);
        }
    }
}
