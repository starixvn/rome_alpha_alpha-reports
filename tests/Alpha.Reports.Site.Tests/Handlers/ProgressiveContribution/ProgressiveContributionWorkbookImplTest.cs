namespace Alpha.Reports.Site.Tests.Handlers.ProgressiveContribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fixtures;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models.ProgressiveContribution;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    [Collection("AnyValueCreatorCollection")]
    public class ProgressiveContributionWorkbookImplTest
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly Sheet sheet;
        private readonly Row row;
        private readonly ProgressiveContributionWorkbookImplForTesting progressiveContributionWorkbook;

        public ProgressiveContributionWorkbookImplTest(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            sheet = Substitute.For<Sheet>();
            row = Substitute.For<Row>();
            progressiveContributionWorkbook = new ProgressiveContributionWorkbookImplForTesting();
        }

        [Fact]
        public void SetData_Always_SetCorrectData()
        {
            // Arrange
            var progressiveContributions = anyValueCreatorFixture.Create<IEnumerable<ProgressiveContributionByProductModel>>();
            var total = anyValueCreatorFixture.Create<ProgressiveContributionByBetTypeModel>();

            // Act
            progressiveContributionWorkbook.SetData(progressiveContributions, total);

            // Assert
            Assert.True(
                progressiveContributionWorkbook.ProgressiveContributions == progressiveContributions
                && progressiveContributionWorkbook.Total == total);
        }

        [Fact]
        public void SetRowMappers_Always_SetCorrectData()
        {
            // Arrange
            var mapper = anyValueCreatorFixture.Create<Mapper<ProgressiveContributionByBetTypeModel>>();
            var totalMaper = anyValueCreatorFixture.Create<Mapper<ProgressiveContributionByBetTypeModel>>();

            // Act
            progressiveContributionWorkbook.SetRowMappers(mapper, totalMaper);

            // Assert
            Assert.True(
                progressiveContributionWorkbook.Mapper == mapper
                && progressiveContributionWorkbook.TotalMapper == totalMaper);
        }

        [Fact]
        public void FillProductTitle_Always_SetProductNameToCorrectCell()
        {
            // Arrange
            row.FirstCellIndex.Returns((short)1);
            sheet.GetRow(0).Returns(row);
            var data = new ProgressiveContributionByProductModel(1, "Product Name", Enumerable.Empty<ProgressiveContributionByBetTypeModel>());
            progressiveContributionWorkbook.SetRowMappers(null, new Mapper<ProgressiveContributionByBetTypeModel>());

            // Act
            progressiveContributionWorkbook.FillProductTitle(sheet, 0, data);

            // Assert
            row.Received().SetCellValue(1, "PRODUCT NAME");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FillProgressiveContribution_Always_SetsValuesToCells(bool isEvenRow)
        {
            // Arrange
            sheet.GetRow(0).Returns(row);
            var data = new ProgressiveContributionByBetTypeModel(1, 2, "Bet Type Name", 100, 100, 10, 20, 30);

            var mapper = new Mapper<ProgressiveContributionByBetTypeModel>();
            mapper.Map("A", p => p.BetTypeName);
            progressiveContributionWorkbook.SetRowMappers(mapper, null);

            // Act
            progressiveContributionWorkbook.FillProgressiveContribution(sheet, 0, isEvenRow, data);

            // Assert
            row.Received().SetCellValue(0, "Bet Type Name");
        }

        [Fact]
        public void FillBody_NullSheet_ThrowsArgumentNullException()
        {
            // Arrange
            Sheet sheet = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => progressiveContributionWorkbook.CallFillBody(sheet));
        }

        [Fact]
        public void FillBodyRows_NullSheet_ThrowsArgumentNullException()
        {
            // Arrange
            Sheet sheet = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => progressiveContributionWorkbook.CallFillBodyRows(sheet));
        }

        [Fact]
        public void FillFooterRows_NullSheet_ThrowsArgumentNullException()
        {
            // Arrange
            Sheet sheet = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => progressiveContributionWorkbook.CallFillFooterRows(sheet));
        }

        [Fact]
        public void TotalRowTemplate_Always_ReturnsCorrectValue()
        {
            // Act
            progressiveContributionWorkbook.TotalRowTemplate = row;

            // Assert
            Assert.Equal(row, progressiveContributionWorkbook.TotalRowTemplate);
        }
    }
}