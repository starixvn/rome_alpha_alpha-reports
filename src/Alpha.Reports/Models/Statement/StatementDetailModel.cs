namespace Alpha.Reports.Models.Statement
{
    using System;
    using Alpha.Reports.Extensions;

    public class StatementDetailModel
    {
        public StatementDetailModel()
        {
        }

        public StatementDetailModel(
            DateTime transDate,
            string transType,
            string transDesc,
            decimal? amount,
            string creator) : this(transDate, transType, transDesc, amount, string.Empty, creator)
        {
        }

        public StatementDetailModel(
            DateTime transDate,
            string transType,
            string transDesc,
            decimal? amount,
            string exchangeRate,
            string creator)
        {
            TransDate = transDate;
            TransType = transType;
            TransDesc = transDesc;
            Amount = amount;
            ExchangeRate = exchangeRate;
            Creator = creator;
        }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public string TransDesc { get; set; }

        public decimal? Amount { get; set; }

        public string ExchangeRate { get; set; }

        public string Creator { get; set; }

        public string FullTransDateName => TransDate.ToFullDateName();

        public string TransDateString => TransDate.ToShortDateWithSlash();

        public string TransTimeString => TransDate.ToLongTime();

        public static StatementDetailModel CreateDefault() =>
             new StatementDetailModel(DateTime.MinValue, string.Empty, string.Empty, null, string.Empty, string.Empty);

        public void Add(StatementDetailModel statement)
        {
            Amount = (Amount ?? 0) + statement.Amount;
        }
    }
}
