namespace Alpha.Reports.Extensions
{
    public static class DecimalExtension
    {
        public static string ToNumericString(this decimal value)
        {
            return value.ToString($"#,##0.##");
        }
    }
}
