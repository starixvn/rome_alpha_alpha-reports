namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByGameType
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinlossByGameType;
    using Site.Requests.WinLossByGameType;
    using Site.ViewModels.WinLossByGameType;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();

        public WinLossByGameTypeViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;
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
            var message = CreateWinlossByGameTypeQuery(custLevelId);
            var winlossByGameTypeViewModel = new WinLossByGameTypeViewModel(
               Enumerable.Empty<WinLossByGameTypeModel>(),
               fixture.Create<WinLossByGameTypeModel>(),
               message,
               fixture.Create<WinLossByGameTypeParam>());

            // Act & Assert
            Assert.Equal(winlossByGameTypeViewModel.CustLevelName, expectedCustLevelName);
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
            var message = CreateWinlossByGameTypeQuery(custLevelId);
            var winlossByGameTypeViewModel = new WinLossByGameTypeViewModel(
               Enumerable.Empty<WinLossByGameTypeModel>(),
               fixture.Create<WinLossByGameTypeModel>(),
               message,
               fixture.Create<WinLossByGameTypeParam>());

            // Act & Assert
            Assert.Equal(winlossByGameTypeViewModel.DownLineLevelName, expectedDownLineLevelName);
        }

        private WinLossByGameTypeQuery CreateWinlossByGameTypeQuery(byte custLevelId)
        {
            var winLossBettypeQuery = new WinLossByGameTypeQuery(userFixture.User);
            winLossBettypeQuery.CustLevelId = custLevelId;
            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossBettypeQuery;
        }
    }
}