using System;
using System.Collections.Generic;
using Alpha.Reports.Models.Consolidation;

namespace Alpha.Reports.StaticData.Configurations
{
    public static class StaticDataMapper
    {
        [ThreadStatic]
        private static IDictionary<string, string> modelMaps;

        public static IDictionary<string, string> ModelMaps
        {
            get
            {
                if (modelMaps == null)
                {
                    modelMaps = FromExpectedModelToExcel();
                }

                return modelMaps;
            }

            set { modelMaps = value; }
        }

        private static IDictionary<string, string> FromExpectedModelToExcel() => new Dictionary<string, string>
        {
            [nameof(ConsolidationByUsername)] = "Consolidation\\ByUsername.xls",
            [nameof(ConsolidationByDate)] = "Consolidation\\ByDate.xls",
            [nameof(ConsolidationByProduct)] = "Consolidation\\ByProduct.xls"
        };
    }
}
