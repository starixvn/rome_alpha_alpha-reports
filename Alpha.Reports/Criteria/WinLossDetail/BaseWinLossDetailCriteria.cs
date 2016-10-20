namespace Alpha.Reports.Criteria.WinLossDetail
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Params;

    public class BaseWinLossDetailCriteria : CriteriaBase
    {
        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDate { get; }

        public IEnumerable<ProductParam> ProductParams { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRows { get; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => FromDate <= ToDate && CustId > 0;

        public override Hashtable GetSpParams()
        {
            var parameters = new Hashtable();
            parameters.Add("custid", CustId);
            parameters.Add("fromdate", FromDate);
            parameters.Add("todate", ToDate);
            parameters.Add("balanceupday", BalanceUpDate);
            parameters.Add("pagesize", PageSize);
            parameters.Add("pageindex", PageIndex);
            parameters.Add("totalrows", TotalRows);

            foreach (var productParam in ProductParams)
            {
                parameters.Add(productParam.Name, productParam.IsSelected);
            }

            return parameters;
        }
    }
}
