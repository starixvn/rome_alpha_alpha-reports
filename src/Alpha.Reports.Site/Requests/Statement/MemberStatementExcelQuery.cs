namespace Alpha.Reports.Site.Requests.Statement
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels;
    using MediatR;

    public class MemberStatementExcelQuery :
        MemberStatementQuery,
        IRequest<ExcelFileViewModel>
    {
        public MemberStatementExcelQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
        }
    }
}