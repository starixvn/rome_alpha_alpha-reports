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

        public string DrilldownFromProductIds { get; set; }

        public DateTime DrilldownDate { get; set; }

        public string DrilldownFromType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

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