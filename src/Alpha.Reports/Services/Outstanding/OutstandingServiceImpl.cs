namespace Alpha.Reports.Services.Outstanding
{
    using System.Collections.Generic;
    using Criteria.Outstanding;
    using Fanex.Data.Repository;
    using Models;
    using Models.Outstanding;

    public class OutstandingServiceImpl : OutstandingService
    {
        private readonly IDynamicRepository repository;

        public OutstandingServiceImpl(IDynamicRepository dynamicRepository)
        {
            repository = dynamicRepository;
        }

        public IEnumerable<OutstandingModel> GetMemberOutstanding(UserLevel userLevel, UserLevel custLevel, int custId)
        {
            if (userLevel >= custLevel)
            {
                OutstandingCriteria criteria = new OutstandingCriteria(custLevel, custId);

                return repository.Fetch<OutstandingModel>(criteria);
            }

            return new List<OutstandingModel>();
        }
    }
}
