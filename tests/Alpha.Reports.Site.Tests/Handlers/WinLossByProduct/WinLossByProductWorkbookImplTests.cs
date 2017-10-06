namespace Alpha.Reports.Site.Tests.Handlers.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Xunit;

    public class WinLossByProductWorkbookImplTests
    {
        private WinLossByProductWorkbookImplForTesting winLossByProductWorkbookImpl;

        public WinLossByProductWorkbookImplTests()
        {
            winLossByProductWorkbookImpl = new WinLossByProductWorkbookImplForTesting();
        }

        [Fact]
        public void SetData_Always_SetsCorrectData()
        {
            // Arrange
            var winLossByProductData = Enumerable.Empty<WinLossByProductModel>();
            var total = new WinLossByBetTypeModel();

            // Act
            winLossByProductWorkbookImpl.SetData(winLossByProductData, total);

            // Assert
            Assert.True(winLossByProductData == winLossByProductWorkbookImpl.WinLossByProductData
                && total == winLossByProductWorkbookImpl.Total);
        }

        [Fact]
        public void SetRowMappers_Always_SetsCorrectValue()
        {
            // Arrange
            var winLossByBetTypeMapper = new Mapper<WinLossByBetTypeModel>();
            var totalMapper = new Mapper<WinLossByBetTypeModel>();

            // Act
            winLossByProductWorkbookImpl.SetRowMappers(winLossByBetTypeMapper, totalMapper);

            // Assert
            Assert.True(winLossByBetTypeMapper == winLossByProductWorkbookImpl.WinLossByBetTypeMapper
                && totalMapper == winLossByProductWorkbookImpl.TotalMapper);
        }

        [Fact]
        public void FillProductTitle_Always_SetsProductNameToCorrectCell()
        {
            // Arrange
            var sheet = Substitute.For<Sheet>();
            var row = Substitute.For<Row>();
            row.FirstCellIndex.Returns((short)1);
            sheet.GetRow(0).Returns(row);
            var data = new WinLossByProductModel(1, "Product Name", Enumerable.Empty<WinLossBySportModel>());

            winLossByProductWorkbookImpl.SetRowMappers(null, new Mapper<WinLossByBetTypeModel>());

            // Act
            winLossByProductWorkbookImpl.FillProductTitle(sheet, 0, data);

            // Assert
            row.Received().SetCellValue(1, "PRODUCT NAME");
        }

        [Fact]
        public void FillSportTitle_Always_SetsSportNameToCorrectCell()
        {
            // Arrange
            var sheet = Substitute.For<Sheet>();
            var row = Substitute.For<Row>();
            row.FirstCellIndex.Returns((short)1);
            sheet.GetRow(0).Returns(row);
            var data = new WinLossBySportModel(1, "Sport Name", Enumerable.Empty<WinLossByBetTypeModel>());

            winLossByProductWorkbookImpl.SetRowMappers(null, new Mapper<WinLossByBetTypeModel>());

            // Act
            winLossByProductWorkbookImpl.FillSportTitle(sheet, 0, data);

            // Assert
            row.Received().SetCellValue(1, "SPORT NAME");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FillWinLossByBetType_Always_SetsValuesToCells(bool isEvenRow)
        {
            // Arrange
            var sheet = Substitute.For<Sheet>();
            var row = Substitute.For<Row>();
            sheet.GetRow(0).Returns(row);
            var data = new WinLossByBetTypeModel(1, "Bet Type Name", 2, 3, "Sport Type Name", false, 4, 5, 6, null, null, null, null);

            var mapper = new Mapper<WinLossByBetTypeModel>();
            mapper.Map("A", winLoss => winLoss.BetTypeName);

            winLossByProductWorkbookImpl.SetRowMappers(mapper, null);

            // Act
            winLossByProductWorkbookImpl.FillWinLossByBetType(sheet, 0, isEvenRow, data);

            // Assert
            row.Received().SetCellValue(0, "Bet Type Name");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(24)]
        [InlineData(26)]
        public void FillSport_IsShowBetTypeDirectly_FillBetTypeRows(int productId)
        {
            // Arrange
            var sheet = Substitute.For<Sheet>();
            var row = Substitute.For<Row>();
            sheet.GetRow(0).Returns(row);
            var data = new WinLossByBetTypeModel(1, "Bet Type Name", 2, 3, "Sport Type Name", false, 4, 5, 6, null, null, null, null);
            var winlossBySport = new WinLossBySportModel(1, "Sport Name", new List<WinLossByBetTypeModel> { data });

            var mapper = new Mapper<WinLossByBetTypeModel>();
            mapper.Map("A", winLoss => winLoss.BetTypeName);

            winLossByProductWorkbookImpl.SetRowMappers(mapper, null);

            // Act
            winLossByProductWorkbookImpl.FillSport(sheet, winlossBySport, productId);

            // Assert
            row.Received().SetCellValue(0, "Bet Type Name");
        }

        [Theory]
        [InlineData(5)]
        [InlineData(23)]
        public void FillSport_NotShowBetTypeDirectly_FillSportTitle(int productId)
        {
            // Arrange
            var sheet = Substitute.For<Sheet>();
            var row = Substitute.For<Row>();
            sheet.GetRow(0).Returns(row);
            var data = new WinLossByBetTypeModel(1, "Bet Type Name", 2, 3, "Sport Type Name", false, 4, 5, 6, null, null, null, null);
            var winlossBySport = new WinLossBySportModel(1, "Sport Name", new List<WinLossByBetTypeModel> { data });

            var mapper = new Mapper<WinLossByBetTypeModel>();
            mapper.Map("A", winLoss => winLoss.BetTypeName);

            winLossByProductWorkbookImpl.SetRowMappers(mapper, new Mapper<WinLossByBetTypeModel>());

            // Act
            winLossByProductWorkbookImpl.FillSport(sheet, winlossBySport, productId);

            // Assert
            row.Received().SetCellValue(0, "SPORT NAME");
        }

        [Fact]
        public void FillBodyRows_NullSheet_ThrowsArgumentNullException()
        {
            // Arrange
            Sheet sheet = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductWorkbookImpl.CallFillBodyRows(sheet));
        }

        [Fact]
        public void FillFooterRows_NullSheet_ThrowsArgumentNullException()
        {
            // Arrange
            Sheet sheet = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductWorkbookImpl.CallFillFooterRows(sheet));
        }

        [Fact]
        public void TotalRowTemplate_Always_ReturnsCorrectValue()
        {
            // Arrange
            var row = Substitute.For<Row>();

            // Act
            winLossByProductWorkbookImpl.TotalRowTemplate = row;

            // Assert
            Assert.Equal(row, winLossByProductWorkbookImpl.TotalRowTemplate);
        }
    }
}
