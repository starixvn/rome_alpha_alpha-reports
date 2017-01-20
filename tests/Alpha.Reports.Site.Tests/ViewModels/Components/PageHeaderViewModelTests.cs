namespace Alpha.Reports.Site.Tests.ViewModels.Components
{
    using System.Collections.Generic;
    using Extensions;
    using Reports.Models;
    using Site.ViewModels.Components;
    using Xunit;

    [Trait("Category", "Site.Components")]
    public class PageHeaderViewModelTests
    {
        [Fact]
        public void Title_Always_ReturnsTitleSetFromConstructor()
        {
            // Arrange
            var title = "Title";
            var pageHeaderViewModel = new PageHeaderViewModel(title, false, false, false, false, null, string.Empty);
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
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, expectedHasStackIcon, false, false, false, null, string.Empty);

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
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, expectedHasHideFilterIcon, false, false, null, string.Empty);

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
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, expectedIsStackMode, false, null, string.Empty);

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
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, false, expectedIsFilterVisible, null, string.Empty);

            // Act
            var actualHideFilter = pageHeaderViewModel.IsFilterVisible;

            // Assert
            AssertExtension.Equal(expectedIsFilterVisible, actualHideFilter);
        }

        [Fact]
        public void BreadcrumbLevel_Always_ReturnsBreadcrumbLevelSetFromConstructor()
        {
            // Arrange
            var expectedBreadcrumbLevelViewModel = new BreadcrumbLevelViewModel(
                new List<CustInfo>
                {
                    new CustInfo(1, "Name", 1)
                },
                "LevelJson");
            var pageHeaderViewModel = new PageHeaderViewModel(
                string.Empty,
                false,
                false,
                false,
                false,
                expectedBreadcrumbLevelViewModel.BreadcrumbLevel,
                expectedBreadcrumbLevelViewModel.BreadcrumbLevelJson);

            // Act
            var actualBreadcrumbLevelViewModel = pageHeaderViewModel.BreadcrumbLevelViewModel;

            // Assert
            AssertExtension.Equal(expectedBreadcrumbLevelViewModel, actualBreadcrumbLevelViewModel);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public void IsShowActionIcons_HasStackIconOrHasFilterIcon_ReturnsTrue(bool hasStackIcon, bool hasHideFilterIcon)
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, hasStackIcon, hasHideFilterIcon, false, false, null, string.Empty);

            // Act
            var actualIsShowActionIcons = pageHeaderViewModel.IsShowActionIcons;

            // Assert
            Assert.True(actualIsShowActionIcons);
        }

        [Fact]
        public void IsShowActionIcons_NotHaveStackIconAndHasFilterIcon_ReturnsTrue()
        {
            // Arrange
            var pageHeaderViewModel = new PageHeaderViewModel(string.Empty, false, false, false, false, null, string.Empty);

            // Act
            var actualIsShowActionIcons = pageHeaderViewModel.IsShowActionIcons;

            // Assert
            Assert.False(actualIsShowActionIcons);
        }
    }
}