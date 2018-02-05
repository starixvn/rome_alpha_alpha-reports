using System;
using System.Collections.Generic;
using System.Data;
using Alpha.RemotingRef;
using Alpha.Reports.Criteria.ProgressiveContribution;
using Alpha.Reports.Models;
using Alpha.Reports.Models.ProgressiveContribution;
using Fanex.Common.Extensions;

namespace Alpha.Reports.DataAccesses
{
    public interface ProgressiveContributionDataAccess
    {
        IEnumerable<ProgressiveContributionByBetTypeModel> GetProgressiveContributions(ProgressiveContributionCriteria criteria);
    }

    public class ProgressiveContributionDataAccessImpl : ProgressiveContributionDataAccess
    {
        private readonly CacheRefService cacheRefService;

        public ProgressiveContributionDataAccessImpl(CacheRefService cacheRefService)
        {
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<ProgressiveContributionByBetTypeModel> GetProgressiveContributions(ProgressiveContributionCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var progressiveContributions = new List<ProgressiveContributionByBetTypeModel>();

            using (var db = ReportObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            progressiveContributions.Add(BuildProgressiveContribution(reader, criteria.ReportLevel));
                        }
                    }
                }
            }

            return progressiveContributions;
        }

        private ProgressiveContributionByBetTypeModel BuildProgressiveContribution(IDataReader reader, UserLevel reportLevel)
        {
            var productId = Convert.IsDBNull(reader["ProductID"]) ? 0 : reader.ToInt("ProductID");
            var bettypeId = reader.ToInt("BetType");

            var progressiveContribution = new ProgressiveContributionByBetTypeModel(
                productId,
                bettypeId,
                cacheRefService.GetBetTypeName(bettypeId),
                reader.ToDecimal("TurnOver"),
                reader.ToDecimal("Contribution"),
                reader.ToDecimal("ContributionAgent"),
                reportLevel.IsMasterOrSuperLevel() ? reader.ToDecimal("ContributionMaster") : 0,
                reportLevel.IsSuperLevel() ? reader.ToDecimal("ContributionSuper") : 0);

            return progressiveContribution;
        }
    }
}