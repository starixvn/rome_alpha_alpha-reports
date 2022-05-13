namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Resources;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using System;
    using System.Text.RegularExpressions;

    public interface BetCreditHistoryService
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;

        public BetCreditHistoryServiceImpl(IDynamicRepository dynamicRepository, PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param)
        {
            var criteria = new BetCreditHistoryCriteria(param);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var reportData = dynamicRepository.Fetch<BetCreditHistoryModel>(criteria);

            foreach(BetCreditHistoryModel row in reportData)
            {
                row.Action = ConvertActionString(row.Action, row.ActionId);
            }    

            return new BetCreditHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData, criteria.TotalRows);
        }

        private static string ConvertActionString(string action, int actionId)
        {
            const string TransLogSettled = "TransLog settled,";
            const string TransLogUpdated = "TransLog updated,";
            const string TransLog = "TransLog";
            const string LC = "LC";
            const string RNG = "RNG";
            const string Bet = "Bet";
            const string VOID = "VOID";
            const string voided = "voided";
            const string to = "to";

            switch (actionId)
            {
                case 0:
                    if (IsMatchPrefixPattern(action, TransLogSettled))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }

                    if (IsMatchPrefixPattern(action, LC) 
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return action.IndexOf(to) != -1
                            ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                            : BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 10:
                    return GetBetPlaceAsWhichActionMayHaveTransId(action);

                case 11:
                case 12:
                    return GetBetPlaceAs(action);

                case 29:
                    return GetActionStatusToStatus(BetCreditHistoryResources.CashOut, action);

                case 30:
                    if (IsMatchPrefixPattern(action, TransLogSettled) 
                        || IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetSettled, action);
                    }

                    break;

                case 31:
                    if (IsMatchPrefixPattern(action, TransLogUpdated) 
                        || IsMatchPrefixPattern(action, Bet))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }

                    if (IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 32:
                    if (IsMatchPrefixPattern(action, TransLogUpdated)
                        || IsMatchPrefixPattern(action, Bet))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }

                    if (IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return action.IndexOf(to) != -1
                            ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                            : BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 34:
                    return GetActionStatusToStatus(BetCreditHistoryResources.CancelCashOut, action);

                case 50:
                    if (IsMatchPrefixAndSuffixPattern(action, Bet, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, TransLog, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, LC, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, RNG, VOID)
                        )
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }
                    
                    if (IsMatchPrefixAndSuffixPattern(action, Bet, voided)
                        || IsMatchPrefixAndSuffixPattern(action, TransLog, voided)
                        || IsMatchPrefixAndSuffixPattern(action, LC, voided)
                        || IsMatchPrefixAndSuffixPattern(action, RNG, voided))
                    {
                        return GetBetUpdatedRunningToVoid();
                    }

                    break;

                case 55:
                    if (IsMatchPrefixPattern(action, TransLog))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }

                    if (IsMatchPrefixPattern(action, LC) 
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return GetBetUpdatedVoidToStatus(action);
                    }

                    break;

                case 110:
                    return GetTransferSubmitted(action);

                default:
                    break;
            }

            return action;
        }

        private static bool IsMatchPrefixAndSuffixPattern(string str, string prefix, string suffix)
        {
            const string prefixAndSuffixPattern = "^({0})(.*)({1})$";

            return Regex.IsMatch(str, string.Format(prefixAndSuffixPattern, prefix, suffix), RegexOptions.IgnoreCase);
        }

        private static bool IsMatchPrefixPattern(string str, string prefix)
        {
            const string prefixPattern = "^({0})";

            return Regex.IsMatch(str, string.Format(prefixPattern, prefix));
        }

        private static string GetActionStatusToStatus(string name ,string action)
        {
            var statusToStatus = action.Split(',')[1];

            var splitStatus = statusToStatus.Split(new[] { "to" }, StringSplitOptions.None);
            var fromStatus = splitStatus[0].Trim().ToLowerInvariant();
            var toStatus = splitStatus[1].Trim().ToLowerInvariant();

            return name + ", " 
                + BetCreditHistoryResources.ResourceManager.GetString(fromStatus) + " " 
                + BetCreditHistoryResources.to + " " 
                + BetCreditHistoryResources.ResourceManager.GetString(toStatus); 
        }

        private static string GetBetUpdatedVoidToStatus(string action)
        {
            var status = action.Split(new[] { "to" }, StringSplitOptions.None)[1].Trim().ToLowerInvariant();
            return BetCreditHistoryResources.BetUpdated + ", "
                + BetCreditHistoryResources._void + " "
                + BetCreditHistoryResources.to + " "
                + BetCreditHistoryResources.ResourceManager.GetString(status);
        }

        private static string GetBetUpdatedRunningToVoid()
        {
            return BetCreditHistoryResources.BetUpdated + ", "
                + BetCreditHistoryResources.running + " "
                + BetCreditHistoryResources.to + " "
                + BetCreditHistoryResources._void;
        }

        private static string GetBetPlaceAsWhichActionMayHaveTransId(string action)
        {
            var afterAs = action.Split(new[] { "as" }, StringSplitOptions.None)[1].Trim();
            var status = afterAs.Split(' ')[0].Trim().ToLowerInvariant();

            return BetCreditHistoryResources.BetPlacedAs + " " + BetCreditHistoryResources.ResourceManager.GetString(status);
        }

        private static string GetBetPlaceAs(string action)
        {
            var status = action.Split(new[] { "as" }, StringSplitOptions.None)[1].Trim().ToLowerInvariant();
            return BetCreditHistoryResources.BetPlacedAs + " " + BetCreditHistoryResources.ResourceManager.GetString(status);
        }

        private static string GetTransferSubmitted(string action)
        {
            var refNo = action.Split(new[] { "inserted" }, StringSplitOptions.None)[1].Trim();
            return BetCreditHistoryResources.TransferSubmitted + " " + refNo;
        }
    }
}
