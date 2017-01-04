using System.Collections.Generic;
using Alpha.Reports.Models.Outstanding;
using Fanex.Data;

namespace Alpha.Reports.DataAccesses.Impl
{
    public class OutstandingDataAccessImpl : OutstandingDataAccess
    {
        public IEnumerable<OutstandingModel> GetMemberOutstandingForSuper(int superId)
        {
            return GetMemberOutstandingDynamic("Age_Rpt_Outstanding_SAg", superId);
        }

        public IEnumerable<OutstandingModel> GetMemberOutstandingForMaster(int masterId)
        {
            return GetMemberOutstandingDynamic("Age_Rpt_Outstanding_MAg", masterId);
        }

        public IEnumerable<OutstandingModel> GetMemberOutstandingForAgent(int agentId)
        {
            return GetMemberOutstandingDynamic("Age_Rpt_Outstanding_Ag", agentId);
        }

        private IEnumerable<OutstandingModel> GetMemberOutstandingDynamic(string settingKey, int custId)
        {
            IEnumerable<OutstandingModel> model = new List<OutstandingModel>();

            using (ObjectDb db = new ObjectDb(settingKey))
            {
                model = db.Query<OutstandingModel>(new { custId = custId });
            }

            return model;
        }
    }
}
