namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MediatR;
    using Newtonsoft.Json;
    using Reports.AppResources;
    using Reports.Helpers;
    using Requests.MemberWinLossDetail;
    using ViewModels.MemberWinLossDetail;

    public class MemberWinLossDetailHandler :
        IRequestHandler<IndexQuery, IndexViewModel>
    {
        private ProductHandler productHandler;

        public MemberWinLossDetailHandler(ProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var visibleProducts = productHandler.VisibleProducts(message.Products);

            var netTurnoverTooltips = new List<string>();
            if (visibleProducts.Where(product => product.Id == 21) != null)
            {
                netTurnoverTooltips.Add(Resources.Tooltip_NetTurnover_21);
            }

            return new IndexViewModel
            {
                MinDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now.AddMonths(-2)),
                MaxDate = null,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,

                CurrentDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now),
                Products = JsonConvert.SerializeObject(visibleProducts),

                HasData = true,
                Language = message.Language,

                NetTurnoverToolTips = netTurnoverTooltips
            };
        }
    }
}