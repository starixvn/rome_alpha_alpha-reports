describe("WinLoss_", () => {
    const winLossView = new WinLossView($,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SortComponent($),
        new MonthPickerComponent($));

    winLossView.init();
});