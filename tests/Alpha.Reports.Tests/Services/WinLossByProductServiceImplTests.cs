namespace Alpha.Reports.Tests.Services.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.PartialUM.Services;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossByProductCriteria;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Reports.Params;
    using Reports.Services;
    using Site.Tests.TestCases.WinLossByProduct;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    public class WinLossByProductServiceImplTests
    {
        private readonly WinLossByProductDataAccess dataAccess;
        private readonly IPartialUMService partialUMService;
        private readonly WinLossByProductServiceImpl winLossByBetTypeServiceImpl;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;

        public WinLossByProductServiceImplTests()
        {
            dataAccess = Substitute.For<WinLossByProductDataAccess>();
            cacheRefService = Substitute.For<CacheRefService>();
            reportDateService = Substitute.For<ReportDateService>();
            partialUMService = Substitute.For<IPartialUMService>();
            winLossByBetTypeServiceImpl = new WinLossByProductServiceImpl(reportDateService, dataAccess, cacheRefService, partialUMService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByBetType_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByProductParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBetTypeServiceImpl.GetWinLossByProduct(nullParam));
        }

        [Fact]
        public void GetWinLossByBetType_EmptyData_ReturnsDefaultReportData()
        {
            // Arrange
            var param = new WinLossByProductParam(
                0,
                currentDate,
                currentDate,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty);

            var winLossByBetTypeData = Enumerable.Empty<WinLossByBetTypeModel>();

            var expectedData = new List<WinLossByProductModel>();

            // Act
            dataAccess.GetWinLossByBetType(Arg.Any<WinLossByProductCriteria>()).Returns(winLossByBetTypeData);
            var actualData = winLossByBetTypeServiceImpl.GetWinLossByProduct(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBetType_HasData_ReturnsCorrectModel()
        {
            // Arrange
            var param = new WinLossByProductParam(
                0,
                currentDate,
                currentDate,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty);

            var winLossByBetTypeData = WinLossByProductModelTestCases.BuildSuperDataTest();

            var winLossBySports = new List<WinLossBySportModel>
            {
                new WinLossBySportModel(0, string.Empty, winLossByBetTypeData)
            };

            var expectedData = new List<WinLossByProductModel>
            {
                new WinLossByProductModel(0, string.Empty, winLossBySports)
            };

            // Act
            dataAccess.GetWinLossByBetType(Arg.Any<WinLossByProductCriteria>()).Returns(winLossByBetTypeData);
            var actualData = winLossByBetTypeServiceImpl.GetWinLossByProduct(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBetType_CallGetWinLossDataAccess()
        {
            // Arrange
            var param = new WinLossByProductParam(
                0,
                currentDate,
                currentDate,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty);

            // Act
            winLossByBetTypeServiceImpl.GetWinLossByProduct(param);

            // Assert
            dataAccess.Received().GetWinLossByBetType(Arg.Any<WinLossByProductCriteria>());
        }
    }
}