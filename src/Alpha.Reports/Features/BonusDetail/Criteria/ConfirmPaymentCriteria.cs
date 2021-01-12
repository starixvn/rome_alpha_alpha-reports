namespace Alpha.Reports.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Fanex.Data.Repository;

    internal class ConfirmPaymentCriteria : NonQueryCommand
    {
        public ConfirmPaymentCriteria(ConfirmPaymentParam param)
        {
            CustId = param.CustId;
            SubAccId = param.SubAccId;
            TransId = param.TransId;
            PaymentDate = param.PaymentDate;
            UserId = param.ViewerId;
            LoginIP = param.LoginIP;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public long TransId { get; }

        public DateTime PaymentDate { get; }

        public int UserId { get; }

        public string LoginIP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_Bonus_ConfirmPayment";

        public override bool IsValid()
            => CustId > 0 && TransId > 0 && UserId > 0;
    }
}
