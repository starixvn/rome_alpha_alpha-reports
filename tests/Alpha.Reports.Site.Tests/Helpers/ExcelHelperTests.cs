namespace Alpha.Reports.Site.Tests.Helpers
{
    using System;
    using System.Resources;
    using NSubstitute;
    using Site.Helpers;
    using Xunit;

    public class ExcelHelperTests
    {
        [Theory]
        [InlineData("ResourceKey")]
        public void HeaderTitleMapping_Always_ReturnsTextFromResourceManager(string resourceKey)
        {
            // Arrange
            var expectedText = "Text";

            var resourceManager = Substitute.For<ResourceManager>();
            resourceManager.GetString(Arg.Any<string>()).Returns(expectedText);
            ExcelHelper.ResourceManager = resourceManager;

            // Act
            var actualText = ExcelHelper.HeaderTitleMapping(resourceKey);

            // Assert
            Assert.Equal(expectedText, actualText);
        }

        [Theory]
        [InlineData("ResourceKey")]
        public void HeaderTitleMapping_Always_CallsResourceManagerGetString(string resourceKey)
        {
            // Arrange
            var expectedText = "Text";

            var resourceManager = Substitute.For<ResourceManager>();
            resourceManager.GetString(Arg.Any<string>()).Returns(expectedText);
            ExcelHelper.ResourceManager = resourceManager;

            // Act
            ExcelHelper.HeaderTitleMapping(resourceKey);

            // Assert
            resourceManager.Received().GetString(Arg.Is<string>(resourceKey));
        }
    }
}
