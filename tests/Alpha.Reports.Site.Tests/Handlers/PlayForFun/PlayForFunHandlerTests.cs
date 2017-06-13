namespace Alpha.Reports.Site.Tests.Handlers.PlayForFun
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Services;
    using Site.Handlers.PlayForFun;
    using Site.Requests.PlayForFun;
    using Site.ViewModels.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("UserCollection")]
    public class PlayForFunHandlerTests
    {
        private readonly PlayForFunHandler handler;
        private readonly IUser user;
        private readonly PlayForFunService reportService;

        private readonly IEnumerable<MasterAgentPlayForFunModel> masterAgentPlayForFunModels;
        private readonly IEnumerable<Product> supportedProduct;

        public PlayForFunHandlerTests(UserFixture userFixture)
        {
            var productService = Substitute.For<ProductService>();
            reportService = Substitute.For<PlayForFunService>();
            handler = new PlayForFunHandler(productService, reportService);

            user = userFixture.User;

            masterAgentPlayForFunModels = new List<MasterAgentPlayForFunModel>
            {
                new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4)
            };

            reportService.MasterOrAgentPlayForFun(Arg.Any<PlayForFunParam>()).Returns(masterAgentPlayForFunModels);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            supportedProduct = new List<Product>
            {
                new Product(6, "Casino", null),
                new Product(22, "Macau Games", null),
                new Product(24, "Fishing World", null)
            };
        }

        [Fact]
        public void HandleMasterOrAgentPlayForFunQuery_NotSupportAnyProduct_ReturnsCorrectViewModel()
        {
            // Arrange
            reportService.SupportedProduct(Arg.Any<int>()).Returns(Enumerable.Empty<Product>());
            var message = new MasterOrAgentPlayForFunQuery(user)
            {
                CustId = 0,
                CustName = string.Empty,
                CustLevelId = 4
            };

            var data = new ReportData<PlayForFunModel>(masterAgentPlayForFunModels, new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4));

            var expectedViewModel = new MasterOrAgentPlayForFunReportViewModel(
                 message,
                 CreatePlayForFunParam(message, 0),
                 new List<CustInfo> { new CustInfo(0, string.Empty, 4) },
                 Enumerable.Empty<SelectListItem>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = "All", Value = "2", Selected = true },
                     new SelectListItem { Text = "Enabled", Value = "0", Selected = false },
                     new SelectListItem { Text = "Disabled", Value = "1", Selected = false }
                 },
                 data);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleMasterOrAgentPlayForFunQuery_HasSupportProducts_ReturnsCorrectViewModel()
        {
            // Arrange
            reportService.SupportedProduct(Arg.Any<int>()).Returns(supportedProduct);

            var message = new MasterOrAgentPlayForFunQuery(user)
            {
                CustId = 0,
                CustName = string.Empty,
                CustLevelId = 3,
                UserSelectedProductId = 22
            };

            var data = new ReportData<PlayForFunModel>(masterAgentPlayForFunModels, new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4));

            var expectedViewModel = new MasterOrAgentPlayForFunReportViewModel(
                 message,
                 CreatePlayForFunParam(message, 22),
                 new List<CustInfo> { new CustInfo(0, string.Empty, 3) },
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = null, Value = "6", Selected = false },
                     new SelectListItem { Text = null, Value = "22", Selected = true },
                     new SelectListItem { Text = null, Value = "24", Selected = false }
                 },
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = "All", Value = "2", Selected = true },
                     new SelectListItem { Text = "Enabled", Value = "0", Selected = false },
                     new SelectListItem { Text = "Disabled", Value = "1", Selected = false }
                 },
                 data);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleMemberPlayForFunQuery_NotSupportAnyProduct_ReturnsCorrectViewModel()
        {
            // Arrange
            reportService.SupportedProduct(Arg.Any<int>()).Returns(Enumerable.Empty<Product>());
            reportService.MemberPlayForFun(Arg.Any<PlayForFunParam>()).Returns(Enumerable.Empty<MemberPlayForFunModel>());
            var message = new MemberPlayForFunQuery(user)
            {
                CustId = 0,
                CustName = string.Empty,
                CustLevelId = 2
            };

            var data = new ReportData<PlayForFunModel>(
                Enumerable.Empty<MemberPlayForFunViewModel>(),
                new MemberPlayForFunViewModel(0, string.Empty, 0, 0, 0, false, string.Empty));

            var expectedViewModel = new MemberPlayForFunReportViewModel(
                 message,
                 CreatePlayForFunParam(message, 0),
                 new List<CustInfo> { new CustInfo(0, string.Empty, 2) },
                 Enumerable.Empty<SelectListItem>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = "All", Value = "2", Selected = true },
                     new SelectListItem { Text = "Enabled", Value = "0", Selected = false },
                     new SelectListItem { Text = "Disabled", Value = "1", Selected = false }
                 },
                 data);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void HandleMemberPlayForFunQuery_HasSupportProducts_ReturnsCorrectViewModel()
        {
            // Arrange
            reportService.SupportedProduct(Arg.Any<int>()).Returns(supportedProduct);
            var memberPlayForFunModels = new List<MemberPlayForFunModel>
            {
                new MemberPlayForFunModel(0, string.Empty, true, 1, 2, 3),
                new MemberPlayForFunModel(0, string.Empty, false, 1, 2, 3),
                new MemberPlayForFunModel(0, string.Empty, false, 1, 2, 3)
            };
            reportService.MemberPlayForFun(Arg.Any<PlayForFunParam>()).Returns(memberPlayForFunModels);
            var message = new MemberPlayForFunQuery(user)
            {
                CustId = 0,
                CustName = string.Empty,
                CustLevelId = 2,
                UserSelectedProductId = 22
            };

            var viewModels = new List<MemberPlayForFunViewModel>
            {
                new MemberPlayForFunViewModel(0, string.Empty, 1, 2, 3, true, "Disabled"),
                new MemberPlayForFunViewModel(0, string.Empty, 1, 2, 3, false, "Enabled"),
                new MemberPlayForFunViewModel(0, string.Empty, 1, 2, 3, false, "Enabled")
            };
            var data = new ReportData<PlayForFunModel>(viewModels, new MemberPlayForFunViewModel(0, string.Empty, 3, 6, 9, false, string.Empty));

            var expectedViewModel = new MemberPlayForFunReportViewModel(
                 message,
                 CreatePlayForFunParam(message, 22),
                 new List<CustInfo> { new CustInfo(0, string.Empty, 2) },
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = null, Value = "6", Selected = false },
                     new SelectListItem { Text = null, Value = "22", Selected = true },
                     new SelectListItem { Text = null, Value = "24", Selected = false }
                 },
                 new List<SelectListItem>
                 {
                     new SelectListItem { Text = "All", Value = "2", Selected = true },
                     new SelectListItem { Text = "Enabled", Value = "0", Selected = false },
                     new SelectListItem { Text = "Disabled", Value = "1", Selected = false }
                 },
                 data);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData(0, "Enabled")]
        [InlineData(1, "Disabled")]
        [InlineData(2, "All")]
        [InlineData(3, "")]
        public void GetStatusName_Always_ReturnsCorrectName(int statusId, string expectedStatusName)
        {
            // Act
            var actualStatusName = handler.GetStatusName(statusId);

            // Assert
            Assert.Equal(expectedStatusName, actualStatusName);
        }

        private PlayForFunParam CreatePlayForFunParam(PlayForFunQuery message, int userSelectedProductId)
            => new PlayForFunParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                userSelectedProductId,
                message.UserSelectedStatusId,
                message.CustLevel);
    }
}
