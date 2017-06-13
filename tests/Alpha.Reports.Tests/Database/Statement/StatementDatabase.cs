using System.Data;
using Alpha.Reports.Tests.Helpers;

namespace Alpha.Reports.Tests.Database.Statement
{
    public static class StatementDatabase
    {
        private static DataSet statementData = FileHelper.LoadDataSet("\\Database\\Statement\\StatementHasData.xml");

        public static DataSet StatementData => statementData;
    }
}
