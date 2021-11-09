namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using Alpha.Reports.Models.Consolidation;
    using Alpha.Reports.Site.App_GlobalResources;

    public class ConsolidationByUsernameGroupByPTViewModel
    {
        public ConsolidationByUsernameGroupByPTViewModel(
            decimal positionTaking,
            ConsolidationEntityViewModel total,
            IEnumerable<ConsolidationByUsername> consolidationByUsernames)
        {
            PositionTaking = positionTaking;
            Total = total;
            ConsolidationByUsernames = consolidationByUsernames;
        }

        public decimal PositionTaking { get; }

        public string PositionTakingText
        {
            get
            {
                const int percentage = 100;
                var positionTakingPercentage = PositionTaking * percentage;

                return $"{positionTakingPercentage.ToString("#,##0.0")}% {Resources.PT}";
            }
        }

        public ConsolidationEntityViewModel Total { get; }

        public IEnumerable<ConsolidationByUsername> ConsolidationByUsernames { get; }
    }
}