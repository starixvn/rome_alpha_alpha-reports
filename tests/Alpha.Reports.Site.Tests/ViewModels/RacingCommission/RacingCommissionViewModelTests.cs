namespace Alpha.Reports.Site.Tests.ViewModels.RacingCommission
{
    using System;
    using System.Collections.Generic;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.RacingCommission;
    using Site.Helpers;
    using Site.Requests.RacingCommission;
    using Site.ViewModels.RacingCommission;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionViewModelTests
    {
        [Fact]
        public void BuildDrillDownLink_WhenViewerLevelIsAgent_CallAddQueryStringParamsFuncToBuilBetListLink()
        {
            // Arrange
            SetUpForHelpers();
            UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var custId = 1;
            var custName = "CustName";

            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)1);

            var query = new RacingCommissionQuery(user);
            query.CustLevelId = 2;
            query.FromDate = DateTime.Parse("1/1/1111").Date;
            query.ToDate = DateTime.Parse("2/2/2222").Date;

            var viewmodel = new RacingCommissionViewModel(query, null, default(DateTime), null);

            // Act
            viewmodel.BuildDrillDownLink(new RacingCommissionModel { CustId = custId, CustName = custName }, null);

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "BetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == custId
                    && (string)routeValue["CustName"] == custName
                    && (string)routeValue["Type"] == "WLByDate"
                    && (DateTime)routeValue["FDate"] == query.FromDate
                    && (DateTime)routeValue["TDate"] == query.ToDate
                    && (string)routeValue["ProductIds"] == "2"
                    && (int)routeValue["FilterPostback"] == 1
                    && (int)routeValue["IsHistoryReport"] == 0));
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
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)1);

            var query = new RacingCommissionQuery(user);
            query.CustLevelId = custLevelId;

            var viewmodel = new RacingCommissionViewModel(query, null, default(DateTime), null);

            // Act
            viewmodel.BuildDrillDownLink(new RacingCommissionModel { CustId = custId, CustName = custName }, currentUrl);

            // Assert
            UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                currentUrl,
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                    (int)routeValue["CustId"] == custId
                    && (string)routeValue["CustName"] == custName
                    && (byte)routeValue["CustLevelId"] == query.CustLevelId - 1
                    && (string)routeValue["BreadcrumbLevelJson"] == "parsedJson"));
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