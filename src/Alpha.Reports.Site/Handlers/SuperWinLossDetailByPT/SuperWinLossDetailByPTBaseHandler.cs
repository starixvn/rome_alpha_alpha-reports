namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.SuperWinLossDetailByPT;
    using Alpha.Reports.Services;
    using Params.SuperWinLossDetailByPT;
    using Requests.SuperWinLossDetailByPT;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.SuperWinLossDetailByPT;

    public class SuperWinLossDetailByPTBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly ProductDetailService productDetailService;
        protected readonly SuperWinLossDetailByPTService reportService;

        public SuperWinLossDetailByPTBaseHandler(
            ProductService productService,
            ProductDetailService productDetailService,
            SuperWinLossDetailByPTService reportService)
        {
            this.productService = productService;
            this.productDetailService = productDetailService;
            this.reportService = reportService;
        }

        protected SuperPTParam CreateSuperPTParam(int superId) => new SuperPTParam(superId);

        protected SuperWinLossDetailByPTParam CreateSuperWinLossDetailByPTParam(SuperWinLossDetailByPTQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                message.Site,
                message.CurrencyId,
                message.UserSelectedProductIds.ToListInt(),
                message.MinDate);
            var validUserSelectedResourceIds = productDetailService.FilterResourceIdsByCustomer(
                message.CustId,
                message.Site,
                message.CurrencyId,
                !message.IsPostBack ? DefaultVariables.AllProductDetails : message.UserSelectedResourceIds);
            validUserSelectedProductIds = productDetailService.RemoveProductIds(validUserSelectedProductIds, validUserSelectedResourceIds);

            return new SuperWinLossDetailByPTParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                validUserSelectedProductIds,
                validUserSelectedResourceIds,
                productDetailService.BuildProductDetailsXML(validUserSelectedProductIds, validUserSelectedResourceIds));
        }

        protected SuperWinLossDetailModel AccumulateGrandTotal(IEnumerable<SuperWinLossDetailByPTModel> reportData)
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
                (winLossDetailByProduct, positionTaking) => new SuperWinLossDetailModel(winLossDetailByProduct, positionTaking))
                .GroupBy(
                winLossDetail => winLossDetail.PositionTaking,
                (positionTaking, winLossDetails) =>
                {
                    var orderedWinLossDetails = winLossDetails.OrderBy(sortPropertyName, isDescendingSort);
                    return new SuperWinLossDetailByPTModel(positionTaking, orderedWinLossDetails);
                })
                .OrderBy(sortPropertyNameOnTotal, isDescendingSort);
        }

        protected IEnumerable<SuperPtProductViewModel> GeneratePtProducts(
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