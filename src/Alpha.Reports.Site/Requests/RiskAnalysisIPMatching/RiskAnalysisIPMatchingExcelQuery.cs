namespace Alpha.Reports.Site.Requests.RiskAnalysisIPMatching
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class RiskAnalysisIPMatchingExcelQuery : RiskAnalysisIPMatchingQuery, IRequest<ExcelFileViewModel>
    {
        public RiskAnalysisIPMatchingExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}