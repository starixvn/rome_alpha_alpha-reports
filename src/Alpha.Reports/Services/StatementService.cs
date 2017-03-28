using System.Collections.Generic;
using System.Linq;
using Alpha.Reports.Criteria.Statement;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Params.Statement;
using Fanex.Data.Repository;

namespace Alpha.Reports.Services
{
    public interface StatementService
    {
        IEnumerable<StatementModel> GetStatements(GetStatementParam param);

        IEnumerable<StatementDetailModel> StatementDetails(StatementDetailsParam param);
    }

    public class StatementServiceImpl : StatementService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository repository;

        public StatementServiceImpl(ReportDateService reportDateService, IDynamicRepository repository)
        {
            this.reportDateService = reportDateService;
            this.repository = repository;
        }

        public IEnumerable<StatementModel> GetStatements(GetStatementParam param)
        {
            var timeRanges = reportDateService.GetTimeRangesToGetReportDataBetweenTables(param.FromDate, param.ToDate);
            IEnumerable<StatementModel> result = new List<StatementModel>();

            foreach (var timeRange in timeRanges)
            {
                var rawData = repository.Fetch<StatementModel>(new GetStatementCriteria(
                    param.CustId,
                    timeRange.FromDate,
                    timeRange.ToDate,
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
