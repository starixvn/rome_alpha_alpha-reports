namespace Alpha.Reports.Site.Tests.ViewModels.Outstanding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Constants;
    using Enumerations;
    using Fixtures;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.Outstanding;
    using Site.Helpers;
    using Site.Requests.Outstanding;
    using Site.ViewModels.Outstanding;
    using Xunit;
    using static Reports.Models.UserLevel;

    [Trait("Category", "Outstanding")]
    [Collection("UserCollection")]
    public class OutstandingViewModelTests
    {
        private readonly UserFixture userFixture;

        public OutstandingViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void BuildDrillDownLink_WhenViewerLevelIsAgent_CallAddQueryStringParamsFuncToBuilBetListLink()
        {
            // Arrange
            SetUpForHelpers();
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var custId = 1;
            var custName = "CustName";
            var userSelectedProductIds = "1,2,3";

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Agent);
            query.CustLevel.Returns(Agent);
            query.UserSelectedProductIds = userSelectedProductIds;

            var viewmodel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            viewmodel.BuildDrillDownLink(new OutstandingModel(custId, custName, 0, 0, 0, 0, false), "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == custId
                    && (string)routeValue["CustName"] == custName
                    && (bool)routeValue["GetAll"] == true
                    && (string)routeValue["ProductIds"] == string.Empty));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void BuildDrillDownLink_WhenViewerLevelIsUplineOfAgent_CallReplaceOrAddQueryStringParamsFuncToBuildDrillDownLink(byte custLevelId)
        {
            // Arrange
            SetUpForHelpers();
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var custId = 1;
            var custName = "CustName";
            var currentUrl = "CurrentUrl";
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(custLevel);
            query.CustLevel.Returns(custLevel);

            var viewmodel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            viewmodel.BuildDrillDownLink(new OutstandingModel(custId, custName, 0, 0, 0, 0, false), currentUrl);

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                currentUrl,
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == custId
                    && (string)routeValue["CustName"] == custName
                    && (string)routeValue["UserSelectedProductIds"] == string.Empty
                    && (byte)routeValue["CustLevelId"] == custLevelId - 1
                    && (string)routeValue["BreadcrumbLevelJson"] == "parsedJson"));
        }

        [Fact]
        public void DisplayOutstandingLevels_SuperViewMasterOutstanding_Show_Member_Super_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Super);
            query.CustLevel.Returns(Super);

            var expected = new List<UserLevel> { Member, Super };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_SuperViewAgentOutstanding_Show_Member_Master_Super_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Super);
            query.CustLevel.Returns(Master);

            var expected = new List<UserLevel> { Member, Master, Super };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_SuperViewMemberOutstanding_Show_Member_Agent_Master_Super_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Super);
            query.CustLevel.Returns(Agent);
            var expected = new List<UserLevel> { Member, Agent, Master, Super };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_MasterViewAgentOutstanding_Show_Member_Master_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Master);
            query.CustLevel.Returns(Master);
            var expected = new List<UserLevel> { Member, Master };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_MasterViewMemberOutstanding_Show_Member_Agent_Master_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Master);
            query.CustLevel.Returns(Agent);
            var expected = new List<UserLevel> { Member, Agent, Master };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayOutstandingLevels_AgentViewMemberOutstanding_Show_Member_Agent_Outstanding()
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Agent);
            query.CustLevel.Returns(Agent);
            var expected = new List<UserLevel> { Member, Agent };
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4, "Master Outstanding")]
        [InlineData(3, "Agent Outstanding")]
        [InlineData(2, "Member Outstanding")]
        public void Title_Always_ReturnsCorrectTitle(byte userLevelId, string expectedTitle)
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            var userLevel = Enumeration.FromValue<UserLevel>(userLevelId);
            query.ViewerLevel.Returns(userLevel);
            query.CustLevel.Returns(userLevel);

            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetSortedProperty_NullOrWhiteSpaceProperty_ReturnsDefaultValue(string sortPropertyName)
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            var userLevel = Enumeration.FromValue<UserLevel>(2);
            query.ViewerLevel.Returns(userLevel);
            query.CustLevel.Returns(userLevel);
            query.SortPropertyName = sortPropertyName;

            var outstandingDataSource = new List<OutstandingModel>
            {
                new OutstandingModel(),
                new OutstandingModel()
            };

            var expectedSortPropertyName = "CustName";
            var viewModel = new OutstandingViewModel(query, outstandingDataSource, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actualSortPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortPropertyName, actualSortPropertyName);
        }

        [Theory]
        [InlineData("SuperOutstanding", 2, "CustName")]
        [InlineData("MemberOutstanding", 2, "MemberOutstanding")]
        public void GetSortedProperty_NotNullOrWhiteSpaceProperty_ReturnsCorrectValue(
            string sortPropertyName,
            byte custLevelId,
            string expectedSortPropertyName)
        {
            // Arrange
            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            query.ViewerLevel.Returns(userLevel);
            query.CustLevel.Returns(userLevel);
            query.SortPropertyName = sortPropertyName;

            var outstandingDataSource = new List<OutstandingModel>
            {
                new OutstandingModel(),
                new OutstandingModel()
            };

            var viewModel = new OutstandingViewModel(query, outstandingDataSource, Enumerable.Empty<int>(), Enumerable.Empty<GroupProductViewModel>(), null, false);

            // Act
            var actualSortPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortPropertyName, actualSortPropertyName);
        }

        private static void SetUpForHelpers()
        {
            JsonHelper.ToJsonFunc = Substitute.For<Func<object, string>>();
            JsonHelper.ToJsonFunc.Invoke(Arg.Any<object>()).Returns("parsedJson");
            Site.Helpers.UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapBetListPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapExMainPath(Arg.Any<string>()).Returns("BetListPath");
            Site.Helpers.UrlHelper.MapBetListPath(Arg.Any<string>()).Returns("NewBetListPath");
        }
    }
}