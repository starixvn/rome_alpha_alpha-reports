using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Reports.Constants;
using Alpha.Reports.Extensions;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Requests;
using Alpha.Reports.Site.ViewModels.Components;

namespace Alpha.Reports.Site.ViewModels.Statement
{
    public class StatementViewModel
    {
        protected readonly ReportBaseQuery Message;

        public StatementViewModel(
            ReportBaseQuery message,
            IEnumerable<StatementModel> statements,
            DateTime minDate,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption,
            bool isFilterVisible,
            int pageSize,
            int pageIndex)
        {
            this.Message = message;
            Statements = statements;

            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                minDate.ToShortDateWithSlash());

            MinDate = minDate;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedDateRangeOption = userSelectedDateRangeOption;
            IsFilterVisible = isFilterVisible;

            PageSize = pageSize == 0 ? 50 : pageSize;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
        }

        public static DateTime MaxDate => DateTime.MaxValue;

        public int CustId => Message.CustId;

        public string CustName => Message.CustName;

        public DateTime MinDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int UserSelectedDateRangeOption { get; }

        public bool IsFilterVisible { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public string ViewDataInstructionMessage { get; }

        public IEnumerable<StatementModel> Statements { get; }

        public UserLevel CustLevel => Message.CustLevel;

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(PageIndex, PageSize, Statements.Count());
    }
}