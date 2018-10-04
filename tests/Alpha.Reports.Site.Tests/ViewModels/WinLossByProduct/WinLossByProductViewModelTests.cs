namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByProduct
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models.WinlossByBetType;
    using Reports.Models.WinLossByProduct;
    using Services;
    using Site.Requests.WinLossByProduct;
    using Site.ViewModels.WinLossByProduct;
    using Sunplus.Agent.Model.BL.Definitions;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly ReportDateService reportDateService;

        public WinLossByProductViewModelTests(UserFixture userFixture)
        {
            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var message = CreateWinlossByProductQuery(4);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>(),
               Enumerable.Empty<SelectListItem>());
            var expectedTitle = "Win Loss By Product";
            
            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
        }

        [Theory]
        [InlineData(4, "Super")]
        [InlineData(3, "Master")]
        [InlineData(2, "Agent")]
        public void BuildResources_CustLevel_ReturnCorrectCustLevelName(
        byte custLevelId,
        string expectedCustLevelName)
        {
            // Arrange
            var message = CreateWinlossByProductQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>(),
               Enumerable.Empty<SelectListItem>());

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.CustLevelName, expectedCustLevelName);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void BuildResources_CustLevel_ReturnCorrectDownLineLevelName(
        byte custLevelId,
        string expectedDownLineLevelName)
        {
            // Arrange
            var message = CreateWinlossByProductQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByProductViewModel(
               Enumerable.Empty<WinLossByProductModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByProductParam>(),
               fixture.Create<Func<int, BetType, string, bool, string>>(),
               Enumerable.Empty<SelectListItem>(),
               Enumerable.Empty<SelectListItem>());

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.DownLineLevelName, expectedDownLineLevelName);
        }

        private WinLossByProductQuery CreateWinlossByProductQuery(byte custLevelId)
        {
            var query = new WinLossByProductQuery(userFixture.User);
            query.CustLevelId = custLevelId;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate;

            return query;
        }
    }
}