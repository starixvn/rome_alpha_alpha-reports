using System;
using System.Collections.Generic;
using Alpha.Reports.Criteria.Consolidation;
using Alpha.Reports.StaticData.Repositories;
using Alpha.StaticData.Mappers;

namespace Alpha.Reports.StaticData.Configurations
{
    public class RepositoryMapper : IRepositoryMapper
    {
        public IDictionary<string, Type> Maps
              => new Dictionary<string, Type>
              {
                  [nameof(GetConsolidationCriteria)] = typeof(ConsolidationExcelRepository),
                  [nameof(GetConsolidationReportProductsCriteria)] = typeof(ConsolidationExcelRepository)
              };
    }
}
