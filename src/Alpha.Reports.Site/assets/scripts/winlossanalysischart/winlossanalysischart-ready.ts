declare let Chart: any;
$(document).ready(() => {
    const winLossAnalysisChartView = new WinLossAnalysisChartView(
        $,
        new MonthRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new LinkBuilderComponent($),
        new DualAxisChartComponent($, Chart));

    winLossAnalysisChartView.init();
});