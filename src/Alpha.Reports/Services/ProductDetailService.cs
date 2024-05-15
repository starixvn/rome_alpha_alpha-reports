namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using AlphaPlus.Configurations;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface ProductDetailService
    {
        IEnumerable<int> FilterResourceIdsByCustomer(int viewerId, string site, int currencyId, string resourceIds);

        IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds, IEnumerable<Product> products);

        IEnumerable<ProductDetail> GetProductDetails(int viewerId, string site, int currencyId);

        IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products);

        string BuildProductDetailsXML(IEnumerable<int> validResourceIds);

        IDictionary<int, string> GetProductNamesExportExcel(
            IEnumerable<int> selectedResourceIds,
            IEnumerable<ProductDetail> productDetails = null);

        // Async methods
        Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId);

        IEnumerable<string> GetProductDetailBelongSabaOtherSports(int viewerId, string site, int currencyId);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.Reports.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly Func<string, int, int, DateTime?, bool> canViewProduct;

        public ProductDetailServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService,
            Func<string, int, int, DateTime?, bool> canViewProduct)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            this.canViewProduct = canViewProduct;
        }

        public IEnumerable<int> FilterResourceIdsByCustomer(int viewerId, string site, int currencyId, string resourceIds)
            => FilterResourceIdsByCustomer(GetProductDetails(viewerId, site, currencyId), resourceIds);

        public IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds, IEnumerable<Product> products)
            => FilterResourceIdsByCustomer(GetProductDetails(custId, products), resourceIds);

        public IEnumerable<ProductDetail> GetProductDetails(int viewerId, string site, int currencyId)
            => GroupByProductDetail(GetOfferedProductDetail(viewerId, site, currencyId));

        public IEnumerable<ProductDetail> GetProductDetails(int custId, IEnumerable<Product> products)
            => GroupByProductDetail(GetAllProductDetails()
                .Where(p => products.Any(i => i.Id == p.ProductId) && IsSupportProductDetail(p.ProductId) && CanViewProductDetailByResourceId(p.ResourceId)));

        public Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId)
            => Task.FromResult(GetProductDetails(viewerId, site, currencyId));

        public string BuildProductDetailsXML(IEnumerable<int> validResourceIds)
        {
            var validProductDetails = new List<ProductDetail>();

            GetAllProductDetails()
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    validProductDetails.AddRange(group.Where(p => validResourceIds.Contains(p.ResourceId)));

                });

            return BuildProductDetailsXML(validProductDetails);

            string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
            {
                if (productDetails.Any())
                {
                    var xml = new StringBuilder("<ROOT>");
                    foreach (var product in productDetails)
                    {
                        xml.Append(product.ToString());
                    }

                    xml.Append("</ROOT>");

                    return xml.ToString();
                }

                return string.Empty;
            }
        }

        public IDictionary<int, string> GetProductNamesExportExcel(
            IEnumerable<int> selectedResourceIds,
            IEnumerable<ProductDetail> productDetails = null)
        {
            var productNames = new Dictionary<int, string>();

            (productDetails ?? GroupByProductDetail(GetAllProductDetails()))
                .Where(i => CanViewProductDetailByResourceId(i.ResourceId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (IsSupportProductDetail(group.Key))
                    {
                        var allResourceIds = group.Select(p => p.ResourceId).ToList();
                        var intersectResourceIds = selectedResourceIds.Intersect(allResourceIds).ToList();

                        if (intersectResourceIds.Count > 0
                            && intersectResourceIds.Count != allResourceIds.Count)
                        {
                            productNames[group.Key] = string.Join(", ", intersectResourceIds.Select(id => cacheRefService.GetProductDetailName(id)));
                        }
                    }
                });

            return productNames;
        }

        public IEnumerable<string> GetProductDetailBelongSabaOtherSports(int viewerId, string site, int currencyId)
            => GetOfferedProductDetail(viewerId, site, currencyId)
                .Where(i => i.ResourceId == ProductDetaiIdConstants.SabaOtherSports)
                .OrderBy(product => product.Order)
                .Select(i => cacheRefService.GetProductDetailName(i.ChildResourceId));

        private IEnumerable<ProductDetail> GetOfferedProductDetail(int viewerId, string site, int currencyId)
        {
            var canViewProductDetailsByProduct = CanViewProductDetailsByProduct(site, currencyId);

            var details = ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{viewerId}",
                () => GetAllProductDetails()
                    .Where(p => canViewProductDetailsByProduct.ContainsKey(p.ProductId) && CanViewProductDetailByResourceId(p.ResourceId)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));

            return details;
        }

        private static IEnumerable<int> FilterResourceIdsByCustomer(IEnumerable<ProductDetail> productDetails, string resourceIds)
        {
            if (IsSelectAllProductDetails(resourceIds))
            {
                return productDetails.Select(p => p.ResourceId);
            }

            var resourceIdsHash = resourceIds.ToIntDictionary();

            return productDetails
                .Where(p => resourceIdsHash.ContainsKey(p.ResourceId))
                .Select(p => p.ResourceId);
        }

        private IEnumerable<ProductDetail> GetAllProductDetails()
            => ConfiguredCacheService.GetOrSet(
                ProductDetailsCacheKey,
                () => dynamicRepository.Fetch<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static bool IsSelectAllProductDetails(string resourceIds)
            => resourceIds != null && resourceIds == DefaultVariables.AllProductDetails;

        private IDictionary<int, bool> CanViewProductDetailsByProduct(string site, int currencyId)
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            foreach (var product in SiteSettings.ProductOfferProductDetail)
            {
                if (canViewProduct(site, currencyId, product, null))
                {
                    canViewProductDetails[product] = true;
                }
            }

            return canViewProductDetails;
        }

        private bool CanViewProductDetailByResourceId(int resourceId)
        {
            if (AppSettings.IsSiteLabel777)
            {
                return !ProductDetaiIdConstants.ExcludedProductDetailFor777.Contains(resourceId);
            }

            return true;
        }

        private static bool IsSupportProductDetail(int productId)
            => SiteSettings.ProductOfferProductDetail.Any(i => i == productId);

        private IEnumerable<ProductDetail> GroupByProductDetail(IEnumerable<ProductDetail> productDetails)
            => productDetails
                .GroupBy(r => r.ResourceId)
                .Select(products =>
                {
                    var product = products.FirstOrDefault();
                    product.DisplayName = cacheRefService.GetProductDetailName(product.ResourceId);

                    return product;
                })
                .OrderBy(product => product.ProductId)
                .ThenBy(product => product.Order);
    }
}