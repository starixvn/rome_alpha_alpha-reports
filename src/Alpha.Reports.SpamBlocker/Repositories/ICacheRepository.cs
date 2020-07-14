namespace Alpha.Reports.SpamBlocker.Repositories
{
    using Alpha.Reports.SpamBlocker.Models;
    using System;

    public interface ICacheRepository
    {
        bool Any(string id);

        ThrottleCounter FirstOrDefault(string id);

        void Save(string id, ThrottleCounter throttleCounter, TimeSpan slidingExpiration);

        void Remove(string id);
    }
}
