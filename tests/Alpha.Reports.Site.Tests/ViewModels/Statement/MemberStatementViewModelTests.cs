using System.Globalization;
using System.Threading;
using Alpha.Reports.Models;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.Tests.Constants;
using Alpha.Reports.Site.Tests.Fixtures;
using Alpha.Reports.Site.ViewModels.Statement;
using Alpha.Reports.Utils;
using Xunit;

namespace Alpha.Reports.Site.Tests.ViewModels.Statement
{
    [Trait("Category", "Statement")]
    [Collection("UserCollection")]
    public class MemberStatementViewModelTests
    {
        private readonly IUser user;

        public MemberStatementViewModelTests(UserFixture userFixture)
        {
            this.user = userFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void PageSize_Default_ReturnsCorrectValue()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            var expectedPageSize = 50;

            // Act
            var actualPageSize = viewModel.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Fact]
        public void PageSize_HasPageSize_ReturnsCorrectValue()
        {
            // Arrange
            var query = new MemberStatementQuery(user)
            {
                PageSize = 100
            };

            var viewModel = new MemberStatementViewModel(query, null);
            var expectedPageSize = 100;

            // Act
            var actualPageSize = viewModel.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Fact]
        public void PageIndex_Default_ReturnsCorrectValue()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            var expectedPageIndex = DefaultVariables.PageIndex;

            // Act
            var actualPageIndex = viewModel.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }

        [Fact]
        public void PageIndex_HasPageIndex_ReturnsCorrectValue()
        {
            // Arrange
            var query = new MemberStatementQuery(user)
            {
                PageIndex = 2
            };

            var viewModel = new MemberStatementViewModel(query, null);
            var expectedPageIndex = 2;

            // Act
            var actualPageIndex = viewModel.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }

        [Fact]
        public void IsNegative_NullValue_ReturnsFalse()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            decimal? cellValue = null;

            // Act
            var actualIsNegative = viewModel.IsNegative(cellValue);

            // Assert
            Assert.False(actualIsNegative);
        }

        [Fact]
        public void IsNegative_NegativeValue_ReturnsTrue()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            decimal? cellValue = -1;

            // Act
            var actualIsNegative = viewModel.IsNegative(cellValue);

            // Assert
            Assert.True(actualIsNegative);
        }

        [Fact]
        public void IsNegative_NotNegativeValue_ReturnsFalse()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            decimal? cellValue = 1;

            // Act
            var actualIsNegative = viewModel.IsNegative(cellValue);

            // Assert
            Assert.False(actualIsNegative);
        }

        [Fact]
        public void ViewDataInstructionMessage_Always_ReturnsCorrectValue()
        {
            // Arrange
            var query = new MemberStatementQuery(user);
            var viewModel = new MemberStatementViewModel(query, null);
            var expectedViewDataInstructionMessage = "You can view the report data from 01/01/2016.";

            // Act
            var actualViewDataInstructionMessage = viewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }
    }
}
