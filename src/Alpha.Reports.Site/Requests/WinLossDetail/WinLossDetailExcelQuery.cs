namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Web;
    using Constants;
    using Enumerations;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class WinLossDetailExcelQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";

        public WinLossDetailExcelQuery() : this(null)
        {
        }

        public WinLossDetailExcelQuery(User user) : base(user)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
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