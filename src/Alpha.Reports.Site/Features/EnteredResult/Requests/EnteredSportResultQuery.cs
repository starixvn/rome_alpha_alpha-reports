namespace Alpha.Reports.Site.Requests.WinLoss
{
    using MediatR;
    using Reports.Models;
    using ViewModels.WinLoss;

    public class EnteredSportResultQuery : ReportBaseQuery, IRequest<EnteredResultViewModel>
    {
        public EnteredSportResultQuery(IUser viewer) : base(viewer)
        {
            IsSupportHistoricReport = true;
        }
    }
}