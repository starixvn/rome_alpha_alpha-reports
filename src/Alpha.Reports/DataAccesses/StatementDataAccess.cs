using System;
using System.Collections.Generic;
using Alpha.Reports.Criteria.Statement;
using Alpha.Reports.Models.Statement;
using Fanex.Common;

namespace Alpha.Reports.DataAccesses
{
    public interface StatementDataAccess
    {
        IEnumerable<StatementModel> GetStatements(GetStatementCriteria criteria);
    }

    public class StatementDataAccessImpl : StatementDataAccess
    {
        public IEnumerable<StatementModel> GetStatements(GetStatementCriteria criteria)
        {
            var data = new List<StatementModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data.Add(new StatementModel(
                                Converter.ToDateTime(reader["WinLostDate"]),
                                Converter.ToInt(reader["CustId"]),
                                Convert.IsDBNull(reader["OpeningBalance"]) ? (decimal?)null : Converter.ToDecimal(reader["OpeningBalance"]),
                                Convert.IsDBNull(reader["VoidAmt"]) ? (decimal?)null : Converter.ToDecimal(reader["VoidAmt"]),
                                Convert.IsDBNull(reader["Outstanding"]) ? (decimal?)null : Converter.ToDecimal(reader["Outstanding"]),
                                Convert.IsDBNull(reader["Turnover"]) ? (decimal?)null : Converter.ToDecimal(reader["Turnover"]),
                                Convert.IsDBNull(reader["TransferAmt"]) ? (decimal?)null : Converter.ToDecimal(reader["TransferAmt"]),
                                Convert.IsDBNull(reader["Winlost"]) ? (decimal?)null : Converter.ToDecimal(reader["Winlost"]),
                                Convert.IsDBNull(reader["Comm"]) ? (decimal?)null : Converter.ToDecimal(reader["Comm"]),
                                Convert.IsDBNull(reader["ClosingBalance"]) ? (decimal?)null : Converter.ToDecimal(reader["ClosingBalance"]),
                                Convert.IsDBNull(reader["GrandPrize"]) ? (decimal?)null : Converter.ToDecimal(reader["GrandPrize"])));
                        }
                    }
                }
            }

            return data;
        }
    }
}
