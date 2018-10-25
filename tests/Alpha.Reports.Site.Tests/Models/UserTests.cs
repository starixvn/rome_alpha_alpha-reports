namespace Alpha.Reports.Site.Tests.Models
{
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void LimitedDateToShowInfo_Always_ReturnCorrectValue()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            var limitDate = new DateTime(2018, 9, 7);
            userSession.GetProperty<DateTime>("CustomerDateLimit").Returns(limitDate);

            var user = new UserForTest(userSession);

            // Act
            var actualValue = user.LimitedDateToShowInfo;

            // Assert
            Assert.Equal(limitDate, actualValue);
        }

        [Fact]
        public void GetEnableProductsForGame_Always_ReturnCorrectValue()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            var products = new List<int> { 6 };
            var user = new UserForTest(userSession);

            // Act
            var actualValue = user.GetEnableProductsForGame("Jackpot");

            // Assert
            Assert.Equal(products, actualValue);
        }

        [Fact]
        public void GetEnableProductsForGame_Always_NotNull()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            IList<int> productIds = null;
            userSession.GetProperty<IList<int>>(Arg.Any<string>()).Returns(productIds);
            var products = new List<int>();
            var user = new User(userSession);

            // Act
            var actualValue = user.GetEnableProductsForGame("Hello World");

            // Assert
            Assert.Equal(products, actualValue);
        }
    }
}
