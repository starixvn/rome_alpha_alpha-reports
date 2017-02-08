namespace Alpha.Reports.Site.Tests.Handlers.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Models.WinlossByBetType;
    using Services;
    using Site.Handlers.WinLossByBetType;
    using Site.Requests.WinLossByBetType;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeHandlerTestsHandlerTests
    {
        private readonly WinLossByBetTypeService reportService;
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBetTypeHandler winLossByBetTypeHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeHandlerTestsHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<WinLossByBetTypeService>();
            reportDateService = Substitute.For<ReportDateService>();

            this.userFixture = userFixture;
            winLossByBetTypeHandler = new WinLossByBetTypeHandler(reportService, reportDateService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossByBettypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByBetTypeQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBetTypeHandler.Handle(nullMessage));
        }

        [Theory]
        [MemberData("WinLossByBetTypeViewModelSources", MemberType = typeof(WinLossByBetTypeViewModelTestCases))]
        public void AccumulateGrandTotal_ValidReportData_ReturnsCorrectGrandTotal(
        IEnumerable<WinLossByBetTypeModel> data,
        WinLossByBetTypeModel expectedGrandTotal)
        {
            // Arrange

            // Act
            var actualGrandTotal = winLossByBetTypeHandler.AccumulateGrandTotal(data);

            // Assert
            var result = compareLogic.Compare(actualGrandTotal, expectedGrandTotal);
            Assert.True(result.AreEqual);
        }
    }
}