/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class WinLossView {
    $: any;
    $pageTitle: any;
    $tableReport: any;

    $isHistoricReport: any;
    $multiSelectButton: any;

    $drilldownLinks: any;
    $isFilterVisible: any;
    $isStackMode: any;
    $isAgentLevel: any;
    $reportForm: any;
    $userSelectedDateRangeOption: any;
    $reportSwitch: any;
    $exportExcelForm: any;

    isFirstSwitchMode = true;
    isHistoricReport = false;
    isSwitchView = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    monthPickerComponent: MonthPickerComponent;

    constructor(
        $: any,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        monthPickerComponent: MonthPickerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.monthPickerComponent = monthPickerComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === "true";
        this.isHistoricReport
            ? this.monthPickerComponent.render()
            : this.dateRangePickerComponent.render();
        this.productMultiSelectionComponent.render();
        this.cacheDOMElements();

        if (this.$isFilterVisible.val().toString().toLowerCase() === "false") {
            this.hideFilter();
        }

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
        this.$drilldownLinks = this.$(".downline-link");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isAgentLevel = this.$("#IsAgentLevel");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$multiSelectButton = this.$(".multiselect.btn");
        this.$reportSwitch = this.$("#report-switch");
    };
    onFilterEvent = () => {
        var filter = this.$reportForm;
        var currentHideState = filter.is(":hidden");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            filter.slideDown("fast", undefined, function () { });
        } else {
            this.hideFilter();
        }

        this.replaceDrilldownParam("IsFilterVisible", !currentHideState);
    };
    onExportExcelEvent = () => {
        this.$("#export-excel-form").submit();
        this.$.unblockUI();
    };
    onReportSwitchEvent = () => {
        this.isSwitchView = true;
        this.isHistoricReport = !this.$reportSwitch.data("is-historic-report");
        this.$reportForm.trigger("submit");
    };
    onStackModeEvent = () => {

    };
    registerEvents = () => {
        var self = this;

        this.$reportSwitch.on("click", this.onReportSwitchEvent);
        this.$(".icon-filter").on("click", this.onFilterEvent);
        this.$("#icon-excel").on("click", this.onExportExcelEvent);
        this.$(".icon-switchmode").on("click", function () {
            var tableReport = self.$tableReport;
            var currentStackModeState = tableReport.hasClass("stack-mode");

            if (currentStackModeState) {
                self.$pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            } else {
                self.$pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }

            self.replaceDrilldownParam("IsStackMode", !currentStackModeState);

            if (self.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-turnover");

                    self.$.each(self.levels, function (index: number) {
                        var level = self.levels[index];
                        alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                    });
                } else {
                    if (self.$isAgentLevel.val().toLocaleLowerCase() === "true") {
                        alphaReports.combineConsecutiveCells(self.$("td.col-turnover"), 2, "text-right col-turnover-combined");
                    }
                    else {
                        alphaReports.combineConsecutiveCells(self.$("td.col-turnover"), 3, "text-right col-turnover-combined");
                    }

                    self.$.each(self.levels, function (index: number) {
                        var level = self.levels[index];
                        var cellCss = "text-right col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                        alphaReports.combineConsecutiveCells(self.$("td.col-" + level + "-winloss"), 3, cellCss);
                    });
                }

                self.isFirstSwitchMode = false;
            }

            self.sortComponent.bindTableSorter(self.getSortedColumnName());
        });

        self.$reportForm.submit(this.onSubmitReportEvent);
    };
    hideFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.slideUp("fast", undefined, function () {});
    };
    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        var sortInfo = this.sortComponent.getSortInfo();
        var tableReport = this.$tableReport;

        return {
            CustId: tableReport.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            HideFilter: this.$reportForm.is(":hidden"),
        };
    };
    showValidaProductError = () => {
        this.$multiSelectButton.addClass("error");
    };
    clearError = () => {
        this.$multiSelectButton.removeClass("error");
    };
    replaceDrilldownParam = (paramName: string, paramValue: any) => {
        var self = this;
        this.$drilldownLinks.each(function () {
            var queryString = self.$(this).attr("href");
            var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
            self.$(this).attr("href", newQueryString);
        });
    };
    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidaProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? params.HideFilter : true );
        this.$isHistoricReport.val(params.IsHistoricReport);
        this.$isStackMode.val(params.IsStackMode);
    };
    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "GrossComm") {
            return "Turnover";
        }

        if (sortedColumn === "MemberNetWinLoss.Comm" || sortedColumn === "MemberNetWinLoss.Total") {
            return "MemberNetWinLoss.WinLoss";
        }

        if (sortedColumn === "AgentNetWinLoss.Comm" || sortedColumn === "AgentNetWinLoss.Total") {
            return "AgentNetWinLoss.WinLoss";
        }

        if (sortedColumn === "MasterNetWinLoss.Comm" || sortedColumn === "MasterNetWinLoss.Total") {
            return "MasterNetWinLoss.WinLoss";
        }

        if (sortedColumn === "SuperNetWinLoss.Comm" || sortedColumn === "SuperNetWinLoss.Total") {
            return "SuperNetWinLoss.WinLoss";
        }

        return sortedColumn;
    };
}

const winLossView = new WinLossView(
    $,
    new DateRangePickerComponent($),
    new ProductMultiSelectionComponent($),
    new SortComponent($),
    new MonthPickerComponent($));

$(function () {
    winLossView.init();
});
