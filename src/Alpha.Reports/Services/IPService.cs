namespace Alpha.Reports.Services
{
    using Alpha.Reports.Criteria;
    using Alpha.Reports.Models;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public interface IPService
    {
        bool IsVNIP(string ipAddress);
    }

    public class IPServiceImpl : IPService
    {
        private const string VNIPListCacheKey = "Alpha.BetLists.Service.IPService.VNIPList";
        private readonly IDynamicRepository dynamicRepository;

        public IPServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            double ipNumber = ConvertIPToNumber(ipAddress);
            var ipVnRange = GetVNIPs();

            return ipVnRange.Any(ip => ip.From <= ipNumber && ipNumber <= ip.To);
        }

        private IEnumerable<IPRange> GetVNIPs()
        {
            var cache = new CacheServiceImpl();

            var vietnamIPs = cache.GetOrSet(
                VNIPListCacheKey,
                () => dynamicRepository.Fetch<IPRange>(new GetIPVNCriteria()));

            return vietnamIPs;
        }

        private static long ConvertIPToNumber(string ipAddress)
        {
            IPAddress parsedIpAddress;

            if (IPAddress.TryParse(ipAddress, out parsedIpAddress))
            {
                var addressBytes = parsedIpAddress.GetAddressBytes();
                Array.Reverse(addressBytes);

                return BitConverter.ToUInt32(addressBytes, 0);
            }

            return 0;
        }
    }

}
