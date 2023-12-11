namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface ProductService
    {
        IEnumerable<Product> GetProgressiveJackpotProduct(string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds);

        IEnumerable<Product> GetAllOfferedProducts(string site, int currencyId, DateTime? minReportDate = null);

        IEnumerable<int> FilterProductIds(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null);

        IEnumerable<Product> FilterProducts(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null);

        bool CanViewProduct(string site, int currencyId, int productId, DateTime? minReportDate = null);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        string ConcatProductNames(string productIds);

        string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames);

        IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData();

        IEnumerable<int> GetOfferedProductsNeedDelayTimeToSumarizeData(string site, int currencyId, DateTime? minReportDate = null);

        Task<IEnumerable<Product>> GetAllOfferedProductsAsync(
            string site,
            int currencyId,
            DateTime? minReportDate = null);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.Reports.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private const int SabaPromotionProductId = 37;
        private readonly IUser user;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName,
            IUser user)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
            this.user = user;
        }

        public IEnumerable<Product> GetProgressiveJackpotProduct(string site, int currencyId, IEnumerable<int> progressiveJackpotproductIds)
            => FilterProducts(site, currencyId, progressiveJackpotproductIds);

        public IEnumerable<int> FilterProductIds(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null)
        {
            var customerProductIds = GetAllOfferedProducts(site, currencyId, minReportDate).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> FilterProducts(string site, int currencyId, IEnumerable<int> productIds, DateTime? minReportDate = null)
            => GetAllOfferedProducts(site, currencyId, minReportDate).Where(product => productIds.Contains(product.Id));

        public Task<IEnumerable<Product>> GetAllOfferedProductsAsync(
            string site,
            int currencyId,
            DateTime? minReportDate = null)
        {
            var products = cacheService.GetOrSet(
                $"{ProductsCacheKey}--{site}--{currencyId}",
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria(site, currencyId))
                    .Where(i => !i.IsHide)
                    .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));
            products = products.Where(i => IsRemainSupportProduct(i, minReportDate) && canViewProduct(i.Id));

            return Task.FromResult(GetOfferedProductsAndSabaPromotion(products));
        }

        public IEnumerable<Product> GetAllOfferedProducts(string site, int currencyId, DateTime? minReportDate = null)
        {
            var products = GetOfferedProducts(site, currencyId, minReportDate);
            return GetOfferedProductsAndSabaPromotion(products);
        }

        public bool CanViewProduct(string site, int currencyId, int productId, DateTime? minReportDate = null)
            => FilterProducts(site, currencyId, new List<int> { productId }, minReportDate).Any();

        public IEnumerable<string> GetProductNames(IEnumerable<int> productIds)
        {
            var productNames = new List<string>();

            if (productIds == null)
            {
                return productNames;
            }

            foreach (var productId in productIds)
            {
                productNames.Add(getProductName(productId));
            }

            return productNames;
        }

        public string ConcatProductNames(string productIds)
            => string.Join(", ", GetProductNames(productIds.ToListInt()));

        public string ConcatProductNames(IEnumerable<int> productIds, IDictionary<int, string> replacedProductNames)
            => productIds == null
            ? string.Empty
            : string.Join(", ", productIds.Select(id => replacedProductNames.TryGetValue(id, out var name) ? name : getProductName(id)));

        public IEnumerable<ProductsByRefTable> GetProductsNeedDelayTimeToSumarizeData()
        {
            var data = cacheService
              .GetOrSet(
              "Alpha.Reports.Services.Impl.ProductsNeedDelayTimeToSumarizeData",
              () => dynamicRepository.Fetch<ProductsByRefTable>(new GetProductsByRefTableCriteria(null)),
              new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return data;
        }

        public IEnumerable<int> GetOfferedProductsNeedDelayTimeToSumarizeData(string site, int currencyId, DateTime? minReportDate = null)
        {
            var data = GetProductsNeedDelayTimeToSumarizeData();

            return FilterProductIds(site, currencyId, data.Select(i => i.ProductId), minReportDate);
        }

        private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
            => !minReportDate.HasValue
            || !product.EndOfSupportDate.HasValue
            || minReportDate.Value <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetOfferedProducts(string site, int currencyId, DateTime? minReportDate)
        {
            var products = cacheService
                 .GetOrSet(
                $"{ProductsCacheKey}--{site}--{currencyId}--{user.Language}",
                () => GetProducts(),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return products.Where(i => canViewProduct(i.Id) && IsRemainSupportProduct(i, minReportDate));

            List<Product> GetProducts()
                => dynamicRepository.Fetch<Product>(new GetProductsCriteria(site, currencyId))
                    .Where(i => !i.IsHide)
                    .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, product.Type, product.OrderNo, getProductName)).ToList();
        }

        private IEnumerable<Product> GetOfferedProductsAndSabaPromotion(
            IEnumerable<Product> custProducts)
        {
            return custProducts;
        }
    }
}