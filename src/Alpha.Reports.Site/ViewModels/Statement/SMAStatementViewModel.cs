using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Reports.Constants;
using Alpha.Reports.Extensions;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.ViewModels.Components;

namespace Alpha.Reports.Site.ViewModels.Statement
{
    public class SMAStatementViewModel
    {
        private readonly SMAStatementQuery message;

        public SMAStatementViewModel(
            SMAStatementQuery message,
            IEnumerable<StatementModel> statements)
        {
            this.message = message;
            Statements = statements;

            PageSize = message.PageSize == 0 ? 50 : message.PageSize;
            PageIndex = message.PageIndex == 0 ? DefaultVariables.PageIndex : message.PageIndex;

            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                message.MinDate.ToShortDateWithSlash());
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => message.FromDate;

        public DateTime ToDate => message.ToDate;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsFilterVisible => message.IsFilterVisible;

        public int PageSize { get; }

        public int PageIndex { get; }

        public string ViewDataInstructionMessage { get; }

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<StatementModel> Statements { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(PageIndex, PageSize, Statements.Count());
    }
}