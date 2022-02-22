namespace Alpha.Reports.Features.BetCreditTracking.StatementMovement.Models
{
    using System;

    public class StatementMovementModel
    {
        public string MemberRefNo { get; set; }

        public DateTime OldEventDate { get; set; }

        public DateTime NewEventDate { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberComm { get; set; }

        public decimal MemberTotal { get; set; }
    }
}
