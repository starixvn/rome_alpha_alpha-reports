namespace Alpha.Reports.Extensions
{
    public static class IntExtensions
    {
        public static bool IsBetween(this int value, int start, int end)
            => start <= value && value <= end;
    }
}
