namespace Alpha.Reports.Site.Requests.Consolidation
{
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class ConsolidationByUsernameExcelQuery : GetConsolidationQuery, IRequest<ExcelFileViewModel>
    {
        public ConsolidationByUsernameExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}