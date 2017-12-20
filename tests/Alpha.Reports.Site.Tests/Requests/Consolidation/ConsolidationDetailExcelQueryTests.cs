namespace Alpha.Reports.Site.Tests.Requests.Consolidation
{
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Reports.Models;
    using Site.Requests.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("UserCollection")]
    public class ConsolidationDetailExcelQueryTests
    {
        private readonly IUser user;

        public ConsolidationDetailExcelQueryTests(UserFixture userFixture)
        {
            user = userFixture.User;
        }

        [Fact]
        public void IsValid_InvalidCustId_ReturnsFalse()
        {
            // Arrange
            var query = new ConsolidationDetailExcelQuery(user) { CustId = 0 };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_EmptyCustName_ReturnsFalse()
        {
            // Arrange
            var query = new ConsolidationDetailExcelQuery(user) { CustId = 1, CustName = string.Empty };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_EmptyLanguage_ReturnsFalse()
        {
            // Arrange
            var currentUser = Substitute.For<IUser>();
            currentUser.Language.Returns(string.Empty);
            currentUser.LevelId.Returns((byte)4);
            var query = new ConsolidationDetailExcelQuery(currentUser) { CustId = 1, CustName = "CustName" };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var currentUser = Substitute.For<IUser>();
            currentUser.Language.Returns("en");
            currentUser.LevelId.Returns((byte)4);
            var query = new ConsolidationDetailExcelQuery(currentUser)
            {
                CustId = 1,
                CustName = "CustName",
                FromDate = DefaultVariablesTest.CurrentDate.AddDays(1),
                ToDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidQuery_ReturnsFalse()
        {
            // Arrange
            var currentUser = Substitute.For<IUser>();
            currentUser.Language.Returns("en");
            currentUser.LevelId.Returns((byte)4);
            var query = new ConsolidationDetailExcelQuery(currentUser)
            {
                CustId = 1,
                CustName = "CustName",
                FromDate = DefaultVariablesTest.CurrentDate,
                ToDate = DefaultVariablesTest.CurrentDate
            };

            // Act
            var actualIsValid = query.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
