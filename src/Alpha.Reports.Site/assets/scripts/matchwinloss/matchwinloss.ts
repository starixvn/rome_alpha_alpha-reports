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
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;

    $isFilterVisible: JQuery;
    $pageTitle: JQuery;

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

        let canSort = this.$tableReport.data("cansort");
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort && canSort.toLowerCase() === "true",
            true,
            this.sortComponent.afterSort,
            { 0: { sorter: "text" }, 1: { sorter: "text" } },
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();
        this.registerEvents();
        this.$.unblockUI();
    };
    cacheElements = () => {
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");

        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
    };
    registerEvents = () => {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    onSubmitReport = () => {
        let this$ = this.$;
        this$.blockUI();

        let sortInfo = this.sortComponent.getSortInfo();
        this.$isDescendingSort.val(String(sortInfo.isDescendingSort));
        this.$sortPropertyName.val(sortInfo.sortPropertyName);
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !this.$reportForm.is(":hidden")));
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    };
}

$(() => {
    const matchWinLossView = new MatchWinLossView(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, new LinkBuilderComponent($)));

    matchWinLossView.init();
});
































