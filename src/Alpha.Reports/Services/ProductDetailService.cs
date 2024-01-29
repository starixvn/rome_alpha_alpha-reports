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

        IEnumerable<int> FilterResourceIdsByCustomerForConsolidationReport(int custId, string resourceIds, IEnumerable<Product> products);

        IEnumerable<ProductDetail> GetProductDetails(int viewerId, string site, int currencyId);

        IEnumerable<ProductDetail> GetProductDetailsForConsolidationReport(int custId, IEnumerable<Product> products);

        string BuildProductDetailsXML(IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds);

        string BuildProductDetailsXML(
            IEnumerable<int> validProductIds,
            IEnumerable<int> validResourceIds,
            IEnumerable<ProductDetail> productDetails);

        IDictionary<int, string> GetProductNamesExportExcel(
            IEnumerable<int> selectedResourceIds,
            IEnumerable<ProductDetail> productDetails = null);

        IEnumerable<int> RemoveProductIds(IEnumerable<int> productIds, IEnumerable<int> userSelectedResourceIds);

        IEnumerable<int> RemoveProductIds(
            IEnumerable<int> productIds,
            IEnumerable<int> userSelectedResourceIds,
            IEnumerable<ProductDetail> productDetails);

        // Async methods
        Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId);
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

        public IEnumerable<int> FilterResourceIdsByCustomerForConsolidationReport(int custId, string resourceIds, IEnumerable<Product> products)
            => FilterResourceIdsByCustomer(GetProductDetailsForConsolidationReport(custId, products), resourceIds);

        public IEnumerable<ProductDetail> GetProductDetails(int viewerId, string site, int currencyId)
        {
            if (viewerId <= 0)
            {
                return Enumerable.Empty<ProductDetail>();
            }

            var productSupportDetail = CanViewProductDetails(site, currencyId);

            return ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{viewerId}",
                () => GetProductDetails()
                    .Where(p => productSupportDetail.ContainsKey(p.ProductId) && SportsbookSupportDetail(p))
                    .Select(product =>
                    {
                        product.DisplayName = GetProductDetailName(product.RefId, product.ResourceId);

                        return product;
                    }),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));
        }

        public IEnumerable<ProductDetail> GetProductDetailsForConsolidationReport(int custId, IEnumerable<Product> products)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<ProductDetail>();
            }

            var canViewProductDetails = CanViewProductDetailsForConsolidationReport(products);

            return GetProductDetails()
                .Where(p => canViewProductDetails.ContainsKey(p.ProductId) && SportsbookSupportDetail(p))
                .Select(product =>
                {
                    product.DisplayName = GetProductDetailName(product.RefId, product.ResourceId);

                    return product;
                });
        }

        public string BuildProductDetailsXML(IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds)
            => BuildProductDetailsXML(validProductIds, validResourceIds, GetProductDetails());

        public string BuildProductDetailsXML(
            IEnumerable<int> validProductIds,
            IEnumerable<int> validResourceIds,
            IEnumerable<ProductDetail> productDetails)
        {
            var validProductDetails = new List<ProductDetail>();

            productDetails
                .Where(p => validProductIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (IsSupportProductDetail(group.Key))
                    {
                        validProductDetails.AddRange(group.Where(p => validResourceIds.Contains(p.ResourceId)));
                    }
                });

            return validProductDetails.Count == 0
                ? string.Empty
                : BuildProductDetailsXML(validProductDetails);
        }

        private static string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
        {
            var xml = new StringBuilder("<ROOT>");
            foreach (var product in productDetails)
            {
                foreach (var leagueId in product.LeagueGroupIds)
                {
                    foreach (var typeId in product.TicketTypeIds)
                    {
                        xml.AppendFormat(
                            "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
                            product.ProductId,
                            product.SportType,
                            product.BetType,
                            leagueId,
                            typeId);
                    }
                }
            }

            xml.Append("</ROOT>");

            return xml.ToString();
        }

        public IDictionary<int, string> GetProductNamesExportExcel(
            IEnumerable<int> selectedResourceIds,
            IEnumerable<ProductDetail> productDetails = null)
        {
            var productNames = new Dictionary<int, string>();

            (productDetails ?? GetProductDetails())
                .Where(i => SportsbookSupportDetail(i))
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
                            productNames[group.Key] = string.Join(", ", intersectResourceIds.Select(id => GetProductDetailName("ProductDetail", id)));
                        }
                    }
                });

            return productNames;
        }

        public IEnumerable<int> RemoveProductIds(IEnumerable<int> productIds, IEnumerable<int> userSelectedResourceIds)
            => RemoveProductIds(productIds, userSelectedResourceIds, GetProductDetails());

        public IEnumerable<int> RemoveProductIds(
            IEnumerable<int> productIds,
            IEnumerable<int> userSelectedResourceIds,
            IEnumerable<ProductDetail> productDetails)
        {
            var removedProductIds = new List<int>(productIds);

            productDetails
                .Where(p => productIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (IsSupportProductDetail(group.Key))
                    {
                        var intersectResourceIds = group.Select(p => p.ResourceId).Intersect(userSelectedResourceIds);

                        if (!intersectResourceIds.Any())
                        {
                            removedProductIds.RemoveAll(id => id == group.Key);
                        }
                    }
                });

            return removedProductIds;
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

        private IEnumerable<ProductDetail> GetProductDetails()
            => ConfiguredCacheService.GetOrSet(
                ProductDetailsCacheKey,
                () => dynamicRepository.Fetch<ProductDetail>(new GetProductsDetailCriteria()).OrderBy(product => product.ProductId).ThenBy(product => product.Order),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static bool IsSelectAllProductDetails(string resourceIds)
            => resourceIds != null && resourceIds == DefaultVariables.AllProductDetails;

        private IDictionary<int, bool> CanViewProductDetails(string site, int currencyId)
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

        private static IDictionary<int, bool> CanViewProductDetailsForConsolidationReport(IEnumerable<Product> products)
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            products
                .ToList()
                .ForEach(product =>
                {
                    if (IsSupportProductDetail(product.Id))
                    {
                        canViewProductDetails[product.Id] = true;
                    }
                });

            return canViewProductDetails;
        }

        private static bool IsSupportProductDetail(int productId)
            => SiteSettings.ProductOfferProductDetail.Any(i => i == productId);

        private string GetProductDetailName(string refId, int resourceId)
        {
            if (AppSettings.IsSiteLabel777 && ProductDetaiIdConstants.SportsbookDetailFor777Site.Contains(resourceId))
            {
                var name = cacheRefService.GetResourceDataByRefIdAndResourceId($"{refId}_Saba", resourceId);

                return string.IsNullOrEmpty(name) ? cacheRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId) : name;
            }

            return cacheRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId);
        }

        private bool SportsbookSupportDetail(ProductDetail detail)
        {
            if (AppSettings.IsSiteLabel777 && detail.ProductId == ProductIdConstants.Sportsbook)
            {
                return ProductDetaiIdConstants.SportsbookDetailFor777Site.Contains(detail.ResourceId);
            }

            return true;
        }

        // Async methods
        public Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId)
        {
            if (viewerId <= 0)
            {
                return Task.FromResult(Enumerable.Empty<ProductDetail>());
            }

            var canViewProductDetails = CanViewProductDetails(site, currencyId);

            var result = ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{viewerId}",
                () => GetProductDetails()
                    .Where(p => canViewProductDetails.ContainsKey(p.ProductId) && SportsbookSupportDetail(p))
                    .Select(product =>
                    {
                        product.DisplayName = cacheRefService.GetResourceDataByRefIdAndResourceId(product.RefId, product.ResourceId);

                        return product;
                    }),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));

            return Task.FromResult(result);
        }
    }
}