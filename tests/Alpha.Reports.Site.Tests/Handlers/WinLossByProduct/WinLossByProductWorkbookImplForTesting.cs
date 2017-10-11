namespace Alpha.Reports.Site.Tests.Handlers.WinLossByProduct
{
    using ONEbook.SpreadSheets;
    using Site.Handlers.WinLossByProduct;

    public class WinLossByProductWorkbookImplForTesting : WinLossByProductWorkbookImpl
    {
        public void CallFillBodyRows(Sheet sheet)
        {
            FillBodyRows(sheet);
        }

        public void CallFillFooterRows(Sheet sheet)
        {
            FillFooterRows(sheet);
        }
    }
}
