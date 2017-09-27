namespace Alpha.Reports.Site.Requests.Statement
{
    using System;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.ViewModels.Statement;
    using Alpha.Reports.Utils;
    using MediatR;

    public class StatementDetailsQuery :
        ReportBaseQuery,
        IRequest<StatementDetailViewModel>
    {
        public StatementDetailsQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime TransferDate { get; set; }

        public DateTime MinDate => SystemTime.Today.AddYears(-3).FirstDayOfYear();

        public override bool IsValid() => base.IsValid() && TransferDate >= MinDate;
    }
}