namespace Alpha.Reports.Site.Requests.WinLoss
{
    using System.Web;
    using MediatR;
    using Reports.Models;
    using ViewModels;

    public class EnteredSportResultExcelQuery : EnteredSportResultQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";

        public EnteredSportResultExcelQuery(IUser user) : base(user)
        {
        }

        public string TemplatePath => HttpContext.Current != null ? HttpContext.Current.Server.MapPath(ExcelTemplateDir) : string.Empty;
    }
}