namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
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
            return CreateViewModel<MasterWinLossDetail, WinLossDetailViewModel<MasterWinLossDetail>>(
                message, UserLevel.Super);
        }

        public WinLossDetailViewModel<AgentWinLossDetail> Handle(WinLossDetailQuery<AgentWinLossDetail> message)
        {
            return CreateViewModel<AgentWinLossDetail, WinLossDetailViewModel<AgentWinLossDetail>>(
                message, UserLevel.Master);
        }

        public WinLossDetailViewModel<MemberWinLossDetail> Handle(WinLossDetailQuery<MemberWinLossDetail> message)
        {
            return CreateViewModel<MemberWinLossDetail, WinLossDetailViewModel<MemberWinLossDetail>>(
                message, UserLevel.Agent);
        }

        private TViewModel CreateViewModel<TModel, TViewModel>(
            WinLossDetailQuery<TModel> message,
            UserLevel reportLevel)
            where TModel : MemberWinLossDetail
            where TViewModel : WinLossDetailViewModel<TModel>, new()
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var products = productService.GetProductsForCustomer(message.CustId);
            var param = CreateParamater(message, products, reportLevel);

            var reportData = reportService.GetWinLossDetail<TModel>(param);

            var viewModel = new TViewModel
            {
                CustId = param.CustId,
                FromDate = param.FromDate,
                ToDate = param.ToDate,

                Products = products,
                SelectedProducts = param.UserSelectedProductIds,
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                HideFilter = message.HideFilter,
                OldMainAppPath = applicationContext.OldMainAppPath,

                ReportData = reportData,
                SelectedDateRange = message.SelectedDateRange,

                SortedColumn = param.SortedColumn,
                SortDirection = param.SortDirection
            };

            return viewModel;
        }

        private WinLossDetailParam CreateParamater<TModel>(
            WinLossDetailQuery<TModel> message,
            IEnumerable<Product> userProducts,
            UserLevel reportLevel)
            where TModel : MemberWinLossDetail
        {
            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 message.UserSelectedProductIds,
                 reportLevel,
                 message.SortedColumn,
                 message.SortDirection,
                 message.IsHistoricReport,
                 userProducts);
        }
    }
}