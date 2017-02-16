namespace Alpha.Reports.Site.Tests.Requests.WinLossDetail
{
    using NSubstitute;
    using Reports.Models;
    using Site.Requests.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailQueryTests
    {
        private readonly IUser user;

        public WinLossDetailQueryTests()
        {
            // this substitute make test slow
            user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);
        }

        [Fact]
        public void CustId_NotSetCustId_ReturnsCustIdOfUserSession()
        {
            // Arrange
            var expectedCustId = 1;
            user.Id.Returns(expectedCustId);
            var winLossDetailQuery = new WinLossDetailQuery(user);

            // Act
            var actualCustId = winLossDetailQuery.CustId;

            // Assert
            Assert.Equal(expectedCustId, actualCustId);
        }

        [Fact]
        public void CustName_NotSetCustId_ReturnsCustNameOfUserSession()
        {
            // Arrange
            var expectedCustName = "CustName";
            user.Name.Returns(expectedCustName);
            var winLossDetailQuery = new WinLossDetailQuery(user);

            // Act
            var actualCustName = winLossDetailQuery.CustName;

            // Assert
            Assert.Equal(expectedCustName, actualCustName);
        }

        [Fact]
        public void UserLevelId_NotSetCustId_ReturnsRoleIdOfUserSession()
        {
            // Arrange
            var expectedCustLevelId = (byte)4;
            user.LevelId.Returns(expectedCustLevelId);
            var winLossDetailQuery = new WinLossDetailQuery(user);

            // Act
            var actualCustLevelId = winLossDetailQuery.CustLevelId;

            // Assert
            Assert.Equal(expectedCustLevelId, actualCustLevelId);
        }
    }
}