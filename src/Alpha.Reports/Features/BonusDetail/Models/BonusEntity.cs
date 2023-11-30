namespace Alpha.Reports.Features.BonusDetail.Models
{
    using System;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BonusDetail.Constants;

    public class BonusEntity
    {
        public BonusEntity()
        {
        }

        public BonusEntity(
            DateTime createdDate,
            long transId,
            string masterName,
            string agentName,
            int custId,
            string username,
            decimal amount,
            DateTime? paymentDate,
            byte statusId,
            bool isComplained)
        {
            CreatedDate = createdDate;
            TransId = transId;
            MasterName = masterName;
            AgentName = agentName;
            CustId = custId;
            Username = username;
            Amount = amount;
            PaymentDate = paymentDate;
            StatusId = statusId;
            IsComplained = isComplained;
        }

        public DateTime CreatedDate { get; set; }

        public long TransId { get; set; }

        public string MasterName { get; set; }

        public string AgentName { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public byte StatusId { get; set; }

        public bool IsComplained { get; set; }

        public string CreatedDateString => CreatedDate.ToLongDateWithoutSecondWithSlash();

        public string PaymentDateString => PaymentDate.ToShortDateWithSlash();

        public string MinConfirmPaymentDate => CreatedDate.ToShortDateWithSlash();

        public bool IsCancelled => StatusId == BonusStatusIdConst.Cancelled;

        public bool CanConfirmPaymentDate => StatusId == BonusStatusIdConst.Pending;
    }
}
