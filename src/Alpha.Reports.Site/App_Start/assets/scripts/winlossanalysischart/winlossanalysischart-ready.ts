/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />

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