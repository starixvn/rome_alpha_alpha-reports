namespace Alpha.Reports.Site.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.User.Models;
    using NSubstitute;
    using Site.Models;
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

        [Theory]
        [InlineData("MBC")]
        [InlineData("IBCIndo")]
        [InlineData("789y")]
        public void Site_Always_ReturnCorrectValue(string site)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.Site.Returns(site);
            var user = new User(userSession);

            // Act
            var actual = user.Site;

            // Assert
            Assert.True(actual == site);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CurrencyId_Always_ReturnCorrectValue(int currencyId)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.CurrencyId.Returns(currencyId);
            var user = new User(userSession);

            // Act
            var actual = user.CurrencyId;

            // Assert
            Assert.True(actual == currencyId);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsAdminCanViewVNIP_Always_ReturnCorrectValue(bool allowed)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.IsAdminCanViewVNIP.Returns(allowed);
            var user = new User(userSession);

            // Act
            var actual = user.IsAdminCanViewVNIP;

            // Assert
            Assert.True(actual == allowed);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void SubAccId_Always_ReturnCorrectValue(int subAccId)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.SubAccId.Returns(subAccId);
            var user = new User(userSession);

            // Act
            var actual = user.SubAccId;

            // Assert
            Assert.True(actual == subAccId);
        }

        [Theory]
        [InlineData("127.0.0.1")]
        [InlineData("142.10.15.16")]
        public void IP_Always_ReturnCorrectValue(string ip)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.UserLoginIp.Returns(ip);
            var user = new User(userSession);

            // Act
            var actual = user.IP;

            // Assert
            Assert.True(actual == ip);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsInternal_Always_ReturnCorrectValue(bool isInternal)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.IsInternal.Returns(isInternal);
            var user = new User(userSession);

            // Act
            var actual = user.IsInternal;

            // Assert
            Assert.True(actual == isInternal);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsProtectedMode_Always_ReturnCorrectValue(bool isProtectedMode)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.IsProtectedMode.Returns(isProtectedMode);
            var user = new User(userSession);

            // Act
            var actual = user.IsProtectedMode;

            // Assert
            Assert.True(actual == isProtectedMode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("-dark")]
        public void DarkModeName_Always_ReturnCorrectValue(string darkModeName)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.DarkModeName.Returns(darkModeName);
            var user = new User(userSession);

            // Act
            var actual = user.DarkModeName;

            // Assert
            Assert.True(actual == darkModeName);
        }

        [Theory]
        [InlineData("localhost")]
        [InlineData("nexdev.net")]
        public void Domain_Always_ReturnCorrectValue(string domain)
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.GetProperty<string>("MainDomain").Returns(domain);
            var user = new User(userSession);

            // Act
            var actual = user.MainDomain;

            // Assert
            Assert.True(actual == domain);
        }
    }
}
