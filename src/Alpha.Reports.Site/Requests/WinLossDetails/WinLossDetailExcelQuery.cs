namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using System.Web;
    using Constants;
    using Enumerations;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class WinLossDetailExcelQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";

        public WinLossDetailExcelQuery() : this(null)
        {
        }

        public WinLossDetailExcelQuery(IUserSession userSession) : base(userSession)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
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