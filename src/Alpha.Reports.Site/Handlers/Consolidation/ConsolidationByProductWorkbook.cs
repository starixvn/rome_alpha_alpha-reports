namespace Alpha.Reports.Site.Handlers.Consolidation
{
    using System.Linq;
    using ONEbook.SpreadSheets;
    using ViewModels.Consolidation;

    public interface ConsolidationByProductWorkbook
    {
        Workbook<ConsolidationByProductViewModel> UseTemplate(TemplateInfo templateInfo);
    }

    public class ConsolidationByProductWorkbookImpl :
        ConsolidationWorkbook<ConsolidationByProductViewModel>,
        ConsolidationByProductWorkbook
    {
        protected override void FillBodyRows(Sheet sheet)
        {
            var rowIndex = TemplateInfo.BodyStartIndex;

            foreach (var date in SpreadSheet.BodyRows)
            {
                foreach (var data in date.WinLossByUsername)
                {
                    var row = sheet.GetRow(rowIndex++);
                    row.CopyCellsFrom(sheet.OddRowTemplate);

                    row.SetCellValue(0, date.ProductName);
                    SetWinLossByUsernameValue(data, row);
                }
            }

            MergeCells(sheet, (date) => date.WinLossByUsername.Count());
        }
    }
}