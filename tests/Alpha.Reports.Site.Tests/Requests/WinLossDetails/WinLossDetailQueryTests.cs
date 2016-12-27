namespace Alpha.Reports.Site.Tests.Requests.WinLossDetails
{
    using Adapters;
    using Extensions;
    using NSubstitute;
    using Site.Helpers;
    using Site.Requests.WinLossDetails;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class WinLossDetailQueryTests
    {
        private readonly IUserSession userSession;

        public WinLossDetailQueryTests()
        {
            userSession = Substitute.For<IUserSession>();
        }

        [Fact]
        public void CustId_NotSetCustId_ReturnsCustIdOfUserSession()
        {
            // Arrange
            var expectedCustId = 1;
            userSession.CustId.Returns(expectedCustId);
            var winLossDetailQuery = new WinLossDetailQuery(userSession);

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
            userSession.UserName.Returns(expectedCustName);
            var winLossDetailQuery = new WinLossDetailQuery(userSession);

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
            userSession.RoleId.Returns(expectedRoleId);
            var winLossDetailQuery = new WinLossDetailQuery(userSession);

            // Act
            var actualRoleId = winLossDetailQuery.UserLevelId;

            // Assert
            Assert.Equal(expectedRoleId, actualRoleId);
        }
    }
}
