namespace Alpha.Reports.Site.ViewModels.SuperWinLossDetailByPT
{
    using App_GlobalResources;

    public class SuperPtProductViewModel
    {
        public SuperPtProductViewModel(
            decimal positionTaking,
            string productNames)
        {
            PositionTaking = positionTaking;
            ProductNames = productNames;
        }

        public decimal PositionTaking { get; }

        public string ProductNames { get; }

        public string Message
        {
            get
            {
                const decimal percentage = 100.0m;
                var positionTakingPercentage = PositionTaking * percentage;
                var positionTakingText = string.Concat(positionTakingPercentage.ToString("##0"), "% {0}");

                return string.Concat(
                    string.Format(positionTakingText, Resources.PT),
                    $": {ProductNames}");
            }
        }
    }
}