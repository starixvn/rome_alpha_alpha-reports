namespace Alpha.Reports.Site.Tests.Handlers.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Services;
    using Site.Handlers.WinLossByBetType;
    using Site.Requests.WinLossByBetType;
    using Site.ViewModels.WinLossByBetType;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeHandlerTests
    {
        private readonly WinLossByBetTypeService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBetTypeHandler winLossByBetTypeHandler;
        private readonly Fixture fixture = new Fixture();

        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<WinLossByBetTypeService>();
            cacheRefService = Substitute.For<CacheRefService>();

            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            this.userFixture = userFixture;
            winLossByBetTypeHandler = new WinLossByBetTypeHandler(reportService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossByBetTypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByBetTypeQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBetTypeHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandlerWinLossByBetTypeQuery_ValidMessage_ReturnCorrectReportData()
        {
            // Arrange
            var message = new WinLossByBetTypeQuery(userFixture.User);
            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;
            message.IsDescendingSort = false;

            var expectedReportData = Enumerable.Empty<WinLossByBetTypeModel>();
            var grandTotal = new WinLossByBetTypeModel();
            var param = new WinLossByBetTypeParam(
                0,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                message.CustLevel);

            reportService.GetWinLossByBetType(Arg.Any<WinLossByBetTypeParam>()).Returns(expectedReportData);

            var expectedWinLossByBetTypeViewModel = new WinLossByBetTypeViewModel(
                expectedReportData,
                grandTotal,
                message,
                param,
                null,
                cacheRefService);

            // Act
            var actualWinLossByBetTypeViewModel = winLossByBetTypeHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedWinLossByBetTypeViewModel.ReportData, expectedReportData);
            Assert.True(result.AreEqual);
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