namespace Alpha.Reports.Models.SuperWinLossDetailByPT
{
    public class SuperWinLossDetailModel
    {
        public SuperWinLossDetailModel()
        {
        }

        public SuperWinLossDetailModel(
            SuperWinLossDetailByProductModel winLossDetailByProduct,
            SuperPTModel positionTaking)
        {
            ProductId = positionTaking.ProductId;
            ProductName = positionTaking.ProductName;
            PositionTaking = positionTaking.SuperPT;

            Turnover = winLossDetailByProduct.Turnover;
            NetTurnover = winLossDetailByProduct.NetTurnover;
            GrossComm = winLossDetailByProduct.GrossComm;

            MemberWinLost = winLossDetailByProduct.MemberWinLost;
            MemberComm = winLossDetailByProduct.MemberComm;
            MemberTotal = winLossDetailByProduct.MemberTotal;

            AgWinLost = winLossDetailByProduct.AgWinLost;
            AgComm = winLossDetailByProduct.AgComm;
            AgTotal = winLossDetailByProduct.AgTotal;

            MAgWinLost = winLossDetailByProduct.MAgWinLost;
            MAgComm = winLossDetailByProduct.MAgComm;
            MAgTotal = winLossDetailByProduct.MAgTotal;

            SAgWinLost = winLossDetailByProduct.SAgWinLost;
            SAgComm = winLossDetailByProduct.SAgComm;
            SAgTotal = winLossDetailByProduct.SAgTotal;

            Company = winLossDetailByProduct.Company;
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public decimal PositionTaking { get; }

        public decimal Turnover { get; private set; }

        public decimal NetTurnover { get; private set; }

        public decimal GrossComm { get; private set; }

        public decimal MemberWinLost { get; private set; }

        public decimal MemberComm { get; private set; }

        public decimal MemberTotal { get; private set; }

        public decimal AgWinLost { get; private set; }

        public decimal AgComm { get; private set; }

        public decimal AgTotal { get; private set; }

        public decimal MAgWinLost { get; private set; }

        public decimal MAgComm { get; private set; }

        public decimal MAgTotal { get; private set; }

        public decimal SAgWinLost { get; private set; }

        public decimal SAgComm { get; private set; }

        public decimal SAgTotal { get; private set; }

        public decimal Company { get; private set; }

        public SuperWinLossDetailModel Add(
            SuperWinLossDetailModel winLossDetailByPT)
        {
            Turnover += winLossDetailByPT.Turnover;
            NetTurnover += winLossDetailByPT.NetTurnover;
            GrossComm += winLossDetailByPT.GrossComm;

            MemberWinLost += winLossDetailByPT.MemberWinLost;
            MemberComm += winLossDetailByPT.MemberComm;
            MemberTotal += winLossDetailByPT.MemberTotal;

            AgWinLost += winLossDetailByPT.AgWinLost;
            AgComm += winLossDetailByPT.AgComm;
            AgTotal += winLossDetailByPT.AgTotal;

            MAgWinLost += winLossDetailByPT.MAgWinLost;
            MAgComm += winLossDetailByPT.MAgComm;
            MAgTotal += winLossDetailByPT.MAgTotal;

            SAgWinLost += winLossDetailByPT.SAgWinLost;
            SAgComm += winLossDetailByPT.SAgComm;
            SAgTotal += winLossDetailByPT.SAgTotal;

            Company += winLossDetailByPT.Company;

            return this;
        }
    }
}