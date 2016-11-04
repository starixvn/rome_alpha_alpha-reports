namespace Alpha.Reports.Extensions
{
    public static class IntExtension
    {
        public static string ToNumericString(this decimal value)
        {
            return value.ToString($"#,##0.##");
        }
    }
}
