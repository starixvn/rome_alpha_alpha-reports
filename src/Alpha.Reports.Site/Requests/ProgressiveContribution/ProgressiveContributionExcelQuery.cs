namespace Alpha.Reports.Site.Requests.ProgressiveContribution
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class ProgressiveContributionExcelQuery : ProgressiveContributionQuery, IRequest<ExcelFileViewModel>
    {
        public ProgressiveContributionExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}