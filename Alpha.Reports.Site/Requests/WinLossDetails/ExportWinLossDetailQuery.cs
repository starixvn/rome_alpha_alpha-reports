namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using System;
    using System.Web;
    using MediatR;
    using Models;
    using Models.WinlossDetails;
    using ViewModels.WinLossDetails;

    public class ExportWinLossDetailQuery : IRequest<byte[]>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLossDetail/";
        private WinLossDetailViewModel viewModel;

        public ExportWinLossDetailQuery(WinLossDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.viewModel = viewModel;

            var excelTemplatePath = ExcelTemplateDir +
                (viewModel.UserLevel.IsSuperLevel()
                ? "Master.xls"
                : viewModel.UserLevel.IsMasterLevel()
                ? "Agent.xls"
                : "Member.xls");

            TemplatePath = HttpContext.Current.Server.MapPath(excelTemplatePath);
        }

        public string TemplatePath { get; set; }

        public UserLevel UserLevel => viewModel.UserLevel;

        public string Title => viewModel.Title;

        public ReportData<WinLossDetail> ReportData => viewModel.ReportData;

        public DateTime FromDate => viewModel.FromDate;

        public DateTime ToDate => viewModel.ToDate;

        public string UserSelectedProductIds => viewModel.UserSelectedProductIds;
    }
}