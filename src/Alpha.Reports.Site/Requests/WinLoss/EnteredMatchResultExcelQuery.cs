namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class EnteredMatchResultExcelQuery : EnteredMatchResultQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";

        public EnteredMatchResultExcelQuery() : this(null)
        {
        }

        public EnteredMatchResultExcelQuery(IUserSession userSession) : base(userSession)
        {
        }

        public string TemplatePath
        {
            get
            {
                return HttpContext.Current != null ? HttpContext.Current.Server.MapPath(ExcelTemplateDir) : string.Empty;
            }
        }
    }
}