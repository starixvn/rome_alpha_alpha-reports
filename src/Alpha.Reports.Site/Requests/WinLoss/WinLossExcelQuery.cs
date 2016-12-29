namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web;
    using Enumerations;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/";

        public WinLossExcelQuery() : this(null)
        {
        }

        public WinLossExcelQuery(IUserSession userSession) : base(userSession)
        {
        }

        public string TemplatePath => HttpContext.Current != null
           ? HttpContext.Current.Server.MapPath(BuildExcelTemplatePath())
           : string.Empty;

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(UserLevelId);

        internal string BuildExcelTemplatePath()
        {
            return ExcelTemplateDir +
                (UserLevel.IsSuperLevel()
                ? "Master.xls"
                : UserLevel.IsMasterLevel()
                ? "Agent.xls"
                : "Member.xls");
        }
    }
}