using System;
using System.Collections.Generic;
using Alpha.IntegrationTest.Mapper;
using Alpha.Reports.StaticData.DataAccesses;

namespace Alpha.Reports.StaticData.Configurations
{
    public class MapperForStaticData : IMapperForStaticData
    {
        public IDictionary<string, string> ResultModelToExcelFile
             => new Dictionary<string, string>
             {
                 ["ConsolidationByUsername"] = "Consolidation\\ByUsername.xls",
                 ["ConsolidationByDate"] = "Consolidation\\ByDate.xls",
                 ["ConsolidationByProduct"] = "Consolidation\\ByProduct.xls"
             };

        public IDictionary<string, Type> CriteriaToRepository
            => new Dictionary<string, Type>
            {
                ["GetConsolidationCriteria"] = typeof(ConsolidationExcelRepository)
            };

        public IDictionary<string, string> CriteriaToExcelFile => null;
    }
}
