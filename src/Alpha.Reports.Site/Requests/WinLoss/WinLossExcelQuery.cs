namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/";

        public WinLossExcelQuery() : this(null)
        {
        }

        public WinLossExcelQuery(User user) : base(user)
        {
        }

        public string TemplatePath => HttpContext.Current != null
           ? HttpContext.Current.Server.MapPath(BuildExcelTemplatePath())
           : string.Empty;

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(UserLevelId);

        internal string BuildExcelTemplatePath() =>
            ExcelTemplateDir +
            (UserLevel.IsSuperLevel()
            ? "Master.xls"
            : UserLevel.IsMasterLevel()
            ? "Agent.xls"
            : "Member.xls");
    }
}