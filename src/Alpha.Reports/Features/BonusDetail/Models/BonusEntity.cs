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

        public DateTime CreatedDate { get; }

        public long TransId { get; }

        public string MasterName { get; }

        public string AgentName { get; }

        public int CustId { get; }

        public string Username { get; }

        public decimal Amount { get; }

        public DateTime? PaymentDate { get; }

        public byte StatusId { get; }

        public bool IsComplained { get; }

        public string CreatedDateString => CreatedDate.ToLongDateWithoutSecondWithSlash();

        public string PaymentDateString => PaymentDate.ToShortDateWithSlash();

        public string MinConfirmPaymentDate => CreatedDate.ToShortDateWithSlash();

        public bool IsCancelled => StatusId == BonusStatusIdConst.Cancelled;

        public bool CanConfirmPaymentDate => StatusId == BonusStatusIdConst.Pending;
    }
}
