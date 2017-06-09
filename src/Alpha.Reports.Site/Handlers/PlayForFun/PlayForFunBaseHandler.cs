namespace Alpha.Reports.Site.Handlers.PlayForFun
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using App_GlobalResources;
    using Constants;
    using Params;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;
    using Services;
    using ViewModels.PlayForFun;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class PlayForFunBaseHandler : BaseHandler
    {
        protected readonly ProductService productService;
        protected readonly PlayForFunService reportService;

        public PlayForFunBaseHandler(ProductService productService, PlayForFunService reportService)
        {
            this.productService = productService;
            this.reportService = reportService;
        }

        protected internal IEnumerable<SelectListItem> BuildStatusSelectListItems(int userSelectedStatusId)
        {
            var statuses = new List<StatusModel>
            {
                new StatusModel(StatusIdConstants.All, Resources.All),
                new StatusModel(StatusIdConstants.Enabled, Resources.Enabled),
                new StatusModel(StatusIdConstants.Disabled, Resources.Disabled)
            };

            return statuses.Select(status =>
            {
                var statusSelectListItem = new SelectListItem
                {
                    Text = status.Name,
                    Value = status.Id.ToString(),
                    Selected = userSelectedStatusId == status.Id
                };

                return statusSelectListItem;
            });
        }

        protected PlayForFunParam CreatePlayForFunParam(PlayForFunQuery message, List<Product> products)
        {
            var firstProductId = products.Any() ? products[0].Id : 0;

            return new PlayForFunParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.UserSelectedProductId == 0 ? firstProductId : message.UserSelectedProductId,
                message.UserSelectedStatusId,
                message.CustLevel);
        }

        protected internal string GetStatusName(int statusId)
        {
            switch (statusId)
            {
                case StatusIdConstants.Enabled:
                    return Resources.Enabled;
                case StatusIdConstants.Disabled:
                    return Resources.Disabled;
                case StatusIdConstants.All:
                    return Resources.All;
                default:
                    return string.Empty;
            }
        }

        protected MasterAgentPlayForFunModel BuildMasterPlayForFunTotal(IEnumerable<MasterAgentPlayForFunModel> models)
        {
            var total = new MasterAgentPlayForFunModel(0, string.Empty, 0, 0, 0, 0);

            foreach (var model in models)
            {
                total.Add(model);
            }

            return total;
        }

        protected MemberPlayForFunViewModel BuildMemberPlayForFunTotal(IEnumerable<MemberPlayForFunModel> models)
        {
            var total = new MemberPlayForFunViewModel(0, string.Empty, 0, 0, 0, false, string.Empty);

            foreach (var model in models)
            {
                total.Add(model);
            }

            return total;
        }
    }
}