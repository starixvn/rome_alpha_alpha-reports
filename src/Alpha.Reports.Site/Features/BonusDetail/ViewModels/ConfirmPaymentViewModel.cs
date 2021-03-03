namespace Alpha.Reports.Site.Features.BonusDetail.ViewModels
{
    using System;
    using Alpha.Reports.Site.Features.BonusDetail.Requests;

    public class ConfirmPaymentViewModel
    {
        public ConfirmPaymentViewModel(ConfirmPaymentQuery message)
        {
            CustId = message.CustId;
            CustName = message.CustName;
            TransId = message.TransId;
            Amount = message.Amount;
            PaymentDate = message.PaymentDate;
        }

        public int CustId { get; }

        public string CustName { get; }

        public long TransId { get; }

        public decimal Amount { get; }

        public DateTime PaymentDate { get; }
    }
}