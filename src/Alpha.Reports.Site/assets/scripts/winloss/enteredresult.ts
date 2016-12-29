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
        let self = this;

        this.dateRangePickerComponent.render();
        this.cacheDOMElements();

        this.sortComponent.render({
            sortContainer: this.$tableReport,
            canSort: parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            hasAllDataOnClient: true,
            headers: {
                0: { sorter: "text" }
            },
            afterSortFunction: function () {}
        });

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$isFilterVisible = this.$("#IsFilterVisible");
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
        let filter = this.$reportForm;
        let currentHideState = filter.is(":hidden");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            filter.slideDown("fast", undefined, function () { });
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.slideUp("fast", undefined, function () { });
        }
    };
    onExportExcelEvent = () => {
        this.$("#export-excel-form").submit();
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
    bindTableSorter = () => {
        let self = this;

        let headers = {
            0: { sorter: "text" }
        };

        let disabledSortColumns = self.$tableReport.find("th[data-disabledsort='true']");
        disabledSortColumns.each(function () {
            headers[this.cellIndex] = { sorter: false };
        });

        let sortedColumnHeader = self.$tableReport.find("[data-sortedname='" + self.$tableReport.data("sortedcolumn") + "']")[0];

        this.$tableReport.tablesorter(
            {
                defaultParser: 1,
                headers: headers,
                sortList: [[self.$(sortedColumnHeader).data("colindex"), 1]],
                textExtraction: function () {}
            });
    };
}

const enterResultView = new EnterResultView(
    $,
    new DateRangePickerComponent($),
    new SortComponent($));

$(function () {
    enterResultView.init();
});
