namespace Alpha.Reports.Site.Features.SuperWinLossDetailByPT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Params;
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.Requests;
    using Alpha.Reports.Site.Features.SuperWinLossDetailByPT.ViewModels;
    using Alpha.Reports.Site.Handlers;

    public class SuperWinLossDetailByPTBaseHandler : BaseHandler
    {
        private readonly CacheRefService refService;
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly SuperWinLossDetailByPTService reportService;

        public SuperWinLossDetailByPTBaseHandler(
            CacheRefService refService,
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService)
        {
            this.refService = refService;
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.reportService = reportService;
        }

        protected static SuperPTParam CreateSuperPTParam(int superId) => new SuperPTParam(superId);

        protected SuperWinLossDetailByPTParam CreateSuperWinLossDetailByPTParam(SuperWinLossDetailByPTQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIds(
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds);

            return new SuperWinLossDetailByPTParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                validUserSelectedProductIds,
                validUserSelectedResourceIds,
                productDetailService.BuildProductDetailsXML(validUserSelectedResourceIds),
                message.LimitedDateToShowInfo);
        }

        protected static SuperWinLossDetailModel AccumulateGrandTotal(IEnumerable<SuperWinLossDetailByPTModel> reportData)
        {
            var grandTotal = new SuperWinLossDetailModel();
            foreach (var winLossDetailByPT in reportData)
            {
                grandTotal.Add(winLossDetailByPT.Total);
            }

            return grandTotal;
        }

        protected IEnumerable<SuperWinLossDetailByPTModel> GenerateReportData(
            IEnumerable<SuperPTModel> positionTakings,
            IEnumerable<SuperWinLossDetailByProductModel> winLossDetailByProducts,
            string sortPropertyName,
            bool isDescendingSort)
        {
            var sortPropertyNameOnTotal = sortPropertyName.Equals("ProductId", StringComparison.OrdinalIgnoreCase)
                ? "PositionTaking"
                : "Total." + sortPropertyName;

            return winLossDetailByProducts.Join(
                positionTakings,
                winLossDetailByProduct => winLossDetailByProduct.ProductId,
                postionTaking => postionTaking.ProductId,
                (winLossDetailByProduct, positionTaking) =>
                {
                    var productName = refService.GetProductName(positionTaking.ProductId);
                    return new SuperWinLossDetailModel(
                        winLossDetailByProduct,
                        new SuperPTModel(
                            positionTaking.CustId,
                            positionTaking.SuperPT,
                            positionTaking.ProductId,
                            productName));
                })
                .GroupBy(
                winLossDetail => winLossDetail.PositionTaking,
                (positionTaking, winLossDetails) =>
                {
                    var orderedWinLossDetails = winLossDetails.OrderBy(sortPropertyName, isDescendingSort);
                    return new SuperWinLossDetailByPTModel(positionTaking, orderedWinLossDetails);
                })
                .OrderBy(sortPropertyNameOnTotal, isDescendingSort);
        }

        protected static IEnumerable<SuperPtProductViewModel> GeneratePtProducts(
            IEnumerable<SuperPTModel> positionTakings,
            IEnumerable<Product> products)
        {
            return products.Join(
                positionTakings,
                product => product.Id,
                positionTaking => positionTaking.ProductId,
                (product, positionTaking) => new SuperPtProductModel(positionTaking.SuperPT, product.DisplayName))
                .GroupBy(
                ptProduct => ptProduct.PositionTaking,
                (positionTaking, ptProducts) =>
                {
                    var productNames = string.Join(", ", ptProducts.Select(product => product.ProductName));
                    return new SuperPtProductViewModel(positionTaking, productNames);
                })
                .OrderBy(viewModel => viewModel.PositionTaking);
        }
    }
}