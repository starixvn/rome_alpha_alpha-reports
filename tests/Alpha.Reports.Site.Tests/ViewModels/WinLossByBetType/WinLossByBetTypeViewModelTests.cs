namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByBetType
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Services;
    using Site.Requests.WinLossByProduct;
    using Site.ViewModels.WinLossByProduct;
    using Sunplus.Agent.Model.BL.Definitions;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly CacheRefService cacheRefService;
        private readonly ReportDateService reportDateService;

        public WinLossByBetTypeViewModelTests(UserFixture userFixture)
        {
            cacheRefService = Substitute.For<CacheRefService>();
            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            cacheRefService.GetProductName(22).Returns("Macau Game");
            cacheRefService.GetProductName(23).Returns("NG2");
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>());
            var expectedTitle = "Win Loss By Product";
            
            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
        }

        [Theory]
        [InlineData(4, "Super")]
        [InlineData(3, "Master")]
        [InlineData(2, "Agent")]
        public void BuildResources_CustLevel_ReturnCorrectCustLevelName(
        byte custLevelId,
        string expectedCustLevelName)
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>());

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.CustLevelName, expectedCustLevelName);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void BuildResources_CustLevel_ReturnCorrectDownLineLevelName(
        byte custLevelId,
        string expectedDownLineLevelName)
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>());

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.DownLineLevelName, expectedDownLineLevelName);
        }

        private WinLossByProductQuery CreateWinlossByBetTypeQuery(byte custLevelId)
        {
            var winLossBettypeQuery = new WinLossByProductQuery(userFixture.User);
            winLossBettypeQuery.CustLevelId = custLevelId;
            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossBettypeQuery;
        }
    }
}