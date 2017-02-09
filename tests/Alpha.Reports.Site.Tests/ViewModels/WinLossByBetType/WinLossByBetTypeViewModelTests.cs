namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByBetType
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinlossByBetType;
    using Site.Requests.WinLossByBetType;
    using Site.ViewModels.WinLossByBetType;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossByBetTypeViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var expectedTitle = "Win Loss By Bet Type";
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                SystemTime.Today,
                fixture.Create<Func<int, string, bool, string>>());

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
        }

        private WinLossByBetTypeQuery CreateWinlossByBetTypeQuery(byte custLevelId)
        {
            var winLossBettypeQuery = new WinLossByBetTypeQuery(userFixture.User);
            winLossBettypeQuery.CustLevelId = custLevelId;
            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossBettypeQuery;
        }
    }
}