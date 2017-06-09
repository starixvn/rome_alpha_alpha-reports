namespace Alpha.Reports.Params
{
    public class DailyEnteredSportParam
    {
        public DailyEnteredSportParam(bool isShowTestData)
        {
            IsShowTestData = isShowTestData;
        }

        public bool IsShowTestData { get; }
    }
}