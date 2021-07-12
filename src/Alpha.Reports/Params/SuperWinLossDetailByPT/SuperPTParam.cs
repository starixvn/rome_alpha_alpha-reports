namespace Alpha.Reports.Params.SuperWinLossDetailByPT
{
    using System;

    public class SuperPTParam
    {
        public SuperPTParam(int superId)
        {
            SuperId = Convert.ToString(superId);
        }

        public string SuperId { get; }
    }
}