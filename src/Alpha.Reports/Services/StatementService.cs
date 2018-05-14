namespace Alpha.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Criteria.Statement;
    using Alpha.Reports.DataAccesses;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Params.Statement;
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
        private readonly StatementDataAccess statementRepository;

        public StatementServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository repository,
            StatementDataAccess statementRepository)
        {
            this.reportDateService = reportDateService;
            this.repository = repository;
            this.statementRepository = statementRepository;
        }

        public IEnumerable<StatementModel> GetStatements(GetStatementParam param)
        {
            var timeRanges = reportDateService.GetTimeRangesToGetReportDataBetweenTables(param.FromDate, param.ToDate);
            var balanceUpDay = reportDateService.GetBalanceUpDay();
            IEnumerable<StatementModel> result = new List<StatementModel>();

            foreach (var timeRange in timeRanges)
            {
                var rawData = statementRepository.GetStatements(new GetStatementCriteria(
                    param.CustId,
                    timeRange.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                    timeRange.ToDate,
                    balanceUpDay,
                    reportDateService.GetStoreMappingSuffix(timeRange.FromDate)));

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
