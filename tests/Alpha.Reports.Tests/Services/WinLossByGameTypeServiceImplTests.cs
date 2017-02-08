namespace Alpha.Reports.Tests.Services.WinLossByGameType
{
    using System;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossByGameType;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByGameType;
    using Reports.Params;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    public class WinLossByGameTypeServiceImplTests
    {
        private readonly WinLossByGameTypeDataAccess dataAccess;
        private readonly WinLossByGameTypeServiceImpl winLossByGameTypeServiceImpl;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;

        public WinLossByGameTypeServiceImplTests()
        {
            dataAccess = Substitute.For<WinLossByGameTypeDataAccess>();
            reportDateService = Substitute.For<ReportDateService>();
            winLossByGameTypeServiceImpl = new WinLossByGameTypeServiceImpl(reportDateService, dataAccess);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByGameType_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByGameTypeParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByGameTypeServiceImpl.GetWinLossByGameType(nullParam));
        }

        [Fact]
        public void GetWinLossByGameType_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossByGameTypeParam param = new WinLossByGameTypeParam(
                0,
                0,
                currentDate,
                currentDate,
                UserLevel.Super);

            var expectedData = Enumerable.Empty<WinLossByGameTypeModel>();

            // Act
            dataAccess.GetWinLossByGameType(Arg.Any<WinLossByGameTypeCriteria>()).Returns(expectedData);
            var actualData = winLossByGameTypeServiceImpl.GetWinLossByGameType(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByGameType_CallGetWinLossDataAccess()
        {
            // Arrange
            WinLossByGameTypeParam param = new WinLossByGameTypeParam(
                0,
                0,
                currentDate,
                currentDate,
                UserLevel.Super);

            // Act
            winLossByGameTypeServiceImpl.GetWinLossByGameType(param);

            // Assert
            dataAccess.Received().GetWinLossByGameType(Arg.Any<WinLossByGameTypeCriteria>());
        }
    }
}