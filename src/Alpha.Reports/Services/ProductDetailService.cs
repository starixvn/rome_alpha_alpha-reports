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
        IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds);

        IEnumerable<ProductDetail> GetProductDetails(int custId);

        string BuildProductDetailsXML(int custId, IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.Reports.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly Func<int, bool> canViewProduct;

        public ProductDetailServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService,
            Func<int, bool> canViewProduct)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            this.canViewProduct = canViewProduct;
        }

        public IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds)
        {
            var productDetails = GetProductDetails(custId);
            var resourceIdsHash = resourceIds.ToIntDictionary();

            if (resourceIdsHash.Count > 0)
            {
                productDetails = productDetails.Where(p => resourceIdsHash.ContainsKey(p.ResourceId));
            }

            return productDetails.Select(p => p.ResourceId);
        }

        public IEnumerable<ProductDetail> GetProductDetails(int custId)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<ProductDetail>();
            }

            return ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{custId}",
                () => GetProductDetails()
                    .Where(p => canViewProduct(p.ProductId) && CanViewSabaClubBet(p.ProductId))
                    .Select(product =>
                    {
                        product.DisplayName = cacheRefService.GetResourceDataByRefIdAndResourceId(product.RefId, product.ResourceId);

                        return product;
                    }),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));
        }

        private IEnumerable<ProductDetail> GetProductDetails()
            => ConfiguredCacheService.GetOrSet(
                ProductDetailsCacheKey,
                () => dynamicRepository.Fetch<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static bool CanViewSabaClubBet(int productId)
            => productId == ProductIdConstants.SabaClub && SiteSettings.Is789y;

        public string BuildProductDetailsXML(int custId, IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds)
        {
            var validProductDetails = GetProductDetails()
                .Where(p => validProductIds.Contains(p.ProductId));

            if (SiteSettings.Is789y)
            {
                validProductDetails = validProductDetails.Where(p => validResourceIds.Contains(p.ResourceId));
            }

            return validProductDetails.Any()
                ? BuildProductDetailsXML(validProductDetails)
                : string.Empty;
        }

        private static string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
        {
            var xml = new StringBuilder("<ROOT>");
            productDetails.ToList().ForEach(product => xml.AppendFormat(
                "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\"/>",
                product.ProductId,
                product.SportType,
                product.BetType));
            xml.Append("</ROOT>");

            return xml.ToString();
        }
    }
}
