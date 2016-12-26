namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using Adapters;
    using MediatR;
    using Models.Winloss;
    using ViewModels.WinLoss;

    public class ExportEnteredResultQuery : IRequest<byte[]>
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";
        private readonly EnteredResultViewModel viewModel;

        public ExportEnteredResultQuery(EnteredResultViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.viewModel = viewModel;

            TemplatePath = HttpAdapter.FuncServerMapPath(ExcelTemplateDir);
        }

        public string TemplatePath { get; set; }

        public string Title => viewModel.Title;

        public IEnumerable<EnteredSportResult> ReportData => viewModel.ReportData;
    }
}