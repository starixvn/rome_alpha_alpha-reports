namespace Alpha.Reports.SpamBlocker.Models
{
    public class ThrottlePolicy
    {
        public int Timer { get; set; }

        public int MaxRequests { get; set; }
    }
}
