namespace Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models
{
    public class LogAction
    {
        public int Id { get; set; }

        public int ActionTypeId { get; set; }

        public string ChineseTraditionalName { get; set; }

        public string ChineseSimplifiedName { get; set; }

        public string EnglishName { get; set; }

        public string VietnameseName { get; set; }

        public string JapaneseName { get; set; }

        public string KoreanName { get; set; }

        public string ThaiName { get; set; }

        public bool IsInternal { get; set; }

        public string SPName { get; set; }
    }
}
