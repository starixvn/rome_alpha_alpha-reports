namespace Alpha.Reports.Site.Features.BetCreditTracking.Requests
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;
    using System.Collections.Generic;

    public class BetCreditHistoryExcelQuery : BetCreditHistoryQuery, IRequest<ExcelFileViewModel>
    {
        public BetCreditHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public IEnumerable<BetCreditHistoryModel> Data { get; set; }
    }
}