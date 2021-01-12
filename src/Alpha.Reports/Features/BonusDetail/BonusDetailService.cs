namespace Alpha.Reports.Features.BonusDetail
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Models;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Fanex.Data.Repository;

    public interface BonusDetailService
    {
        Task<PagedReportData<GroupBonus>> GetBonusDetailAsync(BonusDetailParam param);

        Task<Result> ConfirmPaymentAsync(ConfirmPaymentParam param);
    }

    public class BonusDetailServiceImpl : BonusDetailService
    {
        private readonly IDynamicRepository repository;

        public BonusDetailServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public async Task<PagedReportData<GroupBonus>> GetBonusDetailAsync(BonusDetailParam param)
        {
            GetBonusDetailCriteria criteria = null;
            Func<string, IEnumerable<BonusEntity>, GroupBonus> getLevelBonus;

            if (param.ViewerLevel.IsSuperLevel())
            {
                criteria = new GetSuperBonusCriteria(param);
                getLevelBonus = GetSuperBonus;
            }
            else if (param.ViewerLevel.IsMasterLevel())
            {
                criteria = new GetMasterBonusCriteria(param);
                getLevelBonus = GetMasterBonus;
            }
            else
            {
                criteria = new GetAgentBonusCriteria(param);
                getLevelBonus = GetAgentBonus;
            }

            var rawData = await repository.FetchAsync<BonusEntity>(criteria);
            return GetPagedBonusDetail(param, criteria.TotalRecords, rawData, getLevelBonus);
        }

        private static PagedReportData<GroupBonus> GetPagedBonusDetail(
            BonusDetailParam param,
            int totalRecords,
            IEnumerable<BonusEntity> rawData,
            Func<string, IEnumerable<BonusEntity>, GroupBonus> getLevelBonus)
        {
            var totalAmount = 0M;
            var bonusList = new List<GroupBonus>();

            rawData.GroupBy(b => b.CreatedDate.Date).ToList().ForEach(group =>
            {
                var bonus = getLevelBonus(group.Key.ToShortDateWithSlash(), group);
                bonusList.Add(bonus);

                totalAmount += bonus.TotalAmount;
            });

            return new PagedReportData<GroupBonus>(bonusList, param.PageIndex, param.PageSize, totalRecords, new GroupBonus(totalAmount));
        }

        private static GroupBonus GetSuperBonus(string groupBy, IEnumerable<BonusEntity> entities)
        {
            var colspan = 0;
            var totalAmount = 0M;
            var masterBonusList = new List<GroupBonus>();
            GroupBonus masterBonus;

            var bonusEntityList = entities.GroupBy(b => b.MasterName).ToList();
            bonusEntityList.ForEach(grp =>
            {
                masterBonus = GetMasterBonus(grp.Key, grp);
                masterBonusList.Add(masterBonus);

                totalAmount += masterBonus.TotalAmount;
                colspan += masterBonus.ColSpan;
            });

            return new GroupBonus(groupBy, totalAmount, masterBonusList, colspan);
        }

        private static GroupBonus GetMasterBonus(string groupBy, IEnumerable<BonusEntity> entities)
        {
            var colspan = 0;
            var totalAmount = 0M;
            var agentBonusList = new List<GroupBonus>();
            GroupBonus agentBonus;

            var bonusEntityList = entities.GroupBy(b => b.AgentName).ToList();
            bonusEntityList.ForEach(grp =>
            {
                agentBonus = GetAgentBonus(grp.Key, grp);
                agentBonusList.Add(agentBonus);

                totalAmount += agentBonus.TotalAmount;
                colspan += agentBonus.Bonus.Count;
            });

            return new GroupBonus(groupBy, totalAmount, agentBonusList, colspan);
        }

        private static GroupBonus GetAgentBonus(string groupBy, IEnumerable<BonusEntity> entities)
        {
            var totalAmount = 0M;
            var bonusList = new List<BonusEntity>();

            var entityList = entities.ToList();
            entityList.ForEach(bonus =>
            {
                bonusList.Add(bonus);

                if (!bonus.IsCancelled)
                {
                    totalAmount += bonus.Amount;
                }
            });

            return new GroupBonus(groupBy, totalAmount, bonusList);
        }

        public async Task<Result> ConfirmPaymentAsync(ConfirmPaymentParam param)
        {
            var criteria = new ConfirmPaymentCriteria(param);
            await repository.ExecuteAsync(criteria);

            return new Result(criteria.ErrCode, criteria.ErrMsg);
        }
    }
}
