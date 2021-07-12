namespace Alpha.Reports.Site.Handlers.SuperWinLossDetailByPT
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Models.SuperWinLossDetailByPT;
    using Params.SuperWinLossDetailByPT;
    using Requests.SuperWinLossDetailByPT;
    using Services;
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
            IEnumerable<SuperWinLossDetailByProductModel> winLossDetailByProducts)
        {
            return winLossDetailByProducts.Join(
                positionTakings,
                winLossDetailByProduct => winLossDetailByProduct.ProductId,
                postionTaking => postionTaking.ProductId,
                (winLossDetailByProduct, positionTaking) => new SuperWinLossDetailModel(winLossDetailByProduct, positionTaking))
                .GroupBy(
                winLossDetailByPT => winLossDetailByPT.PositionTaking,
                (positionTaking, winLossDetailByPTs) => new SuperWinLossDetailByPTModel(positionTaking, winLossDetailByPTs));
        }

        protected IEnumerable<SuperPtProductViewModel> GeneratePtProducts(
            IEnumerable<SuperPTModel> positionTakings,
            IEnumerable<Product> products)
        {
            return products.Join(
                positionTakings,
                product => product.Id,
                positionTaking => positionTaking.ProductId,
                (product, positionTaking) => new SuperPtProductModel(
                    positionTaking.SuperPT,
                    product.DisplayName))
                .GroupBy(
                ptProduct => ptProduct.PositionTaking,
                (positionTaking, ptProducts) => new SuperPtProductViewModel(
                    positionTaking,
                    string.Join(", ", ptProducts.Select(product => product.ProductName))));
        }
    }
}