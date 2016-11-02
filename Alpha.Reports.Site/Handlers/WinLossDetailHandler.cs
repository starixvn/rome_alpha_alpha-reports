namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediatR;
    using Models;
    using Models.WinlossDetail;
    using Params;
    using Requests.WinLossDetail;
    using Services;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        IRequestHandler<WinLossDetailQuery<MasterWinLossDetail>, WinLossDetailViewModel<MasterWinLossDetail>>,
        IRequestHandler<WinLossDetailQuery<AgentWinLossDetail>, WinLossDetailViewModel<AgentWinLossDetail>>,
        IRequestHandler<WinLossDetailQuery<MemberWinLossDetail>, WinLossDetailViewModel<MemberWinLossDetail>>
    {
        
        private readonly ProductService productService;
        private readonly WinLossDetailService reportService;
        private readonly ApplicationContext applicationContext;

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService,
            ApplicationContext applicationContext)
        {
            this.productService = productService;
            this.reportService = reportService;
            this.applicationContext = applicationContext;
        }

        public WinLossDetailViewModel<MasterWinLossDetail> Handle(WinLossDetailQuery<MasterWinLossDetail> message)
        {
            message.UserLevel = UserLevel.Super;

            return CreateViewModel<MasterWinLossDetail, WinLossDetailViewModel<MasterWinLossDetail>>(message);
        }

        public WinLossDetailViewModel<AgentWinLossDetail> Handle(WinLossDetailQuery<AgentWinLossDetail> message)
        {
            message.UserLevel = UserLevel.Master;

            return CreateViewModel<AgentWinLossDetail, WinLossDetailViewModel<AgentWinLossDetail>>(message);
        }

        public WinLossDetailViewModel<MemberWinLossDetail> Handle(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            message.UserLevel = UserLevel.Agent;

            return CreateViewModel<MemberWinLossDetail, WinLossDetailViewModel<MemberWinLossDetail>>(message);
        }

        private TViewModel CreateViewModel<TModel, TViewModel>(WinLossDetailQuery<TModel> message)
            where TModel : MemberWinLossDetail
            where TViewModel : WinLossDetailViewModel<TModel>, new()
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId);
            var param = CreateParamater(message, products);

            var reportData = reportService.GetWinLossDetail<TModel>(param);

            var viewModel = new TViewModel
            {
                CustId = param.CustId,
                CurrentDate = DateTime.Now,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = products,
                SelectedProducts = string.Join(",", param.ProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = applicationContext.OldMainAppPath,

                ReportData = reportData,
                SelectedDateRange = message.SelectedDateRange
            };

            return viewModel;
        }

        private WinLossDetailParam CreateParamater<TModel>(
            WinLossDetailQuery<TModel> message,
            IEnumerable<Product> userProducts) where TModel : MemberWinLossDetail
        {
            var selectedProducts = string.IsNullOrWhiteSpace(message.SelectedProducts)
                ? string.Join(",", userProducts.Select(product => product.Id))
                : message.SelectedProducts;

            return new WinLossDetailParam(
                message.CustId,
                message.PageIndex,
                message.PageSize,
                message.FromDate,
                message.ToDate,
                selectedProducts,
                message.UserLevel,
                message.SortedColumn,
                message.SortDirection,
                message.IsHistoricReport);
        }
    }
}