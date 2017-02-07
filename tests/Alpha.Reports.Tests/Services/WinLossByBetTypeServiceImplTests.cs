namespace Alpha.Reports.Tests.Services.WinLossByBetType
{
    using System;
    using System.Linq;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossByBetTypeCriteria;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByBettype;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeServiceImplTests
    {
        private readonly WinLossByBetTypeDataAccess dataAccess;
        private readonly WinLossByBettypeServiceImpl winLossByBetTypeServiceImpl;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;

        public WinLossByBetTypeServiceImplTests()
        {
            dataAccess = Substitute.For<WinLossByBetTypeDataAccess>();
            reportDateService = Substitute.For<ReportDateService>();
            winLossByBetTypeServiceImpl = new WinLossByBettypeServiceImpl(reportDateService, dataAccess);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByBetType_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByBetTypeParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBetTypeServiceImpl.GetWinLossByBetType(nullParam));
        }

        [Fact]
        public void GetWinLossByBetType_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossByBetTypeParam param = new WinLossByBetTypeParam(
                0,
                currentDate,
                currentDate,
                UserLevel.Super);

            var expectedData = Enumerable.Empty<WinLossByBetTypeModel>();

            // Act
            dataAccess.GetWinLossByBetType(Arg.Any<WinLossByBetTypeCriteria>()).Returns(expectedData);
            var actualData = winLossByBetTypeServiceImpl.GetWinLossByBetType(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBetType_CallGetWinLossDataAccess()
        {
            // Arrange
            WinLossByBetTypeParam param = new WinLossByBetTypeParam(
                0,
                currentDate,
                currentDate,
                UserLevel.Super);

            // Act
            winLossByBetTypeServiceImpl.GetWinLossByBetType(param);

            // Assert
            dataAccess.Received().GetWinLossByBetType(Arg.Any<WinLossByBetTypeCriteria>());
        }
    }
}