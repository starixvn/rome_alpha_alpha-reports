namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web.Mvc;
    using MediatR;
    using Reports.Models;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.WinLoss;

    public class WinLossQuery :
         BaseReportQuery,
         IRequest<WinLossViewModel>
    {
        private readonly User user;

        public WinLossQuery() : this(null)
        {
        }

        public WinLossQuery(User user)
        {
            this.user = user ?? DependencyResolver.Current.GetService<User>();

            CustId = this.user.CustId;
            CustName = this.user.CustName;
            UserLevelId = this.user.UserLevelId;
            Language = this.user.Language;
        }

        public string Language { get; }

        public string UserSelectedProductIds { get; set; }

        public bool IsStackMode { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsFilterVisible { get; set; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}