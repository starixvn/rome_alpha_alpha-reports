namespace Alpha.Reports.Site.Requests.Consolidation
{
    using System;
    using MediatR;
    using Reports.Models;
    using ViewModels.Consolidation;

    public class ConsolidationDetailQuery : GetConsolidationQuery, IRequest<ConsolidationDetailViewModel>
    {
        public ConsolidationDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public string DrilldownFromType { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public DateTime ConsolidationFromDate { get; set; }

        public DateTime ConsolidationToDate { get; set; }

        public int ConsolidationUserSelectedDateRangeOption { get; set; }

        public string ConsolidationProductIds { get; set; }

        public string ConsolidationResourceIds { get; set; }

        public static bool CanViewReport(ReportDefaultDateRangeQuery query)
        {
            //// TODO: Side effect here
            query.AssignFromDateValue();

            return query.CustId > 0
                && !string.IsNullOrWhiteSpace(query.CustName)
                && !string.IsNullOrWhiteSpace(query.Language)
                && query.CheckDateRangeConstraint();
        }

        public override bool IsValid() => CanViewReport(this);
    }
}