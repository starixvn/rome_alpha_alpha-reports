namespace Alpha.Reports.Models.WinlossDetail
{
    public interface IWinLossDetail
    {
        decimal AgentComm { get; set; }

        decimal AgentTotal { get; }

        decimal AgentWinLoss { get; set; }

        decimal Company { get; }

        int CustId { get; set; }

        string CustName { get; set; }

        decimal GrossComm { get; set; }

        decimal MemberComm { get; set; }

        decimal MemberTotal { get; }

        decimal MemberWinLoss { get; set; }

        decimal NetTurnover { get; set; }

        int NumberOfTickets { get; set; }

        decimal Turnover { get; set; }
    }
}