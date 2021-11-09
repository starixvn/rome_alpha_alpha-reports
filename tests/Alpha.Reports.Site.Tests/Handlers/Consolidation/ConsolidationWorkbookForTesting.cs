namespace Alpha.Reports.Site.Tests.Handlers.Consolidation
{
    using System;
    using ONEbook.SpreadSheets;
    using Site.Handlers.Consolidation;
    using Site.ViewModels.Consolidation;

    public class ConsolidationWorkbookForTesting : ConsolidationWorkbook<ConsolidationByDateViewModel>
    {
        public void CallSetWinLossByUsernameValue(ConsolidationEntityViewModel data, Row row)
        {
            SetWinLossByUsernameValue(data, row);
        }

        public void CallMergeCells(Sheet sheet, Func<ConsolidationByDateViewModel, int> getGroupLength)
        {
            MergeCells(sheet, getGroupLength);
        }

        public void InitSheet()
        {
            SpreadSheet = new SpreadSheet<ConsolidationByDateViewModel>();
            TemplateInfo = new TemplateInfo(templatePath: string.Empty);
        }
    }
}
