namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Resources;
    using Fanex.Data.Repository;
    using System;
    using System.Text.RegularExpressions;

    public interface BetCreditHistoryService
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private readonly IDynamicRepository repository;

        public BetCreditHistoryServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param)
        {
            var criteria = new BetCreditHistoryCriteria(param);

            var reportData = repository.Fetch<BetCreditHistoryModel>(criteria);

            foreach(BetCreditHistoryModel row in reportData)
            {
                row.Action = ConvertActionString(row.Action, row.ActionId);
            }    

            return new BetCreditHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData);
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
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1]);
                    }

                    if (IsMatchPrefixPattern(action, LC) 
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return action.IndexOf(to) != -1
                            ? string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1])
                            : BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 10:
                case 11:
                case 12:
                    return string.Format(BetCreditHistoryResources.BetPlacedAs, action.Split(new[] { " as " }, StringSplitOptions.None)[1]);

                case 30:
                    if (IsMatchPrefixPattern(action, TransLogSettled) 
                        || IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return string.Format(BetCreditHistoryResources.BetSettled, action.Split(',')[1]);
                    }

                    break;

                case 31:
                    if (IsMatchPrefixPattern(action, TransLogUpdated) 
                        || IsMatchPrefixPattern(action, Bet))
                    {
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1]);
                    }

                    if (IsMatchPrefixPattern(action, RNG))
                    {
                        return BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 32:
                    if (IsMatchPrefixPattern(action, TransLogUpdated)
                        || IsMatchPrefixPattern(action, Bet))
                    {
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1]);
                    }

                    if (IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return action.IndexOf(to) != -1
                            ? string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1])
                            : BetCreditHistoryResources.BetUpdated;
                    }

                    break;

                case 50:
                    if (IsMatchPrefixAndSuffixPattern(action, Bet, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, TransLog, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, LC, VOID)
                        || IsMatchPrefixAndSuffixPattern(action, RNG, VOID)
                        )
                    {
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1]);
                    }
                    
                    if (IsMatchPrefixAndSuffixPattern(action, Bet, voided)
                        || IsMatchPrefixAndSuffixPattern(action, TransLog, voided)
                        || IsMatchPrefixAndSuffixPattern(action, LC, voided)
                        || IsMatchPrefixAndSuffixPattern(action, RNG, voided))
                    {
                        return BetCreditHistoryResources.BetUpdatedRunningToVoid;
                    }

                    break;

                case 55:
                    if (IsMatchPrefixPattern(action, TransLog))
                    {
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToStatus, action.Split(',')[1]);
                    }

                    if (IsMatchPrefixPattern(action, LC) 
                        || IsMatchPrefixPattern(action, RNG))
                    {
                        return string.Format(BetCreditHistoryResources.BetUpdatedStatusToVoid, action.Split(new[] { " to " }, StringSplitOptions.None)[1]);
                    }

                    break;

                case 110:
                    return string.Format(BetCreditHistoryResources.TransferSubmitted, action.Split(new[] { " as " }, StringSplitOptions.None)[1]);
                
                default:
                    break;
            }

            return action;
        }

        private static bool IsMatchPrefixAndSuffixPattern(string str, string prefix, string suffix)
        {
            const string prefixAndSuffixPattern = "^({0}) (.*) ({1})$";

            return Regex.IsMatch(str, string.Format(prefixAndSuffixPattern, prefix, suffix));
        }

        private static bool IsMatchPrefixPattern(string str, string prefix)
        {
            const string prefixPattern = "^({0})";

            return Regex.IsMatch(str, string.Format(prefixPattern, prefix));
        }
    }
}
