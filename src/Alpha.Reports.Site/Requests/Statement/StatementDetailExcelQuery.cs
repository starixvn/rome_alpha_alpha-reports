using Alpha.Reports.Models;
using Alpha.Reports.Site.ViewModels;
using MediatR;

namespace Alpha.Reports.Site.Requests.Statement
{
    public class StatementDetailExcelQuery : StatementDetailsQuery, IRequest<ExcelFileViewModel>
    {
        public StatementDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}