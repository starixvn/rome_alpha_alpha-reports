namespace Alpha.Reports.Site.Tests.ViewModels.Components
{
    using System.Linq;
    using Extensions;
    using Site.ViewModels;
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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HasStackIcon_Always_ReturnsHasStackIconSetFromConstructor(bool expectedHasStackIcon)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, expectedHasStackIcon, false, false, false, null);

            // Act
            var actualHasStackIcon = pageHeaderViewModel.HasStackIcon;

            // Assert
            AssertExtension.Equal(expectedHasStackIcon, actualHasStackIcon);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void HasHideFilterIcon_Always_ReturnsHasHideFilterIconSetFromConstructor(bool expectedHasHideFilterIcon)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, expectedHasHideFilterIcon, false, false, null);

            // Act
            var actualHasHideFilterIcon = pageHeaderViewModel.HasHideFilterIcon;

            // Assert
            AssertExtension.Equal(expectedHasHideFilterIcon, actualHasHideFilterIcon);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsStackMode_Always_ReturnsIsStackModeSetFromConstructor(bool expectedIsStackMode)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, expectedIsStackMode, false, null);

            // Act
            var actualIsStackMode = pageHeaderViewModel.IsStackMode;

            // Assert
            AssertExtension.Equal(expectedIsStackMode, actualIsStackMode);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsFilterVisible_Always_ReturnsIsFilterVisibleSetFromConstructor(bool expectedIsFilterVisible)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, false, expectedIsFilterVisible, null);

            // Act
            var actualHideFilter = pageHeaderViewModel.IsFilterVisible;

            // Assert
            AssertExtension.Equal(expectedIsFilterVisible, actualHideFilter);
        }

        [Fact]
        public void BreadcrumbLevels_Always_ReturnsBreadcrumbLevelsSetFromConstructor()
        {
            // Arrange
            var expectedBreadcrumbLevels = Enumerable.Empty<ActionLinkViewModel>();
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, false, false, expectedBreadcrumbLevels);

            // Act
            var actualBreadcrumbLevels = pageHeaderViewModel.BreadcrumbLevels;

            // Assert
            AssertExtension.Equal(expectedBreadcrumbLevels, actualBreadcrumbLevels);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void IsShowActionIcons_HasStackIconOrHasFilterIcon_ReturnsTrue(bool hasStackIcon, bool hasHideFilterIcon)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, hasStackIcon, hasHideFilterIcon, false, false, null);

            // Act
            var actualIsShowActionIcons = pageHeaderViewModel.IsShowActionIcons;

            // Assert
            Assert.True(actualIsShowActionIcons);
        }

        [Fact]
        public void IsShowActionIcons_NotHaveStackIconAndHasFilterIcon_ReturnsTrue()
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, false, false, null);

            // Act
            var actualIsShowActionIcons = pageHeaderViewModel.IsShowActionIcons;

            // Assert
            Assert.False(actualIsShowActionIcons);
        }
    }
}
