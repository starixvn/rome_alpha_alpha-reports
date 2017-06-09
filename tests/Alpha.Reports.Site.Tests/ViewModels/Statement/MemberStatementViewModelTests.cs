using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.Helpers;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.Tests.Constants;
using Alpha.Reports.Site.Tests.Fixtures;
using Alpha.Reports.Site.Tests.Helpers;
using Alpha.Reports.Site.ViewModels.Statement;
using Alpha.Reports.Utils;
using NSubstitute;
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
            user = userFixture.User;

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
            var expectedViewDataInstructionMessage = "You can view the report data from 09/01/2016.";

            // Act
            var actualViewDataInstructionMessage = viewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedViewDataInstructionMessage, actualViewDataInstructionMessage);
        }

        [Fact]
        public void BuildBetListLink_NotSupportProductIds_ReturnsCorrectLink()
        {
            // Arrange
            var message = new MemberStatementQuery(user)
            {
                CustName = "CustName",
                CustId = 1
            };

            var viewModel = new MemberStatementViewModel(message, Enumerable.Empty<StatementModel>());
            UrlHelperTestSetUp.SetUp();

            // Act
            viewModel.BuildBetListLink();

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == 1
                    && (string)routeValue["CustName"] == "CustName"
                    && (string)routeValue["FromDate"] == "12/01/2016"
                    && (string)routeValue["ToDate"] == "12/02/2016"
                    && (bool)routeValue["GetAll"] == true
                    && (string)routeValue["ProductIds"] == "1,6,2"));
        }

        [Fact]
        public void BuildBetListLink_SupportProductIds_ReturnsCorrectLink()
        {
            // Arrange
            var message = new MemberStatementQuery(user)
            {
                CustName = "CustName",
                CustId = 1,
                ProductIds = "1,2,3,4,5"
            };

            var viewModel = new MemberStatementViewModel(message, Enumerable.Empty<StatementModel>());
            UrlHelperTestSetUp.SetUp();

            // Act
            viewModel.BuildBetListLink();

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == 1
                    && (string)routeValue["CustName"] == "CustName"
                    && (string)routeValue["FromDate"] == "12/01/2016"
                    && (string)routeValue["ToDate"] == "12/02/2016"
                    && (bool)routeValue["GetAll"] == true
                    && (string)routeValue["ProductIds"] == "1,2,3,4,5"));
        }

        [Fact]
        public void BuildBetListRunningLink_Always_ReturnsCorrectLink()
        {
            // Arrange
            var message = new MemberStatementQuery(user)
            {
                CustName = "CustName",
                CustId = 1
            };

            var viewModel = new MemberStatementViewModel(message, Enumerable.Empty<StatementModel>());
            UrlHelperTestSetUp.SetUp();

            // Act
            viewModel.BuildBetListRunningLink();

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == 1
                    && (string)routeValue["CustName"] == "CustName"
                    && (string)routeValue["FromDate"] == "11/25/2016"
                    && (string)routeValue["ToDate"] == "12/02/2016"
                    && (bool)routeValue["GetAll"] == true
                    && (string)routeValue["ProductIds"] == "1,6,2"));
        }
    }
}