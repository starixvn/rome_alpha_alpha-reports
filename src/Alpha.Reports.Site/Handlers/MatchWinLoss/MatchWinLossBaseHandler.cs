namespace Alpha.Reports.Site.Handlers.MatchWinLoss
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using App_GlobalResources;
    using Constants;
    using RemotingRef;
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

        protected IEnumerable<MatchWinLossViewModel> BuildViewModel(IEnumerable<MatchWinLossModel> models)
        {
            var viewModels = new List<MatchWinLossViewModel>();
            if (models == null)
            {
                return viewModels;
            }

            foreach (var matchWinLossModel in models)
            {
                var eventInfo = GetEventInfo(matchWinLossModel);
                var betTypeName = $"{GetBetTypePrefix(matchWinLossModel)}{cacheRefService.GetBetType(matchWinLossModel.BetType)}";

                viewModels.Add(new MatchWinLossViewModel(eventInfo, betTypeName, matchWinLossModel));
            }

            return viewModels;
        }

        protected MatchWinLossViewModel AccumulateGrandTotal(IEnumerable<MatchWinLossViewModel> models)
        {
            var grandTotal = new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 0, 0, 0);

            foreach (var model in models)
            {
                grandTotal.Add(model);
            }

            return grandTotal;
        }

        private string GetEventInfo(MatchWinLossModel model)
        {
            if (BetTypeHelper.IsNumberGame(model.BetType))
            {
                return $"{Resources.NumberGameNo} {model.MatchCode}";
            }

            return string.Join(
                " ",
                cacheRefService.GetTeamName(model.HomeId) + (model.IsNeutral ? Resources.Neutral : string.Empty),
                Resources.vs,
                cacheRefService.GetTeamName(model.AwayId));
        }

        private string GetBetTypePrefix(MatchWinLossModel model)
        {
            switch (model.BetType)
            {
                case BetTypeIdConstants.NumberGameOverUnder:
                    return $"{Resources.Next} ";

                case BetTypeIdConstants.NumberGameOddEven:
                    if (model.LiveIndicator)
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