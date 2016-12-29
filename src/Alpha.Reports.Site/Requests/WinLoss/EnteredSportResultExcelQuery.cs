namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class EnteredSportResultExcelQuery : EnteredSportResultQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";

        public EnteredSportResultExcelQuery() : this(null)
        {
        }

        public EnteredSportResultExcelQuery(IUserSession userSession) : base(userSession)
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