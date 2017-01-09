/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class EnterResultView {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;
    $reportForm: JQuery;
    $isFilterVisible: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $historicReport: JQuery;
    $isHistoricReport: JQuery;
    $isStackMode: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();
        this.dateRangePickerComponent.render();
        this.cacheDOMElements();

        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            true,
            function () { },
            { 0: { sorter: "text" } }));

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#IsHistoricReport").length > 0;
    };
    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isStackMode = this.$("#IsStackMode");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$submitButton = this.$(".btn-submit");
    };
    registerEvents = () => {
        this.$historicReport.on("click", this.onSubmitHistoricReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$submitButton.on("click", this.onSubmitNormalReportEvent);
    };
    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass("hide");
        currentHideState ? this.showFilter() : this.hideFilter();
    };
    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };
    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };
    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    onSubmitNormalReportEvent = () => {
        this.setHiddenValue(false);
    };
    onSubmitHistoricReportEvent = () => {
        this.setHiddenValue(true);
        this.$reportForm.trigger("submit");
    };
    setHiddenValue = (isHistoryReport: boolean) => {
        this.$isHistoricReport.val(String(isHistoryReport));
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
        this.$isStackMode.val(String(alphaReports.screenSmallThan768()));
        this.$.blockUI();
    };
}

const enterResultView = new EnterResultView(
    $,
    new DateRangePickerComponent($),
    new SortComponent($));

$(function () {
    enterResultView.init();
});