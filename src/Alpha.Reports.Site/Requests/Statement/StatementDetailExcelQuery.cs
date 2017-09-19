namespace Alpha.Reports.Site.Requests.Statement
{
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class StatementDetailExcelQuery : StatementDetailsQuery, IRequest<ExcelFileViewModel>
    {
        public StatementDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}