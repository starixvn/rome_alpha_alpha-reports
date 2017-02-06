namespace Alpha.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria.WinLossByGameType;
    using DataAccesses;
    using Models.WinlossByGameType;
    using Params;

    public interface WinLossByGameTypeService
    {
        IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeParam param);
    }

    public class WinLossByGameTypeServiceImpl : WinLossByGameTypeService
    {
        private readonly ReportDateService reportDateService;
        private readonly WinLossByGameTypeDataAccess dataAccess;

        public WinLossByGameTypeServiceImpl(
            ReportDateService reportDateService,
            WinLossByGameTypeDataAccess dataAccess)
        {
            this.reportDateService = reportDateService;
            this.dataAccess = dataAccess;
        }

        public IEnumerable<WinLossByGameTypeModel> GetWinLossByGameType(WinLossByGameTypeParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var criteria = new WinLossByGameTypeCriteria(
                param.CustId,
                param.ProductId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                param.CustLevel);

            var winlossData = dataAccess.GetWinLossByGameType(criteria);

            return winlossData;
        }
    }
}