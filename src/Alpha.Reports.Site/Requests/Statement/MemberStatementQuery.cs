using Alpha.Reports.Models;
using Alpha.Reports.Site.Constants;
using Alpha.Reports.Site.ViewModels.Statement;
using Alpha.Reports.Utils;
using MediatR;

namespace Alpha.Reports.Site.Requests.Statement
{
    public class MemberStatementQuery :
        FromLast3MonthDateRangeQuery,
        IRequest<MemberStatementViewModel>
    {
        public MemberStatementQuery(IUser viewer) : base(viewer)
        {
            FromDate = SystemTime.Today.AddDays(-1);
        }

        public string ProductIds { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; } = UserSelectedDateRangeOptionConstant.SpecificDates;
    }
}