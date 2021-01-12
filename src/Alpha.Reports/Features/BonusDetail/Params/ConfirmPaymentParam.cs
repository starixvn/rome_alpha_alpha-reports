namespace Alpha.Reports.Features.BonusDetail.Params
{
    using System;

    public class ConfirmPaymentParam
    {
        public ConfirmPaymentParam(
            int viewerId,
            int subAccId,
            int custId,
            long transId,
            DateTime paymentDate,
            string loginIP)
        {
            ViewerId = viewerId;
            SubAccId = subAccId;
            CustId = custId;
            TransId = transId;
            PaymentDate = paymentDate;
            LoginIP = loginIP;
        }

        public int ViewerId { get; }

        public int SubAccId { get; }

        public int CustId { get; }

        public long TransId { get; }

        public DateTime PaymentDate { get; }

        public string LoginIP { get; }
    }
}
