namespace Alpha.Reports.Site.Handlers.CommissionByBettype
{
    using System;
    using System.Collections.Generic;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Reports.Models.CommissionByBettype;
    using Services;
    using Sunplus.Agent.Model.Definitions;

    public interface CommissionByBettypeWorkbook
    {
        Workbook<CommissionByBettypeModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<CommissionByBettypeModel> Title(string titleText);

        Workbook<CommissionByBettypeModel> Header(Func<string, string> headerMapping);

        Workbook<CommissionByBettypeModel> Body(IEnumerable<CommissionByBettypeModel> rows, Mapper<CommissionByBettypeModel> mapper);

        Workbook<CommissionByBettypeModel> Footer(CommissionByBettypeModel row, Mapper<CommissionByBettypeModel> rowMapper);

        Workbook<CommissionByBettypeModel> Sheet(string sheetName);

        byte[] ExportToBytes();
    }

    public class CommissionByBettypeWorkbookImpl : NpoiWorkbook<CommissionByBettypeModel>, CommissionByBettypeWorkbook
    {
        private readonly UserPermissionService userPermissionService;

        public CommissionByBettypeWorkbookImpl(UserPermissionService userPermissionServiceImpl)
        {
            userPermissionService = userPermissionServiceImpl;
        }

        protected override void FillBody(Sheet sheet)
        {
            base.FillBody(sheet);

            var npoiSheet = Workbook.GetSheetAt(0);
            var headerAddress = npoiSheet.GetMergedRegion(0);

            npoiSheet.RemoveMergedRegion(0);

            if (userPermissionService.IsIMbcCustomer() == false)
            {
                RemoveColumnIndex(npoiSheet, 7);
                headerAddress.LastColumn--;
            }
            if (userPermissionService.IsShowMyanmarOdds() == false)
            {
                RemoveColumnIndex(npoiSheet, 5);
                headerAddress.LastColumn--;
            }
            if (userPermissionService.IsEnableProduct(ProductIdConstants.CockFighting) == false)
            {
                RemoveColumnIndex(npoiSheet, 4);
                headerAddress.LastColumn--;
            }

            npoiSheet.AddMergedRegion(headerAddress);
        }

        private void RemoveColumnIndex(ISheet sheet, int colIndex)
        {
            var rowIter = sheet.GetEnumerator();
            while (rowIter.MoveNext())
            {
                var row = (HSSFRow)rowIter.Current;
                var cell = row.GetCell(colIndex);

                row.RemoveCell(cell);

                for (int i = cell.ColumnIndex + 1; i <= row.Cells.Count; i++)
                {
                    row.MoveCell(row.GetCell(i), i - 1);
                }
            }
        }
    }
}