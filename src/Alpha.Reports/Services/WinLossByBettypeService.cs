namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.WinLossByBetTypeCriteria;
    using DataAccesses;
    using Models.WinlossByBetType;
    using Params;

    public interface WinLossByBetTypeService
    {
        IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByBetTypeParam param);
    }

    public class WinLossByBettypeServiceImpl : WinLossByBetTypeService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBetTypeDataAccess dataAccess;

        public WinLossByBettypeServiceImpl(
            ReportDateService reportDateService,
            WinLossByBetTypeDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
        }

        public IEnumerable<WinLossByBetTypeModel> GetWinLossByBetType(WinLossByBetTypeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossByBetTypeCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            var winlossData = dataAccess.GetWinLossByBetType(criteria);

            return winlossData;
        }
    }
}