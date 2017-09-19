namespace Alpha.Reports.Site.Tests.Requests.Statement
{
    using System;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class MemberStatementQueryTests
    {
        private readonly IUser user;

        public MemberStatementQueryTests(UserFixture userFixture)
        {
            this.user = userFixture.User;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void IsFilterVisible_DefaultValue_ReturnsTrue()
        {
            // Arrange
            var query = new MemberStatementQuery(user);

            // Act
            var actualIsFilterVisible = query.IsFilterVisible;

            // Assert
            Assert.True(actualIsFilterVisible);
        }

        [Fact]
        public void FromDate_DefaultValue_ReturnsYesterday()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var expectedFromDate = new DateTime(2016, 12, 1);

            // Act
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }
    }
}
