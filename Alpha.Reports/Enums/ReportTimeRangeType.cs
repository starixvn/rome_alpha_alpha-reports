namespace Alpha.Reports.Enums
{
    public enum ReportTimeRangeType
    {
        /// <summary>
        /// Default date.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///  From the latest BalanceUp date.
        /// </summary>
        BalanceUp = 1,

        /// <summary>
        /// First day of previous 3 Months.
        /// </summary>
        FromLast3Months = 2,

        /// <summary>
        /// First date of previous 18 Months.
        /// </summary>
        FromLast18Months = 4,

        /// <summary>
        /// First date of first month of previous 3 Years.
        /// </summary>
        FromLast3Years = 5
    }
}
