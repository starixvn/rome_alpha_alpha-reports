namespace Alpha.Reports.StaticData.Repositories
{
    using Alpha.Reports.Models.Consolidation;
    using Alpha.StaticData.Npoi;
    using NPOI.SS.UserModel;

    public class ConsolidationExcelRepository : ReportExcelRepository
    {
        public static object CreateConsolidationByUsername(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByUsername(
               breakDown: row.Get<int>(columnIndex++),
               custId: row.Get<int>(columnIndex++),
               username: row.Get<string>(columnIndex++),
               currencyId: row.Get<byte>(columnIndex++),
               custLevel: row.Get<int>(columnIndex++),
               turnover: row.Get<decimal>(columnIndex++),
               memberWinLoss: row.Get<decimal>(columnIndex++),
               memberComm: row.Get<decimal>(columnIndex++),
               memberTotal: row.Get<decimal>(columnIndex++),
               ownerTurnover: row.Get<decimal>(columnIndex++),
               ownerWinLoss: row.Get<decimal>(columnIndex++),
               ownerComm: row.Get<decimal>(columnIndex++),
               ownerTotal: row.Get<decimal>(columnIndex++),
               company: row.Get<decimal>(columnIndex++),
               betCount: row.Get<int>(columnIndex));
        }

        public static object CreateConsolidationByDate(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByDate(
               breakDown: row.Get<string>(columnIndex++),
               custId: row.Get<int>(columnIndex++),
               username: row.Get<string>(columnIndex++),
               currencyId: row.Get<byte>(columnIndex++),
               custLevel: row.Get<int>(columnIndex++),
               turnover: row.Get<decimal>(columnIndex++),
               memberWinLoss: row.Get<decimal>(columnIndex++),
               memberComm: row.Get<decimal>(columnIndex++),
               memberTotal: row.Get<decimal>(columnIndex++),
               ownerTurnover: row.Get<decimal>(columnIndex++),
               ownerWinLoss: row.Get<decimal>(columnIndex++),
               ownerComm: row.Get<decimal>(columnIndex++),
               ownerTotal: row.Get<decimal>(columnIndex++),
               company: row.Get<decimal>(columnIndex++),
               betCount: row.Get<int>(columnIndex));
        }

        public static object CreateConsolidationByProduct(IRow row)
        {
            var columnIndex = 0;

            return new ConsolidationByProduct(
               breakDown: (byte)row.Get<int>(columnIndex++),
               custId: row.Get<int>(columnIndex++),
               username: row.Get<string>(columnIndex++),
               currencyId: row.Get<byte>(columnIndex++),
               custLevel: row.Get<int>(columnIndex++),
               turnover: row.Get<decimal>(columnIndex++),
               memberWinLoss: row.Get<decimal>(columnIndex++),
               memberComm: row.Get<decimal>(columnIndex++),
               memberTotal: row.Get<decimal>(columnIndex++),
               ownerTurnover: row.Get<decimal>(columnIndex++),
               ownerWinLoss: row.Get<decimal>(columnIndex++),
               ownerComm: row.Get<decimal>(columnIndex++),
               ownerTotal: row.Get<decimal>(columnIndex++),
               company: row.Get<decimal>(columnIndex++),
               betCount: row.Get<int>(columnIndex));
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
