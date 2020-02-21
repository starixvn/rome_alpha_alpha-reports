namespace Alpha.Reports.Site.Tests.Handlers.BetChoiceMatching
{
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Handlers.BetChoiceMatching;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BetChoiceMatching")]
    public class BetChoiceMatchingWorkbookTests
    {
        private readonly IBetListExcelBuilder excelBuilder;
        private readonly ITicketHelper ticketHelper;
        private readonly CacheRefService cacheRefService;
        private readonly BetChoiceMatchingWorkbook workbook;

        public BetChoiceMatchingWorkbookTests()
        {
            excelBuilder = Substitute.For<IBetListExcelBuilder>();
            ticketHelper = Substitute.For<ITicketHelper>();
            cacheRefService = Substitute.For<CacheRefService>();
            workbook = new BetChoiceMatchingWorkbookImpl(excelBuilder, ticketHelper, cacheRefService);
        }

        public void aa()
        {
            // Arrange

            // Act
            // workbook.SetData

            // Assert
        }
    }
}
