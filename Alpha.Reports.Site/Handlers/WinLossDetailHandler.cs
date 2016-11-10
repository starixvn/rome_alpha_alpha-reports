namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Extensions;
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

        public WinLossDetailHandler(
            ProductService productService,
            WinLossDetailService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
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

            var winLossDetailParam = CreateParamater(message, reportLevel);

            var reportData = reportService.GetWinLossDetail<TModel>(winLossDetailParam);

            var viewModel = new TViewModel
            {
                CustId = winLossDetailParam.CustId,
                FromDate = winLossDetailParam.FromDate,
                ToDate = winLossDetailParam.ToDate,

                Products = BuildProductSelectListItems(winLossDetailParam.UserSelectedProductIds, products),
                UserSelectedProductIds = string.Join(",", winLossDetailParam.UserSelectedProductIds),
                Language = message.Language,
                IsStackMode = message.IsStackMode,
                IsHistoricReport = message.IsHistoricReport,
                IsFilterVisible = message.IsFilterVisible,

                ReportData = reportData,
                UserSelectedDateRangeOption = message.UserSelectedDateRangeOption,
                IsDirectReportViewAction = message.IsDirectReportViewAction,

                SortPropertyName = winLossDetailParam.SortByPropertyName,
                IsDescendingSort = winLossDetailParam.IsDescendingSort
            };

            return viewModel;
        }

        private WinLossDetailParam CreateParamater<TModel>(
            WinLossDetailQuery<TModel> message,
            UserLevel reportLevel)
            where TModel : MemberWinLossDetail
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(
                message.CustId,
                string.IsNullOrWhiteSpace(message.UserSelectedProductIds) ? Enumerable.Empty<int>() : message.UserSelectedProductIds.ToListInt());

            return new WinLossDetailParam(
                 message.CustId,
                 message.PageIndex,
                 message.PageSize,
                 message.FromDate,
                 message.ToDate,
                 validUserSelectedProductIds,
                 reportLevel,
                 message.SortPropertyName,
                 message.IsDescendingSort,
                 message.IsHistoricReport);
        }

        private IEnumerable<SelectListItem> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
        {
            if (products == null
                || !products.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return products.Select(product =>
            {
                var productSelectListItem = new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = userSelectedProductIds.Contains(product.Id)
                };

                return productSelectListItem;
            });
        }
    }
}