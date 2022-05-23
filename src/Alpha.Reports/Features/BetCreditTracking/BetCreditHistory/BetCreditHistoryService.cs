namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using Alpha.Reports.Constants;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Resources;
    using Alpha.Web.Helpers;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public interface BetCreditHistoryService
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private const string LogActionCacheKey = "Alpha.Reports.BetCreditTracking.BetCreditHistory.LogAction";

        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;
        private readonly ICacheService cacheService;

        public BetCreditHistoryServiceImpl(IDynamicRepository dynamicRepository, PartialUMService partialUMService, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.cacheService = cacheService;
        }

        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param)
        {
            var criteria = new BetCreditHistoryCriteria(param);
            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var reportData = dynamicRepository.Fetch<BetCreditHistoryModel>(criteria);

            foreach(BetCreditHistoryModel row in reportData)
            {
                row.Action = row.IsUserLogActionId == 1 ? 
                    GetLogActionName(row.Action, row.ActionId, param.Language) : 
                    ConvertActionName(row.Action, row.ActionId);
            }    

            return new BetCreditHistoryResultModel(criteria.ErrorCode, criteria.ErrorMess, reportData, criteria.TotalRows);
        }

        private string GetLogActionName(string action, int actionId, string language)
        {
            var logAction = GetLogAction(actionId);

            if (logAction == null)
            {
                return action;
            }

            switch (language)
            {
                case LanguageCode.ChineseTraditional:
                    return logAction.ChineseTraditionalName;

                case LanguageCode.ChineseSimplified:
                    return logAction.ChineseSimplifiedName;

                case LanguageCode.Japanese:
                    return logAction.JapaneseName;

                case LanguageCode.Thai:
                    return logAction.ThaiName;

                case LanguageCode.Korean:
                    return logAction.KoreanName;

                case LanguageCode.Vietnamese:
                    return logAction.VietnameseName;

                default:
                    return logAction.EnglishName;
            }
        }

        private LogAction GetLogAction(int actionId)
        {
            var logActionDictionary = cacheService.Get<Dictionary<int, LogAction>>(LogActionCacheKey);

            if (logActionDictionary == null
                || !logActionDictionary.Any())
            {
                logActionDictionary = dynamicRepository
                    .Fetch<LogAction>(new GetLogActionsCriteria())
                    .ToDictionary(logAction => logAction.Id);

                cacheService.Set(LogActionCacheKey, logActionDictionary, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return logActionDictionary.ContainsKey(actionId) ?
                logActionDictionary[actionId] :
                null;
        }

        private static string ConvertActionName(string action, int actionId)
        {
            const string TransLogSettled = "TransLog settled,";
            const string TransLog = "TransLog";
            const string LC = "LC";
            const string RNG = "RNG";
            const string Bet = "Bet";
            const string BetUpdated = "Bet updated,";
            const string BetSettled = "Bet settled,";
            const string VOID = "VOID";
            const string voided = "voided";
            const string to = "to";

            switch (actionId)
            {
                case -1:
                    return BetCreditHistoryResources.OpeningCredit;

                case 0:
                    if (IsMatchPrefixPattern(action, TransLogSettled) 
                        || IsMatchPrefixPattern(action, BetUpdated))
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
                    if (IsMatchPrefixPattern(action, BetUpdated))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
                    }

                    if (IsMatchPrefixPattern(action, TransLogSettled) 
                        || IsMatchPrefixPattern(action, LC)
                        || IsMatchPrefixPattern(action, RNG)
                        || IsMatchPrefixPattern(action, BetSettled))
                    {
                        return GetActionStatusToStatus(BetCreditHistoryResources.BetSettled, action);
                    }

                    break;

                case 31:
                    if (IsMatchPrefixPattern(action, TransLog) 
                        || IsMatchPrefixPattern(action, BetUpdated))
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
                    if (IsMatchPrefixPattern(action, TransLog)
                        || IsMatchPrefixPattern(action, BetUpdated))
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

            return Regex.IsMatch(str, string.Format(prefixPattern, prefix), RegexOptions.IgnoreCase);
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
