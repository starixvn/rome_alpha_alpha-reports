namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Adapters;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class ExportEnteredResultQuery : EnteredResultQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";

        public ExportEnteredResultQuery() : this(null)
        {
        }

        public ExportEnteredResultQuery(IUserSession userSession) : base(userSession)
        {
        }

        public string TemplatePath
        {
            get
            {
                return HttpAdapter.FuncServerMapPath(ExcelTemplateDir);
            }
        }
    }
}