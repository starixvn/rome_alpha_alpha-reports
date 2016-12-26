namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Adapters;
    using Enumerations;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class ExportWinLossQuery : WinLossQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/";

        public ExportWinLossQuery() : this(null)
        {
        }

        public ExportWinLossQuery(IUserSession userSession) : base(userSession)
        {
        }

        public string TemplatePath
        {
            get
            {
                var excelTemplatePath = ExcelTemplateDir +
                    (UserLevel.IsSuperLevel()
                    ? "Master.xls"
                    : UserLevel.IsMasterLevel()
                    ? "Agent.xls"
                    : "Member.xls");

                return HttpAdapter.FuncServerMapPath(excelTemplatePath);
            }
        }

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(UserLevelId);
    }
}