namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.Shared.Requests;
    using Alpha.Reports.Site.ViewModels.WinLossDetail;
    using MediatR;

    public class WinLossDetailGetDataQuery : ReportDefaultDateRangeNewQuery, IAsyncRequest<WinLossDetailGetDataViewModel>
    {
        public WinLossDetailGetDataQuery(IUser viewer) : base(viewer)
        {
            IsEnableSabaPromotion = viewer.IsEnableSabaPromotion;
            IsProtectedMode = viewer.IsProtectedMode;
        }

        public string ProductIds { get; set; } = DefaultVariables.AllProducts;

        public string ResourceIds { get; set; } = DefaultVariables.AllProductDetails;

        public string BreadcrumbLevelJson { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsEnableSabaPromotion { get; }

        public bool IsProtectedMode { get; }

        private IDictionary<int, bool> userSelectedProductIds;
        public IDictionary<int, bool> UserSelectedProductIds
        {
            get
            {
                userSelectedProductIds = userSelectedProductIds ?? ProductIds.ToIntDictionary();

                return userSelectedProductIds;
            }
        }

        public bool IsSelectAllProducts  => DefaultVariables.AllProducts == ProductIds;

        private IDictionary<int, bool> userSelectedResourceIds;
        public IDictionary<int, bool> UserSelectedResourceIds
        {
            get
            {
                userSelectedResourceIds = userSelectedResourceIds ?? ResourceIds.ToIntDictionary();

                return userSelectedResourceIds;
            }
        }

        public bool IsSelectAllProductDetails => DefaultVariables.AllProductDetails == ResourceIds;
    }
}