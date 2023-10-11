namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Alpha.PartialUM.Services;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Resources;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface BetCreditHistoryService
    {
        BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param);
    }

    public class BetCreditHistoryServiceImpl : BetCreditHistoryService
    {
        private const string LogActionCacheKey = "Alpha.Reports.BetCreditTracking.BetCreditHistory.LogAction";

        private const string TransLogSettled = "TransLog settled,";
        private const string TransLog = "TransLog";
        private const string LC = "LC";
        private const string RNG = "RNG";
        private const string Bet = "Bet";
        private const string BetUpdated = "Bet updated,";
        private const string BetSettled = "Bet settled,";
        private const string BetRejected = "Bet rejected";
        private const string VOID = "VOID";
        private const string Move = "Move";
        private const string voided = "voided";
        private const string to = "to";
        private const string to0 = "to 0";
        private const string to1 = "to 1";

        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly ICacheService cacheService;

        public BetCreditHistoryServiceImpl(IDynamicRepository dynamicRepository, IPartialUMService partialUMService, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.cacheService = cacheService;
        }

        public BetCreditHistoryResultModel GetBetCreditHistory(BetCreditHistoryParam param)
        {
            var criteria = new BetCreditHistoryCriteria(param);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var reportData = dynamicRepository.Fetch<BetCreditHistoryModel>(criteria);

            foreach (BetCreditHistoryModel row in reportData)
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
            switch (actionId)
            {
                case -1:
                    return BetCreditHistoryResources.OpeningCredit;

                case 0:
                    return TransformActionIdEqualsTo0(action);

                case 2:
                    return GetActionName2(action);

                case 10:
                case 11:
                case 12:
                    return GetActionName10(action);

                case 13:
                    return GetActionName13(action);

                case 20:
                    return TransformActionIdEqualsTo20(action);

                case 29:
                    return GetActionStatusToStatus(BetCreditHistoryResources.CashOut, action);

                case 30:
                    return TransformActionIdEqualsTo30(action);

                case 31:
                    return TransformActionIdEqualsTo31(action);

                case 32:
                    return TransformActionIdEqualsTo32(action);

                case 34:
                    return GetActionStatusToStatus(BetCreditHistoryResources.CancelCashOut, action);

                case 50:
                    return TransformActionIdEqualsTo50(action);

                case 55:
                    return TransformActionIdEqualsTo55(action);

                case 110:
                    return GetTransferSubmitted(action);

                default:
                    return action;
            }
        }

        private static string TransformActionIdEqualsTo0(string action)
        {
            if (IsMatchPrefixPattern(action, TransLogSettled)
                || IsMatchPrefixPattern(action, BetUpdated))
            {
                return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
            }

            if (IsMatchPrefixPattern(action, LC)
                || IsMatchPrefixPattern(action, RNG))
            {
                return IsHaveStatusToStatus(action)
                    ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                    : BetCreditHistoryResources.BetUpdated;
            }

            return action;
        }

        private static string TransformActionIdEqualsTo20(string action)
        {
            if (action.Trim().Equals(BetRejected, StringComparison.OrdinalIgnoreCase))
            {
                return BetCreditHistoryResources.BetRejected;
            }

            return action;
        }

        private static string TransformActionIdEqualsTo30(string action)
        {
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

            return action;
        }

        private static string TransformActionIdEqualsTo31(string action)
        {
            if (IsMatchPrefixPattern(action, TransLog)
                || IsMatchPrefixPattern(action, BetUpdated))
            {
                return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
            }

            if (IsMatchPrefixPattern(action, LC)
                || IsMatchPrefixPattern(action, RNG))
            {
                return IsHaveStatusToStatus(action)
                    ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                    : BetCreditHistoryResources.BetUpdated;
            }

            return action;
        }

        private static string TransformActionIdEqualsTo32(string action)
        {
            if (IsMatchPrefixPattern(action, TransLog)
                || IsMatchPrefixPattern(action, BetUpdated)
                || IsMatchPrefixPattern(action, Move))
            {
                return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
            }

            if (IsMatchPrefixPattern(action, LC)
                || IsMatchPrefixPattern(action, RNG))
            {
                return IsHaveStatusToStatus(action)
                    ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                    : BetCreditHistoryResources.BetUpdated;
            }

            return action;
        }

        private static string TransformActionIdEqualsTo50(string action)
        {
            if (IsMatchPrefixAndSuffixPattern(action, Bet, VOID)
                || IsMatchPrefixAndSuffixPattern(action, TransLog, VOID)
                || IsMatchPrefixAndSuffixPattern(action, LC, VOID)
                || IsMatchPrefixAndSuffixPattern(action, RNG, VOID))
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

            return action;
        }

        private static string TransformActionIdEqualsTo55(string action)
        {
            if (IsMatchPrefixPattern(action, TransLog))
            {
                return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
            }

            if (IsMatchPrefixPattern(action, LC)
                || IsMatchPrefixPattern(action, RNG))
            {
                return GetBetUpdatedVoidToStatus(action);
            }

            return action;
        }

        private static bool IsHaveStatusToStatus(string action)
            => action.IndexOf(to, StringComparison.OrdinalIgnoreCase) != -1 &&
                   action.IndexOf(to0, StringComparison.OrdinalIgnoreCase) == -1 &&
                   action.IndexOf(to1, StringComparison.OrdinalIgnoreCase) == -1;


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

        private static string GetActionStatusToStatus(string name, string action)
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

        private static string GetTransferSubmitted(string action)
        {
            var refNo = action.Split(new[] { "inserted" }, StringSplitOptions.None)[1].Trim();
            return BetCreditHistoryResources.TransferSubmitted + " " + refNo;
        }

        private static string GetActionName2(string action)
        {
            var strTo = " to ";
            var toPos = action.ToLowerInvariant().LastIndexOf(strTo);

            var prefixLength = 26;
            var d1 = action.Substring(prefixLength, toPos - prefixLength);
            var d2 = action.Substring(toPos + strTo.Length, action.Length - toPos - strTo.Length);

            return string.Format(BetCreditHistoryResources.Action2, d1, d2);
        }

        private static string GetActionName13(string action)
        {
            var strTo = " to ";
            var toPos = action.ToLowerInvariant().LastIndexOf(strTo);

            var prefixLength = 13;
            var s1 = action.Substring(prefixLength, toPos - prefixLength).ToLowerInvariant();
            var s2 = action.Substring(toPos + strTo.Length, action.Length - toPos - strTo.Length).ToLowerInvariant();

            return BetCreditHistoryResources.BetSettled + ", "
                + BetCreditHistoryResources.ResourceManager.GetString(s1) + " "
                + BetCreditHistoryResources.to + " "
                + BetCreditHistoryResources.ResourceManager.GetString(s2);
        }

        private static string GetActionName10(string action)
        {
            var a = action.ToLowerInvariant();
            var afterAs = action.Split(new[] { "as" }, StringSplitOptions.None)[1].Trim();
            var status = afterAs.Split(' ')[0].Trim().ToLowerInvariant();

            var waitings = new[] { "waiting", "running" };
            return waitings.Any(s => a.Contains(s))
                ? BetCreditHistoryResources.BetPlaced
                : BetCreditHistoryResources.BetPlacedAs + " " + BetCreditHistoryResources.ResourceManager.GetString(status);
        }
    }
}
