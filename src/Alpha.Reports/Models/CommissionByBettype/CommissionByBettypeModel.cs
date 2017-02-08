namespace Alpha.Reports.Models.CommissionByBettype
{
    public class CommissionByBettypeModel
    {
        public int CustId { get; set; }

        public int Recommend { get; set; }

        public int Mrecommend { get; set; }

        public string UserName { get; set; }

        public decimal TotalComm_1 { get; set; }

        public decimal TotalComm_2 { get; set; }

        public decimal TotalComm_3 { get; set; }

        public decimal MO_Comm { get; set; }

        public decimal Casino_Comm { get; set; }

        public decimal Other_Comm { get; set; }

        public decimal Charge_Comm { get; set; }
    }
}
