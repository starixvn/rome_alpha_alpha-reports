namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Fanex.Common;
    using Fanex.Data;
    using System;
    using System.Data;

    public interface BetCreditHistoryDataAccess
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryCriteria criteria);
    }

    public class BetCreditHistoryDataAcessImpl : BetCreditHistoryDataAccess
    {
        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryCriteria criteria)
        {
            var result = new BetCreditHistoryResultModel();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(BuildBetCreditHistoryModel(reader));
                        }

                        reader.Close();

                        var outputParams = db.GetOuputParams();
                        if (outputParams != null)
                        {
                            result.ErrorCode = Converter.ToInt(outputParams["errorcode"]);
                            result.ErrorMess = outputParams["errormess"].ToString();
                        }
                    }
                }
            }

            return result;
        }

        public BetCreditHistoryModel BuildBetCreditHistoryModel(IDataReader reader)
        {
            return new BetCreditHistoryModel
            {
                ActionDate = Converter.ToDateTime(reader["ActionDate"]),
                WinLostDate = Convert.IsDBNull(reader["winlostdate"]) ? (DateTime?)null : Converter.ToDateTime(reader["winlostdate"]),
                RefNo = Convert.IsDBNull(reader["refNo"]) ? (long?)null : Convert.ToInt64(reader["refNo"]),
                ActualStake = Convert.IsDBNull(reader["actual_stake"]) ? (decimal?)null : Converter.ToDecimal(reader["actual_stake"]),
                WinLoss = Convert.IsDBNull(reader["winloss"]) ? (decimal?)null : Converter.ToDecimal(reader["winloss"]),
                PlayerComm = Convert.IsDBNull(reader["playercomm"]) ? (decimal?)null : Converter.ToDecimal(reader["playercomm"]),
                BalanceChanged = Convert.IsDBNull(reader["BalanceChanged"]) ? (decimal?)null : Converter.ToDecimal(reader["BalanceChanged"]),
                NewBalance = Convert.IsDBNull(reader["NewBalance"]) ? (decimal?)null : Converter.ToDecimal(reader["NewBalance"]),
                GivenCredit = Converter.ToDecimal(reader["GivenCredit"]),
                BetCredit = Convert.IsDBNull(reader["BetCredit"]) ? (decimal?)null : Converter.ToDecimal(reader["BetCredit"]),
                Action = reader["Action"].ToString()
            };
        }
    }
}
