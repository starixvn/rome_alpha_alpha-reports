using Alpha.IntegrationTest.Npoi;
using Alpha.Reports.Models.Consolidation;
using NPOI.SS.UserModel;

namespace Alpha.Reports.StaticData.DataAccesses
{
    public class ConsolidationExcelRepository : ReportExcelRepository
    {
        public static object CreateConsolidationByUsername(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByUsername(
               breakDown: row.GetCellValue<string>(columnIndex++),
               custId: row.GetCellValue<int>(columnIndex++),
               username: row.GetCellValue<string>(columnIndex++),
               currencyId: row.GetCellValue<byte>(columnIndex++),
               custLevel: row.GetCellValue<int>(columnIndex++),
               turnover: row.GetCellValue<decimal>(columnIndex++),
               memberWinLoss: row.GetCellValue<decimal>(columnIndex++),
               memberComm: row.GetCellValue<decimal>(columnIndex++),
               memberTotal: row.GetCellValue<decimal>(columnIndex++),
               ownerTurnover: row.GetCellValue<decimal>(columnIndex++),
               ownerWinLoss: row.GetCellValue<decimal>(columnIndex++),
               ownerComm: row.GetCellValue<decimal>(columnIndex++),
               ownerTotal: row.GetCellValue<decimal>(columnIndex++),
               company: row.GetCellValue<decimal>(columnIndex++),
               betCount: row.GetCellValue<int>(columnIndex));
        }

        public static object CreateConsolidationByDate(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByDate(
               breakDown: row.GetCellValue<string>(columnIndex++),
               custId: row.GetCellValue<int>(columnIndex++),
               username: row.GetCellValue<string>(columnIndex++),
               currencyId: row.GetCellValue<byte>(columnIndex++),
               custLevel: row.GetCellValue<int>(columnIndex++),
               turnover: row.GetCellValue<decimal>(columnIndex++),
               memberWinLoss: row.GetCellValue<decimal>(columnIndex++),
               memberComm: row.GetCellValue<decimal>(columnIndex++),
               memberTotal: row.GetCellValue<decimal>(columnIndex++),
               ownerTurnover: row.GetCellValue<decimal>(columnIndex++),
               ownerWinLoss: row.GetCellValue<decimal>(columnIndex++),
               ownerComm: row.GetCellValue<decimal>(columnIndex++),
               ownerTotal: row.GetCellValue<decimal>(columnIndex++),
               company: row.GetCellValue<decimal>(columnIndex++),
               betCount: row.GetCellValue<int>(columnIndex));
        }

        public static object CreateConsolidationByProduct(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByProduct(
               breakDown: row.GetCellValue<string>(columnIndex++),
               custId: row.GetCellValue<int>(columnIndex++),
               username: row.GetCellValue<string>(columnIndex++),
               currencyId: row.GetCellValue<byte>(columnIndex++),
               custLevel: row.GetCellValue<int>(columnIndex++),
               turnover: row.GetCellValue<decimal>(columnIndex++),
               memberWinLoss: row.GetCellValue<decimal>(columnIndex++),
               memberComm: row.GetCellValue<decimal>(columnIndex++),
               memberTotal: row.GetCellValue<decimal>(columnIndex++),
               ownerTurnover: row.GetCellValue<decimal>(columnIndex++),
               ownerWinLoss: row.GetCellValue<decimal>(columnIndex++),
               ownerComm: row.GetCellValue<decimal>(columnIndex++),
               ownerTotal: row.GetCellValue<decimal>(columnIndex++),
               company: row.GetCellValue<decimal>(columnIndex++),
               betCount: row.GetCellValue<int>(columnIndex));
        }

        public override T Create<T>(IRow row)
        {
            if (typeof(T) == typeof(ConsolidationByUsername))
            {
                return (T)CreateConsolidationByUsername(row);
            }

            if (typeof(T) == typeof(ConsolidationByDate))
            {
                return (T)CreateConsolidationByDate(row);
            }

            if (typeof(T) == typeof(ConsolidationByProduct))
            {
                return (T)CreateConsolidationByProduct(row);
            }

            return default(T);
        }
    }
}
