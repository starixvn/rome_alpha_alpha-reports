namespace Alpha.Reports.Site.Requests.WinLossAnalysis
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossAnalysisExcelQuery : WinLossAnalysisQuery, IRequest<ExcelFileViewModel>
    {
        public WinLossAnalysisExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}