////namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByGameType
////{
////    using System.Globalization;
////    using System.Linq;
////    using System.Threading;
////    using Constants;
////    using Fixtures;
////    using Params;
////    using Ploeh.AutoFixture;
////    using Reports.Models.WinlossByGameType;
////    using Site.Requests.WinLossByGameType;
////    using Site.ViewModels.WinLossByGameType;
////    using Utils;
////    using Xunit;

////    [Trait("Category", "WinLossByGameType")]
////    [Collection("UserCollection")]
////    public class WinLossByGameTypeViewModelTests
////    {
////        private readonly UserFixture userFixture;
////        private readonly Fixture fixture = new Fixture();

////        public WinLossByGameTypeViewModelTests(UserFixture userFixture)
////        {
////            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
////            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
////            this.userFixture = userFixture;
////            SystemTime.Today = DefaultVariables.CurrentDate;
////        }

////        [Fact]
////        public void Title_WinLossLevel_ReturnCorrectTitle()
////        {
////            // Arrange
////            var expectedTitle = "Win Loss By Bet Type";
////            var message = CreateWinlossByGameTypeQuery(4);
////            var winlossByBettypeViewModel = new WinLossByGameTypeViewModel(
////                Enumerable.Empty<WinLossByGameTypeModel>(),
////                fixture.Create<WinLossByGameTypeModel>(),
////                message,
////                fixture.Create<WinLossByGameTypeParam>());

////            // Act & Assert
////            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
////        }

////        private WinLossByGameTypeQuery CreateWinlossByGameTypeQuery(byte custLevelId)
////        {
////            var winLossBettypeQuery = new WinLossByGameTypeQuery(userFixture.User);
////            winLossBettypeQuery.CustLevelId = custLevelId;
////            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
////            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

////            return winLossBettypeQuery;
////        }
////    }
////}