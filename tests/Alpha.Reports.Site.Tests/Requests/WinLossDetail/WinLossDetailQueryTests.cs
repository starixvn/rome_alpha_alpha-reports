namespace Alpha.Reports.Site.Tests.Requests.WinLossDetail
{
    using NSubstitute;
    using Reports.Models;
    using Site.Requests.WinLossDetail;
    using Xunit;

    public class WinLossDetailQueryTests
    {
        private readonly IUser user;

        public WinLossDetailQueryTests()
        {
            user = Substitute.For<IUser>();
        }

        [Fact]
        public void CustId_NotSetCustId_ReturnsCustIdOfUserSession()
        {
            // Arrange
            var expectedCustId = 1;
            user.CustId.Returns(expectedCustId);
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
            user.CustName.Returns(expectedCustName);
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
            var expectedRoleId = (byte)4;
            user.UserLevelId.Returns(expectedRoleId);
            var winLossDetailQuery = new WinLossDetailQuery(user);

            // Act
            var actualRoleId = winLossDetailQuery.UserLevelId;

            // Assert
            Assert.Equal(expectedRoleId, actualRoleId);
        }
    }
}