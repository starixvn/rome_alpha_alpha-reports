using Alpha.Reports.Constants;
using Alpha.Reports.Models;
using Alpha.Reports.Site.ViewModels;
using MediatR;

namespace Alpha.Reports.Site.Requests.Statement
{
    public class SMAStatementExcelQuery :
        SMAStatementQuery,
        IRequest<ExcelFileViewModel>
    {
        public SMAStatementExcelQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }
    }
}