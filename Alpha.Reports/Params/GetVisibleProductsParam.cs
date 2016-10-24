namespace Alpha.Reports.Params
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class GetVisibleProductsParam
    {
        public GetVisibleProductsParam(IEnumerable<Product> productSetting, Dictionary<string, Func<int, bool>> productPermissions)
        {
            ProductSetting = productSetting;
            ProductPermissions = productPermissions;
        }

        public IEnumerable<Product> ProductSetting { get; }

        public Dictionary<string, Func<int, bool>> ProductPermissions { get; }
    }
}
