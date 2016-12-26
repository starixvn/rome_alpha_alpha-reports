namespace Alpha.Reports.Site.Requests.WinLoss
{
    using Enumerations;
    using Helpers;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using WinLoss;

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

                return HttpAdapter.ServerMapPathFunc(excelTemplatePath);
            }
        }

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(UserLevelId);
    }
}