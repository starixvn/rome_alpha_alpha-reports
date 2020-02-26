namespace Alpha.Reports.Services
{
    using Sunplus.Core.Common.IPs;
    using Utilities.IP;

    /// <summary>
    /// TODO: This class must be removed.
    /// </summary>
    public class IPServiceImpl : IPService
    {
        public bool IsVNIP(string ipAddress)
        {
            return IpUtils.IsVnIp(ipAddress);
        }
    }
}