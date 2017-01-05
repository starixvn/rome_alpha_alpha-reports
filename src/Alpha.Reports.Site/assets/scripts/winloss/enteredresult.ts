/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class EnterResultView {
    $: any;
    $pageTitle: any;
    $tableReport: any;
    $reportForm: any;
    $isFilterVisible: any;
    $userSelectedDateRangeOption: any;
    $reportSwitch: any;
    $isStackMode: any;
    isSwitchView = false;
    $exportExcelForm: any;

    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        $: any,
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
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isStackMode = this.$("#IsStackMode");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportSwitch = this.$("#report-switch");
    };
    registerEvents = () => {
        this.$reportSwitch.on("click", this.onSwitchModeEvent);
        this.$(".icon-filter").on("click", this.onFilterEvent);
        this.$("#icon-excel").on("click", this.onExportExcelEvent);
        this.$(".btn-submit").on("click", this.onSubmitReportEvent);
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
    onSubmitReportEvent = () => {
        this.$("#IsHistoricReport").val(false);
        this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
        this.$isStackMode.val(alphaReports.screenSmallThan768());
        this.$.blockUI();
    };
    onSwitchModeEvent = () => {
        this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
        this.$isStackMode.val(alphaReports.screenSmallThan768());
        this.$reportForm.trigger("submit");
    };
}

const enterResultView = new EnterResultView(
    $,
    new DateRangePickerComponent($),
    new SortComponent($));

$(function () {
    enterResultView.init();
});
