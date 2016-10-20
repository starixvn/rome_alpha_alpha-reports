namespace Alpha.Reports.Services.Impl
{
    using System;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public class ArrangeBetsInDatabaseServiceImpl : ArrangeBetsInDatabaseService
    {
        private const string TimeToArrangeBetsInDatabaseKey = "Alpha.Reports.TimeToArrangeBetsInDatabase";

        private readonly IDynamicRepository dynamicRepository;

        public ArrangeBetsInDatabaseServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public TimeToArrangeBetsInDatabase GetTimeToArrangeBetsInDatabase()
        {
            var timeToArrangeBetsInDatabase = CacheService.Get<TimeToArrangeBetsInDatabase>(TimeToArrangeBetsInDatabaseKey);

            if (timeToArrangeBetsInDatabase == null)
            {
                var criteria = new TimeToArrangeBetsInDatabaseCriteria();

                timeToArrangeBetsInDatabase = dynamicRepository.Get<TimeToArrangeBetsInDatabase>(criteria);

                CacheService.Set(TimeToArrangeBetsInDatabaseKey, timeToArrangeBetsInDatabase, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return timeToArrangeBetsInDatabase;
        }
    }
}
