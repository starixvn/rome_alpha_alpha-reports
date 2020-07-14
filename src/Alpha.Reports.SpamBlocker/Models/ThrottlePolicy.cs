namespace Alpha.Reports.SpamBlocker.Models
{
    public class ThrottlePolicy
    {
        public int PerSeconds { get; set; }

        public int MaxRequests { get; set; }
    }
}
