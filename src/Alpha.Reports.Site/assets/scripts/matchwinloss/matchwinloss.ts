/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class MatchWinLossView {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    $tableReport: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheElements();

        this.dateRangePickerComponent.render();

        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            true,
            this.sortComponent.afterSort,
            { 0: { sorter: "text" }, 1: { sorter: "text" } },
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();
        this.registerEvents();
        this.$.unblockUI();
    };
    cacheElements = () => {
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
    };
    registerEvents = () => {
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
}

$(() => {
    const matchWinLossView = new MatchWinLossView(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, new LinkBuilderComponent($)));

    matchWinLossView.init();
});
































