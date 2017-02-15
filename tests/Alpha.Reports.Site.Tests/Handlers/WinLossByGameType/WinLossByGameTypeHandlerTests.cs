namespace Alpha.Reports.Site.Tests.Handlers.WinLossByGameType
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
    using Reports.Models.WinlossByGameType;
    using Services;
    using Site.Handlers.WinLossByGameType;
    using Site.Requests.WinLossByGameType;
    using Site.ViewModels.WinLossByGameType;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("UserCollection")]
    public class WinLossByGameTypeHandlerTests
    {
        private readonly WinLossByGameTypeService reportService;
        private readonly WinLossByGameTypeHandler winLossByGameTypeHandler;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;

        public WinLossByGameTypeHandlerTests(UserFixture userFixture)
        {
            reportService = Substitute.For<WinLossByGameTypeService>();

            winLossByGameTypeHandler = new WinLossByGameTypeHandler(reportService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            this.userFixture = userFixture;
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandlerWinLossByGameTypeQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByGameTypeQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => winLossByGameTypeHandler.Handle(nullMessage));
        }

        [Fact]
        public void HandlerWinLossByGameTypeQuery_ValidMessage_ReturnCorrectReportData()
        {
            // Arrange
            var message = new WinLossByGameTypeQuery(userFixture.User);
            message.CustId = 1;
            message.CustName = "CustName";
            message.CustLevelId = 3;
            message.IsDescendingSort = false;

            var expectedReportData = Enumerable.Empty<WinLossByGameTypeModel>();
            var grandTotal = new WinLossByGameTypeModel();
            var param = new WinLossByGameTypeParam(
                0,
                22,
                DefaultVariables.CurrentDate,
                DefaultVariables.CurrentDate,
                message.CustLevel);

            reportService.GetWinLossByGameType(Arg.Any<WinLossByGameTypeParam>()).Returns(expectedReportData);

            var expectedWinLossByGameTypeViewModel = new WinLossByGameTypeViewModel(
                expectedReportData,
                grandTotal,
                message,
                param);

            // Act
            var actualWinLossByGameTypeViewModel = winLossByGameTypeHandler.Handle(message);

            // Assert
            var result = compareLogic.Compare(expectedWinLossByGameTypeViewModel.ReportData, expectedReportData);
            Assert.True(result.AreEqual);
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