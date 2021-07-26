namespace Alpha.Reports.Services
{
    using Alpha.Web.Helpers;
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;
    using Constants;
    using Criteria.SuperWinLossDetailByPT;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Models.SuperWinLossDetailByPT;
    using Params.SuperWinLossDetailByPT;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;

    public interface SuperWinLossDetailByPTService
    {
        IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param);

        IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param);
    }

    public class SuperWinLossDetailByPTServiceImpl : SuperWinLossDetailByPTService
    {
        private readonly IDynamicRepository repository;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService refService;
        private readonly PartialUMService partialUMService;

        public SuperWinLossDetailByPTServiceImpl(
            IDynamicRepository repository,
            ReportDateService reportDateService,
            CacheRefService refService,
            PartialUMService partialUMService)
        {
            this.repository = repository;
            this.reportDateService = reportDateService;
            this.refService = refService;
            this.partialUMService = partialUMService;
        }

        public IEnumerable<SuperPTModel> GetSuperPositionTakings(SuperPTParam param)
            => repository
            .Fetch<SuperPTModel>(new SuperPTCriteria(param.SuperId))
            .Concat(GetSuperPositionTakingsByRnG(param.SuperId));

        public IEnumerable<SuperWinLossDetailByProductModel> GetSuperWinLossDetailByProducts(SuperWinLossDetailByPTParam param)
        {
            var criteria = new SuperWinLossDetailByPTCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay(),
                param.UserSelectedProductIds,
                param.ProductDetailsXML);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return repository.Fetch<SuperWinLossDetailByProductModel>(criteria);
        }

        private IEnumerable<SuperPTModel> GetSuperPositionTakingsByRnG(string superId)
        {
            try
            {
                var customer = new Customer();
                var rngCasinoPositionTakingsTable = customer.GetSuperDesktopPT(new[] { superId });
                var rngCasinoProductName = refService.GetProductName(ProductIdConstants.RNGCasino);

                var rngCasinoPTs = new List<SuperPTModel>();
                foreach (DataRow row in rngCasinoPositionTakingsTable.Rows)
                {
                    rngCasinoPTs.Add(new SuperPTModel(
                        Converter.ToInt(row["OBSuperID"]),
                        Converter.ToDecimal(row["SPositionTaking"]),
                        ProductIdConstants.RNGCasino,
                        rngCasinoProductName));
                }

                return rngCasinoPTs;
            }
            catch (ErrorException casinoEx)
            {
                ExceptionHandler.Handle(new Exception(casinoEx.Message, casinoEx), TraceEventType.Information);
                return Enumerable.Empty<SuperPTModel>();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return Enumerable.Empty<SuperPTModel>();
            }
        }
    }
}