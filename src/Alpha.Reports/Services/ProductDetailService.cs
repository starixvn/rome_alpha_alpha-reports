namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Alpha.Reports.Configurations;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface ProductDetailService
    {
        IEnumerable<int> FilterResourceIdsByCustomer(int viewerId, string site, int currencyId, string resourceIds);

        IEnumerable<int> FilterResourceIdsByCustomerForConsolidationReport(int custId, string resourceIds, IEnumerable<Product> products);

        IEnumerable<ProductDetail> GetProductDetails(int viewerId, string site, int currencyId);

        IEnumerable<ProductDetail> GetProductDetailsForConsolidationReport(int custId, IEnumerable<Product> products);

        string BuildProductDetailsXML(IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds);

        IDictionary<int, string> GetProductNamesExportExcel(IEnumerable<int> selectedResourceIds);

        IEnumerable<int> RemoveProductIds(IEnumerable<int> productIds, IEnumerable<int> userSelectedResourceIds);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.Reports.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly Func<int, string, int, int, bool> canViewBySite;
        private readonly Func<int, bool> canViewProduct;

        public ProductDetailServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService,
            Func<int, string, int, int, bool> canViewBySite,
            Func<int, bool> canViewProduct)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            this.canViewBySite = canViewBySite;
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

            var canViewProductDetails = CanViewProductDetails(viewerId, site, currencyId);

            return ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{viewerId}",
                () => GetProductDetails()
                    .Where(p => canViewProductDetails.ContainsKey(p.ProductId))
                    .Select(product =>
                    {
                        product.DisplayName = cacheRefService.GetResourceDataByRefIdAndResourceId(product.RefId, product.ResourceId);

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
                .Where(p => canViewProductDetails.ContainsKey(p.ProductId))
                .Select(product =>
                {
                    product.DisplayName = cacheRefService.GetResourceDataByRefIdAndResourceId(product.RefId, product.ResourceId);

                    return product;
                });
        }

        public string BuildProductDetailsXML(IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds)
        {
            var validProductDetails = new List<ProductDetail>();

            GetProductDetails()
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

            return validProductDetails.Any()
                ? BuildProductDetailsXML(validProductDetails)
                : string.Empty;
        }

        public IDictionary<int, string> GetProductNamesExportExcel(IEnumerable<int> selectedResourceIds)
        {
            var productNames = new Dictionary<int, string>();

            GetProductDetails()
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

        public IEnumerable<int> RemoveProductIds(IEnumerable<int> productIds, IEnumerable<int> userSelectedResourceIds)
        {
            var removedProductIds = new List<int>(productIds);

            GetProductDetails()
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
                () => dynamicRepository.Fetch<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

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

        private static bool IsSelectAllProductDetails(string resourceIds)
            => resourceIds != null && resourceIds == DefaultVariables.AllProductDetails;

        private IDictionary<int, bool> CanViewProductDetails(int viewerId, string site, int currencyId)
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            foreach (var product in SiteSettings.ProductOfferProductDetail)
            {
                if (canViewBySite(viewerId, site, currencyId, product) || canViewProduct(product))
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
    }
}
