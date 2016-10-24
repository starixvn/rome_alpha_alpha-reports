namespace Alpha.Reports.Site.Handlers
{
    using System;
    using MediatR;
    using Newtonsoft.Json;
    using Reports.Helpers;
    using Requests.MemberWinLossDetail;
    using ViewModels.MemberWinLossDetail;

    public class MemberWinLossDetailHandler :
        IRequestHandler<IndexQuery, IndexViewModel>
    {
        public IndexViewModel Handle(IndexQuery message)
        {
            return new IndexViewModel
            {
                MinDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now.AddMonths(-2)),
                MaxDate = null,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,

                CurrentDate = DateTimeHelper.ToShortDateWithSlash(DateTime.Now),
                Products = JsonConvert.SerializeObject(message.Products),

                HasData = true,
                Language = message.Language
            };
        }
    }
}