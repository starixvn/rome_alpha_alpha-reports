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
        var self = this;

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
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportSwitch = this.$("#report-switch");
    };
    registerEvents = () => {
        var self = this;

        this.$reportSwitch.on("click", function () {
            self.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
            self.$reportForm.trigger("submit");
        });

        this.$(".icon-filter").on("click", function () {
            var filter = self.$reportForm;
            var currentHideState = filter.is(":hidden");

            if (currentHideState) {
                self.$pageTitle.removeClass("hide-filter");
                filter.slideDown("fast", undefined, function () { });
            } else {
                self.hideFilter();
            }
        });

        this.$("#icon-excel").on("click", function () {
            self.$("#export-excel-form").submit();
            self.$.unblockUI();
        });

        this.$(".btn-submit").on("click", function () {
            self.$("#IsHistoricReport").val(false);
            self.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
            self.onSubmitReportEvent();
        });
    };
    hideFilter = () => {
        var self = this;

        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.slideUp("fast", undefined, function () { });
    };
    onSubmitReportEvent = () => {
        this.$.blockUI();
    };
    bindTableSorter = () => {
        var self = this;

        var headers = {
            0: { sorter: "text" }
        };

        var disabledSortColumns = self.$tableReport.find("th[data-disabledsort='true']");
        disabledSortColumns.each(function () {
            headers[this.cellIndex] = { sorter: false };
        });

        var sortedColumnHeader = self.$tableReport.find("[data-sortedname='" + self.$tableReport.data("sortedcolumn") + "']")[0];

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
