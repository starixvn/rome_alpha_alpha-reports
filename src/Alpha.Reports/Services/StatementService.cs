namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
    using AlphaPlus.PartialUM.Services;
    using Extensions;
    using Fanex.Data.Repository;

    public interface StatementService
    {
        IEnumerable<StatementModel> GetStatements(GetStatementParam param);

        IEnumerable<StatementDetailModel> StatementDetails(StatementDetailsParam param);
    }

    public class StatementServiceImpl : StatementService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository repository;
        private readonly IPartialUMService partialUMService;

        public StatementServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository repository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.repository = repository;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<StatementModel> GetStatements(GetStatementParam param)
        {
            var timeRanges = reportDateService.GetTimeRangesToGetReportDataBetweenTables(param.FromDate, param.ToDate);
            var balanceUpDay = reportDateService.GetBalanceUpDay();
            IEnumerable<StatementModel> result = new List<StatementModel>();

            foreach (var timeRange in timeRanges)
            {
                var storeMappingSuffix = reportDateService.GetStoreMappingSuffix(timeRange.FromDate);

                var criteria = string.Equals(storeMappingSuffix, ReportDateServiceImpl.BetTrans14SPSuffix, StringComparison.OrdinalIgnoreCase)
                    ? new GetStatementBetTrans14Criteria(param.CustId, timeRange.FromDate.GetGreaterDate(param.LimitedDateToShowInfo), timeRange.ToDate, balanceUpDay)
                    : new GetStatementCriteria(param.CustId, timeRange.FromDate.GetGreaterDate(param.LimitedDateToShowInfo), timeRange.ToDate, storeMappingSuffix);

                partialUMService.ThrowIfUM(criteria.GetSettingKey());

                var rawData = repository.Fetch<StatementModel>(criteria);

                result = Enumerable.Concat(result, rawData);
            }

            return result;
        }

        public IEnumerable<StatementDetailModel> StatementDetails(StatementDetailsParam param)
        {
            var rawData = repository.Fetch<StatementDetailModel>(new TransferDetailsCriteria(
                    param.CustId,
                    param.TransferDate,
                    param.TransferDate >= reportDateService.GetBalanceUpDay() ? string.Empty : ReportDateServiceImpl.WarehouseSPSuffix));

            return rawData;
        }
    }
}