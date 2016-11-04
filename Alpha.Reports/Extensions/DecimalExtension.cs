namespace Alpha.Reports.Extensions
{
    public static class DecimalExtension
    {
        public static string ToNumericString(this int value)
        {
            return value.ToString($"#,##0.##");
        }
    }
}
