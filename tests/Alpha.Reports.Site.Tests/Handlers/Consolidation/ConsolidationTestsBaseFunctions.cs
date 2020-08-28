namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Params.Consolidation;
    using Reports.Models;
    using Reports.Models.Consolidation;
    using Reports.Models.WinLossDetail;
    using Services;
    using Utils;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class ConsolidationTestsBaseFunctions
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly ConsolidationService reportService;
        protected readonly CacheRefService cacheRefService;
        protected readonly IUser user;
        protected readonly AnyValueCreatorFixture anyValueCreatorFixture;

        protected readonly PagedReportData<WinLossDetailModel> winLossDetailData;
        protected readonly IUser viewer;
        protected readonly WinLossDetailService winLossDetailService;

        public ConsolidationTestsBaseFunctions(UserFixture userFixture)
        {
            user = userFixture.User;
            productService = Substitute.For<ProductService>();
            productDetailService = Substitute.For<ProductDetailService>();
            reportService = Substitute.For<ConsolidationService>();

            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetProductName(Arg.Any<int>()).Returns(callInfo => "Product " + callInfo.Args()[0]);

            Func<int, string> getProductDisplayName = (id) => "Product " + id;
            var products = new List<Product>
            {
                new Product(1, "Product 1", null, 0, 0, getProductDisplayName),
                new Product(2, "Product 2", null, 0, 0, getProductDisplayName)
            };

            reportService.GetProducts(Arg.Any<GetConslidationReportProductsParam>()).Returns(products);

            reportService.GetCurrencies(Arg.Any<GetCurrencyParam>()).Returns(new List<CurrencyModel>
            {
                new CurrencyModel(1, "Currency 1"),
                new CurrencyModel(2, "Currency 2")
            });

            reportService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>(), Arg.Any<DateTime>()).Returns(new List<int> { 1 });

            reportService.GetConsolidationExcludeProduct(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>()).Returns(new ConsolidationExcludeProduct(products));

            anyValueCreatorFixture = new AnyValueCreatorFixture();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            winLossDetailData = new PagedReportData<WinLossDetailModel>(
               Enumerable.Empty<WinLossDetailModel>(),
               0,
               0,
               0,
               null);

            viewer = Substitute.For<IUser>();
            viewer.LevelId.Returns((byte)2);

            winLossDetailService = Substitute.For<WinLossDetailService>();
            winLossDetailService.GetWinLossDetail(Arg.Any<WinLossDetailParam>()).Returns(winLossDetailData);

            productDetailService.GetProductDetails(Arg.Any<int>()).Returns(new List<ProductDetail>
            {
                new ProductDetail("ProductDetail", 1, 42, 150, 0, 1) { DisplayName = "A" },
                new ProductDetail("ProductDetail", 2, 42, 200, 0, 2) { DisplayName = "B" },
            });
            productDetailService.FilterResourceIdsByCustomer(Arg.Any<int>(), Arg.Any<string>()).Returns(new List<int> { 1, 2 });
            productDetailService.BuildProductDetailsXML(Arg.Any<int>(), Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>()).Returns(string.Empty);
            productDetailService.GetProductDetailsForConsolidationReport(Arg.Any<int>(), Arg.Any<IEnumerable<Product>>()).Returns(new List<ProductDetail>
            {
                new ProductDetail("ProductDetail", 1, 42, 150, 0, 1) { DisplayName = "A" },
                new ProductDetail("ProductDetail", 2, 42, 200, 0, 2) { DisplayName = "B" },
            });
            productDetailService.FilterResourceIdsByCustomerForConsolidationReport(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<IEnumerable<Product>>()).Returns(new List<int> { 1, 2 });
            productDetailService.GetSabaclubNameExportExcel(Arg.Any<IEnumerable<int>>()).Returns("Saba.club");
            productService.ConcatProductNames(Arg.Any<IEnumerable<int>>(), 42, "Saba.club").Returns("1,33");
        }
    }
}
