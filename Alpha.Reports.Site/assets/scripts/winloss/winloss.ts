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
    $hideFilter: any;
    $isStackMode: any;
    $isDescendingSort: any;
    $sortPropertyName: any;
    $isAgentLevel: any;
    $reportForm: any;
    $userSelectedDateRangeOption: any;
    $reportSwitch: any;

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
        var self = this;

        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === "true";
        this.isHistoricReport
            ? this.monthPickerComponent.render()
            : this.dateRangePickerComponent.render();
        this.productMultiSelectionComponent.render();
        this.cacheDOMElements();

        if (this.$hideFilter.val() === "True") {
            this.hideFilter();
        }

        this.sortComponent.render({
            sortContainer: this.$tableReport,
            canSort: parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            hasAllDataOnClient: true,
            headers: {
                0: { sorter: "text" }
            },
            afterSortFunction: function () {
                self.replaceDrilldownParam("SortPropertyName", self.$tableReport.data("sortedcolumn"));
                self.replaceDrilldownParam("IsDescendingSort", self.$tableReport.data("descendingsort"));
            }
        });

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$drilldownLinks = this.$(".downline-link");
        this.$hideFilter = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isAgentLevel = this.$("#IsAgentLevel");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$multiSelectButton = this.$(".multiselect.btn");
        this.$reportSwitch = this.$("#report-switch");
    };
    registerEvents = () => {
        var self = this;

        this.$reportSwitch.on("click", function () {
            self.isSwitchView = true;
            self.isHistoricReport = !self.$reportSwitch.data("is-historic-report");
            self.$reportForm.trigger("submit");
        });

        this.$(".icon-filter").on("click", function () {
            var filter = self.$reportForm;
            var currentHideState = filter.is(":hidden");

            if (currentHideState) {
                self.$pageTitle.removeClass("hide-filter");
                filter.slideDown("fast", undefined, function () {});
            } else {
                self.hideFilter();
            }

            self.replaceDrilldownParam("IsFilterVisible", !currentHideState);
        });

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

            this.sortComponent.bindTableSorter(this.getSortedColumnName());
        });

        this.$reportForm.submit(this.onSubmitReportEvent);

        this.$("#icon-excel").on("click", function () {
            self.$reportForm.attr("action", "WinLoss/WinLossExcel");
            self.$reportForm.submit();

            self.$reportForm.attr("action", null);
            self.$.unblockUI();
        });

        this.$(".icon-back").on("click", function () {
            window.history.back();
        });
    };
    hideFilter = () => {
        var self = this;

        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.slideUp("fast", undefined, function () {});
    };
    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        var sortInfo = this.sortComponent.getSortInfo();
        var tableReport = this.$tableReport;

        return {
            CustId: tableReport.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
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

        this.$hideFilter.val(params.HideFilter);
        this.$isDescendingSort.val(params.IsDescendingSort);
        this.$isHistoricReport.val(params.IsHistoricReport);
        this.$isStackMode.val(params.IsStackMode);
        this.$sortPropertyName.val(params.SortPropertyName);

        if (alphaReports.screenSmallThan768()) {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.slideUp("fast");
        }
    };
    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "NetTurnover" || sortedColumn === "GrossComm") {
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
