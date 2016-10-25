namespace Alpha.Reports.Criteria
{
    using System;
    using System.Collections.Generic;

    public class ReportProductCriteria : ReportCriteriaBase
    {
        public ReportProductCriteria(
            int custId, 
            int pageIndex, 
            int pageSize, 
            DateTime fromDate, 
            DateTime toDate, 
            List<int> productIdList) 
            : base(custId, pageIndex, pageSize, fromDate, toDate)
        {
            ProductIdList = productIdList;
        }

        public List<int> ProductIdList { get; }

        public string ProductIds
        {
            get
            {
                string productIds = string.Empty;

                ProductIdList.ForEach(id =>
                {
                    productIds += $"{id},";
                });

                return productIds.TrimEnd(',');
            }
        }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
