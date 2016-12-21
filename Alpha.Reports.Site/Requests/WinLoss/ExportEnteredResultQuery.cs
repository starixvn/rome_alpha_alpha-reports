namespace Alpha.Reports.Site.Requests.WinLossDetails
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using MediatR;
    using Models.Winloss;
    using ViewModels.WinLoss;

    public class ExportEnteredResultQuery : IRequest<byte[]>
    {
        private const string ExcelTemplatePath = "~/App_Data/ExcelTemplates/WinLoss/MatchResultEntered.xls";
        private EnteredResultViewModel viewModel;

        public ExportEnteredResultQuery(EnteredResultViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            this.viewModel = viewModel;

            TemplatePath = HttpContext.Current.Server.MapPath(ExcelTemplatePath);
        }

        public string TemplatePath { get; set; }

        public string Title => viewModel.Title;

        public IEnumerable<EnteredSportResult> ReportData => viewModel.ReportData;
    }
}