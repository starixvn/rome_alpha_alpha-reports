namespace Alpha.Reports.Site.Handlers.MatchWinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using RemotingRef;
    using Reports.Constants;
    using Reports.Helpers;
    using Reports.Models.MatchWinLoss;
    using Services;
    using ViewModels.MatchWinLoss;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter",
        Justification = "Reviewed. Suppression is OK here.")]
    public class MatchWinLossBaseHandler : BaseHandler
    {
        protected readonly MatchWinLossService reportService;
        protected readonly CacheRefService cacheRefService;

        public MatchWinLossBaseHandler(MatchWinLossService reportService, CacheRefService cacheRefService)
        {
            this.reportService = reportService;
            this.cacheRefService = cacheRefService;
        }

        protected IEnumerable<MatchWinLossViewModel> BuildViewModel(IEnumerable<MatchWinLossModel> rawData)
        {
            var viewModels = new List<MatchWinLossViewModel>();
            if (rawData == null)
            {
                return viewModels;
            }

            foreach (var matchWinLossModel in rawData)
            {
                var eventInfo = GetEventInfo(matchWinLossModel);
                var betTypeName = $"{GetBetTypePrefix(matchWinLossModel)}{cacheRefService.GetBetType(matchWinLossModel.BetType)}";

                viewModels.Add(new MatchWinLossViewModel(eventInfo, betTypeName, matchWinLossModel));
            }

            return viewModels;
        }

        protected MatchWinLossViewModel AccumulateGrandTotal(IEnumerable<MatchWinLossViewModel> reportData)
        {
            var grandTotal = new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 0, 0, 0);

            if (reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal = grandTotal.Add(data);
                }
            }

            return grandTotal;
        }

        private string GetEventInfo(MatchWinLossModel matchWinLossModel)
        {
            if (BetTypeHelper.IsNumberGame(matchWinLossModel.BetType))
            {
                return $"{Resources.NumberGameNo} {matchWinLossModel.MatchCode}";
            }

            return string.Join(
                " ",
                cacheRefService.GetTeamName(matchWinLossModel.HomeId) + (matchWinLossModel.IsNeutral ? Resources.Neutral : string.Empty),
                Resources.vs,
                cacheRefService.GetTeamName(matchWinLossModel.AwayId));
        }

        private string GetBetTypePrefix(MatchWinLossModel matchWinLossModel)
        {
            switch (matchWinLossModel.BetType)
            {
                case BetTypeIdConstants.NumberGameOverUnder:
                    return $"{Resources.Next} ";

                case BetTypeIdConstants.NumberGameOddEven:
                    if (matchWinLossModel.LiveIndicator)
                    {
                        return $"{Resources.Next} ";
                    }

                    return $"{Resources.FT} ";

                default:
                    return string.Empty;
            }
        }
    }
}