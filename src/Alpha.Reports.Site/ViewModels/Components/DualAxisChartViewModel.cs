namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class DualAxisChartViewModel
    {
        public DualAxisChartViewModel(
            string title,
            IEnumerable<string> labels,
            string firstDataLabel,
            string secondDataLabel,
            string firstData,
            string secondData,
            string currencyName)
        {
            Title = title;
            Labels = labels;
            FirstDataLabel = firstDataLabel;
            SecondDataLabel = secondDataLabel;
            FirstData = firstData;
            SecondData = secondData;
            CurrencyName = currencyName;
        }

        public string Title { get; }

        public IEnumerable<string> Labels { get; }

        public string FirstDataLabel { get; }

        public string SecondDataLabel { get; }

        public string FirstData { get; }

        public string SecondData { get; }

        public string CurrencyName { get; }
    }
}