namespace Alpha.Reports.Site.Tests.ViewModels.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using NSubstitute;
    using Reports.Models;
    using Reports.Models.CommissionByBettype;
    using Site.Helpers;
    using Site.Requests.CommissionByBettype;
    using Site.ViewModels.CommissionByBettype;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    public class CommissionByBettypeViewModelTests
    {
        [Theory]
        [InlineData(true, false, 6)]
        [InlineData(false, true, 6)]
        [InlineData(false, false, 5)]
        public void Should_ReturnCorrectNumberOfColumns(bool showChargeCommColumn, bool showMyanmarOddsColumn, int exptectedValue)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)1);
            var query = new CommissionByBettypeQuery(user);
            var viewmodel = new CommissionByBettypeViewModel(query, null, null, default(DateTime), showChargeCommColumn, showMyanmarOddsColumn);

            // Act
            var result = viewmodel.GetNumCols();

            // Assert
            Assert.Equal(exptectedValue, result);
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

            var query = new CommissionByBettypeQuery(user);
            query.CustLevelId = custLevelId;

            var viewmodel = new CommissionByBettypeViewModel(query, null, null, default(DateTime), false, false);

            // Act
            viewmodel.BuildDrillDownLink(new CommissionByBettypeModel(custId, custName, 0, 0, 0, 0, 0, 0), currentUrl);

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
