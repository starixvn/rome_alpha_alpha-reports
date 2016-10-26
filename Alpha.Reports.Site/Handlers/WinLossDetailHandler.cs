namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using MediatR;
    using Newtonsoft.Json;
    using Reports.AppResources;
    using Reports.Helpers;
    using Requests.WinLossDetail;
    using ViewModels.WinLossDetail;

    public class WinLossDetailHandler :
        IRequestHandler<MemberWinLossDetailQuery, IndexViewModel>
    {
        private ProductHandler productHandler;

        public WinLossDetailHandler(ProductHandler productHandler)
        {
            this.productHandler = productHandler;
        }

        public IndexViewModel Handle(MemberWinLossDetailQuery message)
        {
            var visibleProducts = productHandler.TransformProducts(message.Products);

            var netTurnoverTooltips = new List<string>();
            if (visibleProducts.Where(product => product.Id == ProductId.AllBet) != null)
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