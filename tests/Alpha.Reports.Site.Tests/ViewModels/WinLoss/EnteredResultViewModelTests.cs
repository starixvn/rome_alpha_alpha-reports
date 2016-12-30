namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Enumerations;
    using Reports.Models;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredResultViewModelTests
    {
        private readonly EnteredResultViewModel enteredResultViewModel;

        public EnteredResultViewModelTests()
        {
            enteredResultViewModel = new EnteredResultViewModel();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_EnteredResult_ReturnCorrectTitle(
            byte userLevel,
            string expectedTitle)
        {
            // Arrange
            enteredResultViewModel.UserLevel = Enumeration.FromValue<UserLevel>(userLevel);

            // Act & Assert
            Assert.Equal(enteredResultViewModel.Title, expectedTitle);
        }
    }
}