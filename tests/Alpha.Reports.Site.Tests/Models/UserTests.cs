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
            userSession.LangKey.Returns("en");
            userSession.Currency.Returns("CurrencyName");
            userSession.IsShowConsolidatedReport.Returns(true);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.True(
                user.Id == 1
                && user.Name == "username"
                && user.LevelId == 2
                && user.Language == "en-US"
                && user.LanguageKey == "en"
                && user.IsShowConsolidatedReport.ToString() == "True"
                && user.CurrencyName == "CurrencyName");
        }

        [Fact]
        public void GetHideProduct_Always_ReturnsCorrectStatus()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.GetHideProduct(Arg.Any<int>()).Returns(true);
            var user = new User(userSession);
            var productId = 1;
            var expectedHide = true;

            // Act
            var actualHide = user.GetHideProduct(productId);

            // Assert
            Assert.Equal(expectedHide, actualHide);
        }

        [Fact]
        public void IsEnabledJackpot_JackpotEnabled_ReturnTrue()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.IsCasinoSync = true;
            var user = new UserForTest(userSession);

            // Act
            var value = user.IsEnabledJackpot;

            // Assert
            Assert.True(value);
        }
    }
}
