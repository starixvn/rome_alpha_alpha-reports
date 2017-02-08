namespace Alpha.Reports.Site.Tests.Handlers.WinLossByGameType
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Models.WinlossByGameType;
    using Services;
    using Site.Handlers.WinLossByGameType;
    using Site.Requests.WinLossByGameType;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeHandlerTestsHandlerTests
    {
        private readonly WinLossByGameTypeService reportService;
        private readonly WinLossByGameTypeHandler winLossByGameTypeHandler;
        private readonly CompareLogic compareLogic;

        public WinLossByGameTypeHandlerTestsHandlerTests()
        {
            reportService = Substitute.For<WinLossByGameTypeService>();

            winLossByGameTypeHandler = new WinLossByGameTypeHandler(reportService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossByBettypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByGameTypeQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByGameTypeHandler.Handle(nullMessage));
        }

        [Theory]
        [MemberData("WinLossByGameTypeViewModelSources", MemberType = typeof(WinLossByGameTypeViewModelTestCases))]
        public void AccumulateGrandTotal_ValidReportData_ReturnsCorrectGrandTotal(
        IEnumerable<WinLossByGameTypeModel> data,
        WinLossByGameTypeModel expectedGrandTotal)
        {
            // Arrange

            // Act
            var actualGrandTotal = winLossByGameTypeHandler.AccumulateGrandTotal(data);

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }
    }
}