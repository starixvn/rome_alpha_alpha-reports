namespace Alpha.Reports.Site.Tests.ViewModels.Components
{
    using Extensions;
    using Site.ViewModels.Components;
    using Xunit;

    public class PageHeaderViewModelTests
    {
        [Fact]
        public void Title_Always_ReturnsTitleSetFromConstructor()
        {
            // Arrange
            var title = "Title";
            var pageHeaderViewModel = new PageHeaderViewModel(title, false, false, false, false, null);
            var expectedTitle = title;

            // Act
            var actualTitle = pageHeaderViewModel.Title;

            // Assert
            AssertExtension.Equal(expectedTitle, actualTitle);
        }
    }
}
