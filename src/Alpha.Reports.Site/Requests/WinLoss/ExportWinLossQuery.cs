namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using Adapters;
    using MediatR;
    using Models;
    using Models.Winloss;
    using ViewModels.WinLoss;

    public class ExportWinLossQuery : IRequest<byte[]>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/";
        private readonly WinLossViewModel viewModel;

        public ExportWinLossQuery(WinLossViewModel viewModel)
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

            TemplatePath = HttpAdapter.FuncServerMapPath(excelTemplatePath);
        }

        public string TemplatePath { get; set; }

        public UserLevel UserLevel => viewModel.UserLevel;

        public string Title => viewModel.Title;

        public IEnumerable<WinLoss> ReportData => viewModel.ReportData;

        public WinLoss GrantTotal => viewModel.GrantTotal;

        public DateTime FromDate => viewModel.FromDate;

        public DateTime ToDate => viewModel.ToDate;

        public string UserSelectedProductIds => viewModel.UserSelectedProductIds;
    }
}