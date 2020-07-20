namespace Alpha.Reports.SpamBlocker.Models
{
    public class ThrottlePolicy
    {
        public int PeriodInSeconds { get; set; }

        public int MaxRequests { get; set; }
    }
}
