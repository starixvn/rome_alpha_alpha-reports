namespace Alpha.Reports.Site.Tests.Models
{
    using NSubstitute;
    using Site.Models;
    using Sunplus.Agent.Model.Entities;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void Constructor_Always_GetUserDataFromSession()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.UserName.Returns("username");
            userSession.RoleId.Returns((byte)2);
            userSession.Language.Returns("en-US");

            // Act
            var user = new User(userSession);

            // Assert
            Assert.True(
                user.Id == 1
                && user.Name == "username"
                && user.LevelId == 2
                && user.Language == "en-US");
        }
    }
}