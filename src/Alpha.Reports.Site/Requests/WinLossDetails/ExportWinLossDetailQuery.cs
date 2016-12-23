namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using Constants;
    using Enumerations;
    using Helpers;
    using MediatR;
    using Models;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;

    public class ExportWinLossDetailQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";

        public ExportWinLossDetailQuery() : this(null)
        {
        }

        public ExportWinLossDetailQuery(IUserSession userSession) : base(userSession)
        {
            PageIndex = DefaultVariables.PageIndex;
            PageSize = DefaultVariables.PageSize;
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