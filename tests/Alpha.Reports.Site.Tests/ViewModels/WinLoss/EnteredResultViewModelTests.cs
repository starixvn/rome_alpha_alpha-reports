namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using Reports.Models.Winloss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class EnteredResultViewModelTests
    {
        private readonly UserFixture userFixture;

        public EnteredResultViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_EnteredResult_ReturnCorrectTitle(
            byte custLevelId,
            string expectedTitle)
        {
            // Arrange
            var message = new EnteredSportResultQuery(userFixture.User);
            message.CustLevelId = custLevelId;

            // Act
            var enteredResultViewModel = new EnteredResultViewModel(message, Enumerable.Empty<EnteredSportResult>());

            // Assert
            Assert.Equal(enteredResultViewModel.Title, expectedTitle);
        }
    }
}