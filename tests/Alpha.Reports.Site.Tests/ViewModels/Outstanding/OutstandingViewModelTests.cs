namespace Alpha.Reports.Site.Tests.ViewModels.Outstanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        }

        [Fact]
        public void BuildDrillDownLink_WhenViewerLevelIsAgent_CallAddQueryStringParamsFuncToBuilBetListLink()
        {
            // Arrange
            SetUpForHelpers();
            UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var custId = 1;
            var custName = "CustName";
            var userSelectedProductIds = "1,2,3";

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(Agent);
            query.CustLevel.Returns(Agent);
            query.UserSelectedProductIds = userSelectedProductIds;

            var viewmodel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

            // Act
            viewmodel.BuildDrillDownLink(new OutstandingModel { CustId = custId, CustName = custName }, null);

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "BetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == custId
                    && (string)routeValue["Username"] == custName
                    && (string)routeValue["Type"] == "RunByDate_Mem"
                    && (string)routeValue["Chkgetall"] == "on"
                    && (string)routeValue["ProductIds"] == userSelectedProductIds
                    && (int)routeValue["FilterPostback"] == 1));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void BuildDrillDownLink_WhenViewerLevelIsUplineOfAgent_CallReplaceOrAddQueryStringParamsFuncToBuildDrillDownLink(byte custLevelId)
        {
            // Arrange
            SetUpForHelpers();
            UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var custId = 1;
            var custName = "CustName";
            var currentUrl = "CurrentUrl";
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);

            var query = Substitute.For<OutstandingQuery>(userFixture.User);
            query.ViewerLevel.Returns(custLevel);
            query.CustLevel.Returns(custLevel);

            var viewmodel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

            // Act
            viewmodel.BuildDrillDownLink(new OutstandingModel { CustId = custId, CustName = custName }, currentUrl);

            // Assert
            UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

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
            var viewModel = new OutstandingViewModel(query, null, Enumerable.Empty<int>(), null, null);

            // Act
            var actual = viewModel.DisplayOutstandingLevels;

            // Assert
            Assert.Equal(expected, actual);
        }

        private static void SetUpForHelpers()
        {
            JsonHelper.ToJsonFunc = Substitute.For<Func<object, string>>();
            JsonHelper.ToJsonFunc.Invoke(Arg.Any<object>()).Returns("parsedJson");
            UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapExMainPath(Arg.Any<string>()).Returns("BetListPath");
        }
    }
}