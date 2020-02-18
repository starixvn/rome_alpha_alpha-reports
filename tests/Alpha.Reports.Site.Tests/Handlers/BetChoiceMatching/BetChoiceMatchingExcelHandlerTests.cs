namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using NSubstitute;

    public class BetChoiceMatchingExcelHandlerTests
    {
        private readonly BetChoiceMatchingService betChoiceMatchingService;
        private readonly BetChoiceMatchingWorkbook workbook;
        private readonly BetChoiceMatchingExcelHandler handler;

        public BetChoiceMatchingExcelHandlerTests()
        {
            betChoiceMatchingService = Substitute.For<BetChoiceMatchingService>();
            workbook = Substitute.For<BetChoiceMatchingWorkbook>();
            handler = new BetChoiceMatchingExcelHandler(betChoiceMatchingService, workbook);
        }
    }
}
