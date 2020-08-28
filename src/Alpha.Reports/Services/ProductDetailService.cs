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
    using Sunplus.Agent.Model.Entities.Permission;

    public interface ProductDetailService
    {
        IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds);

        IEnumerable<int> FilterResourceIdsByCustomerForConsolidationReport(int custId, string resourceIds, IEnumerable<Product> products);

        IEnumerable<ProductDetail> GetProductDetails(int custId);

        IEnumerable<ProductDetail> GetProductDetailsForConsolidationReport(int custId, IEnumerable<Product> products);

        string BuildProductDetailsXML(int custId, IEnumerable<int> validProductIds, IEnumerable<int> validResourceIds);

        string GetSabaclubNameExportExcel(IEnumerable<int> selectedResourceIds);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.Reports.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public ProductDetailServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<int> FilterResourceIdsByCustomer(int custId, string resourceIds)
            => FilterResourceIdsByCustomer(GetProductDetails(custId), resourceIds);

        public IEnumerable<int> FilterResourceIdsByCustomerForConsolidationReport(int custId, string resourceIds, IEnumerable<Product> products)
            => FilterResourceIdsByCustomer(GetProductDetailsForConsolidationReport(custId, products), resourceIds);

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

        public IEnumerable<ProductDetail> GetProductDetails(int custId)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<ProductDetail>();
            }

            var canViewProductDetails = CanViewProductDetails();

            return ConfiguredCacheService.GetOrSet(
                $"{ProductDetailsCacheKey}-{custId}",
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

        private IEnumerable<ProductDetail> GetProductDetails()
            => ConfiguredCacheService.GetOrSet(
                ProductDetailsCacheKey,
                () => dynamicRepository.Fetch<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

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

        private static bool IsSelectAllProductDetails(string resourceIds)
            => resourceIds != null && resourceIds == DefaultVariables.AllProductDetails;

        private static IDictionary<int, bool> CanViewProductDetails()
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            if (SiteSettings.Is789y && UserPermission.HasPermissionOnProduct(ProductIdConstants.SabaClub))
            {
                canViewProductDetails[ProductIdConstants.SabaClub] = true;
            }

            return canViewProductDetails;
        }

        private static IDictionary<int, bool> CanViewProductDetailsForConsolidationReport(IEnumerable<Product> products)
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            if (SiteSettings.Is789y && products.Any(p => p.Id == ProductIdConstants.SabaClub))
            {
                canViewProductDetails[ProductIdConstants.SabaClub] = true;
            }

            return canViewProductDetails;
        }

        public string GetSabaclubNameExportExcel(IEnumerable<int> selectedResourceIds)
        {
            if (SiteSettings.Is789y)
            {
                var productDetailIds = GetProductDetails()
                    .Where(p => p.ProductId == ProductIdConstants.SabaClub)
                    .Select(p => p.ResourceId);

                if (!IsEquals(productDetailIds, selectedResourceIds))
                {
                    return string.Join(", ", selectedResourceIds.Select(id => cacheRefService.GetProductDetailName(id)));
                }
            }

            return cacheRefService.GetProductName(ProductIdConstants.SabaClub);
        }

        private static bool IsEquals(IEnumerable<int> source, IEnumerable<int> destination)
            => Enumerable.SequenceEqual(source.OrderBy(id => id), destination.OrderBy(id => id));
    }
}
