namespace Alpha.Reports.Site.Tests.Requests.Statement
{
    using System;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests.Statement;
    using Alpha.Reports.Site.Tests.Constants;
    using Alpha.Reports.Site.Tests.Fixtures;
    using Alpha.Reports.Utils;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class StatementDetailsQueryTests
    {
        private readonly IUser user;

        public StatementDetailsQueryTests(UserFixture userFixture)
        {
            this.user = userFixture.User;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void MinDate_Always_ReturnsCorrectDate()
        {
            // Arrange
            var query = new StatementDetailsQuery(user);
            var expectedMinDate = new DateTime(2013, 1, 1);

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void IsValid_TransferDateLessThanMinDate_ReturnsFalse()
        {
            // Arrange
            var query = new StatementDetailsQuery(user)
            {
                TransferDate = new DateTime(2012, 1, 1)
            };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidQuery_ReturnsTrue()
        {
            // Arrange
            var query = new StatementDetailsQuery(user)
            {
                TransferDate = new DateTime(2013, 1, 1)
            };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidCustInfo_ReturnsFalse()
        {
            // Arrange
            var fakeUser = Substitute.For<IUser>();
            fakeUser.LevelId.Returns((byte)1);

            var query = new StatementDetailsQuery(fakeUser)
            {
                TransferDate = new DateTime(2013, 1, 1)
            };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
