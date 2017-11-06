namespace Alpha.Reports.Site.Tests.Requests.Statement
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class MemberStatementExcelQueryTests
    {
        private readonly IUser user;

        public MemberStatementExcelQueryTests(UserFixture userFixture)
        {
            this.user = userFixture.User;
        }

        [Fact]
        public void PageIndex_Always_ReturnsDefaultValue()
        {
            // Arrange
            var query = new MemberStatementExcelQuery(user);
            var expectedPageIndex = DefaultVariablesTest.PageIndex;

            // Act
            var actualPageIndex = query.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }

        [Fact]
        public void PageSize_Always_ReturnsDefaultValue()
        {
            // Arrange
            var query = new MemberStatementExcelQuery(user);
            var expectedPageSize = DefaultVariablesTest.PageSize;

            // Act
            var actualPageSize = query.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }
    }
}
