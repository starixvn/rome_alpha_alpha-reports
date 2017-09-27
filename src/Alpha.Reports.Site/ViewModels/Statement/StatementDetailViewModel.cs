namespace Alpha.Reports.Site.ViewModels.Statement
{
    using System;
    using System.Collections.Generic;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Site.Requests.Statement;

    public class StatementDetailViewModel
    {
        private readonly StatementDetailsQuery message;

        public StatementDetailViewModel(
            StatementDetailsQuery message,
            IEnumerable<StatementDetailModel> statements)
        {
            this.message = message;
            Statements = statements;

            Total = AccummulateTotal(statements);
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime TransferDate => message.TransferDate;

        public IEnumerable<StatementDetailModel> Statements { get; }

        public StatementDetailModel Total { get; private set; }

        public static StatementDetailModel AccummulateTotal(IEnumerable<StatementDetailModel> statements)
        {
            var total = StatementDetailModel.CreateDefault();

            foreach (var statement in statements)
            {
                total.Add(statement);
            }

            return total;
        }
    }
}