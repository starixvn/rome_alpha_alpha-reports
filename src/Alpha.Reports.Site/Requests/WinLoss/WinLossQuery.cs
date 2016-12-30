namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLoss;

    public class WinLossQuery :
         ReportBaseQuery,
         IRequest<WinLossViewModel>
    {
        public WinLossQuery(IUser user)
        {
            CustId = user.CustId;
            CustName = user.CustName;
            UserLevelId = user.UserLevelId;
            Language = user.Language;
        }

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}