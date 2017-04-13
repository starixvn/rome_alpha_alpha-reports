namespace Alpha.Reports.Site.Handlers.PlayForFun
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using MediatR;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;
    using Services;
    using ViewModels.PlayForFun;

    public class PlayForFunHandler :
        PlayForFunBaseHandler,
        IRequestHandler<MasterOrAgentPlayForFunQuery, MasterOrAgentPlayForFunReportViewModel>,
        IRequestHandler<MemberPlayForFunQuery, MemberPlayForFunReportViewModel>
    {
        public PlayForFunHandler(ProductService productService, PlayForFunService reportService)
            : base(productService, reportService)
        {
        }

        public MasterOrAgentPlayForFunReportViewModel Handle(MasterOrAgentPlayForFunQuery message)
        {
            var products = reportService.SupportedProduct(message.CustId).ToList();
            var param = CreatePlayForFunParam(message, products);
            var reportData = reportService.MasterOrAgentPlayForFun(param);

            return new MasterOrAgentPlayForFunReportViewModel(
                message,
                param,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(new List<int> { param.ProductId }, products),
                BuildStatusSelectListItems(message.UserSelectedStatusId),
                new ReportData<PlayForFunModel>(reportData, BuildMasterPlayForFunTotal(reportData)));
        }

        public MemberPlayForFunReportViewModel Handle(MemberPlayForFunQuery message)
        {
            var products = reportService.SupportedProduct(message.CustId).ToList();
            var param = CreatePlayForFunParam(message, products);
            var reportData = reportService.MemberPlayForFun(param);

            return new MemberPlayForFunReportViewModel(
                message,
                param,
                BuildBreadcrumbLevel(message),
                BuildProductSelectListItems(new List<int> { param.ProductId }, products),
                BuildStatusSelectListItems(message.UserSelectedStatusId),
                BuildMemberPlayForFunViewModel(reportData));
        }

        protected ReportData<PlayForFunModel> BuildMemberPlayForFunViewModel(IEnumerable<MemberPlayForFunModel> models)
        {
            var reportData = new List<MemberPlayForFunViewModel>();

            foreach (var model in models)
            {
                reportData.Add(new MemberPlayForFunViewModel(
                    model.CustId,
                    model.CustName,
                    model.Turnover,
                    model.WinLoss,
                    model.BetCount,
                    model.StatusId == StatusIdConstants.Disabled,
                    GetStatusName(model.StatusId)));
            }

            return new ReportData<PlayForFunModel>(reportData, BuildMemberPlayForFunTotal(reportData));
        }
    }
}