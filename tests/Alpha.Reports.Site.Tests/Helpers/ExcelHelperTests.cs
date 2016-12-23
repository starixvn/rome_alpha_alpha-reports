namespace Alpha.Reports.Site.Tests.Helpers
{
    using System;
    using System.Resources;
    using NSubstitute;
    using Site.Helpers;
    using Xunit;

    public class ExcelHelperTests
    {
        [Fact]
        public void ContentType_Always_ReturnsMSExcel()
        {
            // Arrange
            var expectedContentType = "application/vnd.ms-excel";

            // Act
            var actualContentType = ExcelHelper.ContentType;

            // Assert
            Assert.Equal(expectedContentType, actualContentType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetFileName_NullOrWhiteSpaceFileName_ThrowsArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExcelHelper.GetFileName(fileName));
        }

        [Theory]
        [InlineData("FileName/", "FileName_.xls")]
        [InlineData("FileName ", "FileName_.xls")]
        [InlineData("FileName/Name Name", "FileName_Name_Name.xls")]
        [InlineData("FileName Name/Name", "FileName_Name_Name.xls")]
        [InlineData("FileName/Name/Name", "FileName_Name_Name.xls")]
        [InlineData("FileName Name Name", "FileName_Name_Name.xls")]
        [InlineData("FileName", "FileName.xls")]
        [InlineData("<>FileName<>", "<>FileName<>.xls")]
        public void GetFileName_FileNameHasSlashOrSpaceOrNot_Returns(
            string fileName,
            string expectedFileName)
        {
            // Act
            var actualFileName = ExcelHelper.GetFileName(fileName);

            // Assert
            Assert.Equal(expectedFileName, actualFileName);
        }

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
            resourceManager.Received().GetString(Arg.Is<string>(s => string.Equals(resourceKey, s)));
        }
    }
}
