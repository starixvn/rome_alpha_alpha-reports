describe("WinLossDetail_", () => {
    const winLossDetailView = new WinLossDetailView($,
        new DateRangePickerComponent($),
        new PagerComponent($),
        new ProductMultiSelectionComponent($),
        new SortComponent($),
        new ReportTooltip($),
        new MonthPickerComponent($));

    winLossDetailView.init();
});