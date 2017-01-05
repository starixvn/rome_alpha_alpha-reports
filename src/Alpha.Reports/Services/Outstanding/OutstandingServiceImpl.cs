using System;
using System.Collections.Generic;
using Alpha.Reports.DataAccesses.Outstanding;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Outstanding;

namespace Alpha.Reports.Services.Outstanding
{
    public class OutstandingServiceImpl : OutstandingService
    {
        private readonly OutstandingDataAccess _outstandingDataAccess;

        public OutstandingServiceImpl(OutstandingDataAccess outstandingDataAccess)
        {
            _outstandingDataAccess = outstandingDataAccess;
        }

        public IEnumerable<OutstandingModel> GetMemberOutstanding(UserLevel userLevel, UserLevel custLevel, int custId)
        {
            if (userLevel >= custLevel)
            {
                if (custLevel == UserLevel.Super)
                {
                    return _outstandingDataAccess.GetMemberOutstandingForSuper(custId);
                }
                else if (custLevel == UserLevel.Master)
                {
                    return _outstandingDataAccess.GetMemberOutstandingForMaster(custId);
                }
                else if (custLevel == UserLevel.Agent)
                {
                    return _outstandingDataAccess.GetMemberOutstandingForAgent(custId);
                }
            }

            return new List<OutstandingModel>();
        }
    }
}
