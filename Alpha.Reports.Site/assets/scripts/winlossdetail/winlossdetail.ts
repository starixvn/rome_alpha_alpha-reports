/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/netturnover-tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/productmultiselection.ts" />

"use strict";

class WinLossDetailView {
    $: any;

    $dateRangePicker: any;
    $dropdownMonth: any;
    $pager: any;
    $dropdownProduct: any;

    $pageTitle: any;
    $tableReport: any;

    $isHistoricReport: any;
    $sortColumns: any;
    $multiSelectButton: any;

    $drilldownLinks: any;
    $hideFilter: any;
    $isStackMode: any;
    $isDescendingSort: any;
    $sortPropertyName: any;
    $pageIndex: any;
    $pageSize: any;
    $fromDate: any;
    $toDate: any;
    $reportForm: any;
    $userSelectedProductIds: any;
    $userSelectedDateRangeOption: any;
    $netTurnoverTooltip: any;

    isFirstSwitchMode = true;
    sortedColumn = "";
    descendingsort = true;
    isHistoricReport = false;
    isSwitchView = false;

    constructor($: any) {
        this.$ = $;
    }

    init = () => {
        var self = this;

        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === "true";

        if (this.isHistoricReport) {
            this.$dropdownMonth = monthPickerComponent.render({ longMonthsInYear: this.getLongMonthsInYear() });
        } else {
            this.$dateRangePicker = dateRangePickerComponent.render();
        }

        this.$dropdownProduct = productMultiSelectionComponent.render();

        this.cacheDOMElements();

        if (this.$reportForm.data("ishide") === 1) {
            this.hideFilter();
        }

        this.$netTurnoverTooltip = netTurnoverTooltipComponent.render();
        this.$pager = pagerComponent.render({
            onSelectPage: function () {
                self.$reportForm.trigger("submit");
            }
        });

        var tableReport = this.$tableReport;
        this.renderSortedColumn();
        this.sortedColumn = tableReport.data("sortedcolumn");
        this.descendingsort = tableReport.data("descendingsort");

        tableReport.stickyTableHeaders();

        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");

        this.$tableReport = this.$("#tbl-report");

        this.$sortColumns = this.$tableReport.find("th[data-sortedname]");

        this.$drilldownLinks = this.$(".downline-link");
        this.$hideFilter = this.$("#HideFilter");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$pageIndex = this.$("#PageIndex");
        this.$pageSize = this.$("#PageSize");
        this.$fromDate = this.$("#FromDate");
        this.$toDate = this.$("#ToDate");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$userSelectedProductIds = this.$("#UserSelectedProductIds");
        this.$multiSelectButton = this.$(".multiselect.btn");
    };

    getLongMonthsInYear = () => {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = this.$.parseJSON(this.$("#longmonths-inyear").val());
        return longMonthsInYear;
    };

    registerEvents = () => {
        var self = this;

        this.$("#historicreport-switch").on("click", function () {
            self.isSwitchView = true;
            self.isHistoricReport = true;
            self.$reportForm.trigger("submit");
        });

        this.$("#currentreport-switch").on("click", function () {
            self.isSwitchView = true;
            self.isHistoricReport = false;
            self.$reportForm.trigger("submit");
        });

        this.$(".icon-filter").on("click", function () {
            var filter = self.$reportForm;
            var currentHideState = filter.is(":hidden");

            if (currentHideState) {
                self.$pageTitle.removeClass("hide-filter");
                filter.slideDown("fast", undefined, function () {
                    netTurnoverTooltipComponent.positionNetTurnoverTooltip(self.$netTurnoverTooltip);
                });
            } else {
                self.hideFilter();
            }

            self.replaceDrilldownParam("HideFilter", !currentHideState);
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
                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-turnover",
                        "text-right hidden-stack-mode col-netturnover",
                        "text-right hidden-stack-mode col-grosscomm"],
                        ".col-turnover-combined");

                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-memberwinloss",
                        "text-right hidden-stack-mode col-membercomm",
                        "text-right bold hidden-stack-mode col-membertotal"],
                        ".col-memberwinloss-combined");

                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-agentwinloss",
                        "text-right hidden-stack-mode col-agentcomm",
                        "text-right bold hidden-stack-mode col-agenttotal"],
                        ".col-agentwinloss-combined");

                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-masterwinloss",
                        "text-right hidden-stack-mode col-mastercomm",
                        "text-right bold hidden-stack-mode col-mastertotal"],
                        ".col-masterwinloss-combined");

                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-superwinloss",
                        "text-right hidden-stack-mode col-supercomm",
                        "text-right bold hidden-stack-mode col-supertotal"],
                        ".col-superwinloss-combined");
                } else {
                    alphaReports.combineColumns([
                        "td.col-turnover",
                        "td.col-netturnover",
                        "td.col-grosscomm"],
                        "text-right visible-stack-mode combinedcolumn col-turnover-combined", 1);
                    alphaReports.combineColumns([
                        "td.col-memberwinloss",
                        "td.col-membercomm",
                        "td.col-membertotal"],
                        "text-right visible-stack-mode combinedcolumn column-memberwinloss-combined", 3);
                    alphaReports.combineColumns([
                        "td.col-agentwinloss",
                        "td.col-agentcomm",
                        "td.col-agenttotal"],
                        "text-right visible-stack-mode combinedcolumn column-agentwinloss-combined altercol", 3);
                    alphaReports.combineColumns([
                        "td.col-masterwinloss",
                        "td.col-mastercomm",
                        "td.col-mastertotal"],
                        "text-right visible-stack-mode combinedcolumn column-masterwinloss-combined", 3);
                    alphaReports.combineColumns([
                        "td.col-superwinloss",
                        "td.col-supercomm",
                        "td.col-supertotal"],
                        "text-right visible-stack-mode combinedcolumn column-superwinloss-combined altercol", 3);
                }

                self.isFirstSwitchMode = false;
            }
        });

        this.$reportForm.submit(this.onSubmitReportEvent);
    };

    hideFilter = () => {
        var self = this;

        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.slideUp("fast", undefined, function () {
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(self.$netTurnoverTooltip);
        });
    };
    getParams = () => {
        var dateRange = this.getDateRangeParam();
        var selectedProducts = productMultiSelectionComponent.getSelectedItems(this.$dropdownProduct);

        var tableReport = this.$tableReport;

        return {
            CustId: tableReport.data("custid"),
            PageIndex: this.$pager.data("pageindex"),
            PageSize: this.$pager.data("pagesize"),
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            UserSelectedProductIds: selectedProducts.join(","),
            SortPropertyName: this.sortedColumn,
            IsDescendingSort: this.descendingsort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            HideFilter: this.$reportForm.is(":hidden"),
            UserSelectedDateRangeOption: this.$("#dropdown-daterange").find("option:selected").val(),
            TimeStamp: new Date()
        };
    };
    getDateRangeParam = () => {
        if (this.isSwitchView) {
            // switch to historic report, we can't use date range of date range picker

            return this.isHistoricReport
                ? { FromDate: null, ToDate: null }
                : monthPickerComponent.getDateRange(this.$dropdownMonth);
        }

        return this.isHistoricReport
            ? monthPickerComponent.getDateRange(this.$dropdownMonth)
            : this.$dateRangePicker.nexDatePicker("getDateRange");
    };
    renderSortedColumn = () => {
        var self = this;
        var tableReport = this.$tableReport;

        if (parseInt(tableReport.data("totalrecords"), 0) <= 1) {
            this.$sortColumns.removeAttr("data-sortedname");

            return;
        } else {
            this.$sortColumns.on("click",
                function () {
                    var sortedColumn = self.$(this).data("sortedname");

                    if (sortedColumn !== self.sortedColumn) {
                        self.descendingsort = true;
                    } else {
                        self.descendingsort = !self.descendingsort;
                    }

                    self.sortedColumn = sortedColumn;
                    self.$reportForm.trigger("submit");
                });
        }

        var sortedColumn = tableReport.data("sortedcolumn");
        var descendingsort = tableReport.data("descendingsort");

        var sortClass = descendingsort
            ? "icon-menu-down descSorting"
            : "icon-menu-up ascSorting";

        var columns = tableReport.find("th[data-sortedname='" + sortedColumn + "']");
        columns.each(function () {
            var column = self.$(this);
            var isCombinedColumn = column.hasClass("col-combine");

            if (isCombinedColumn) {
                column.find(".prior-sortedcolumn").append("<span class=\"" + sortClass + "\"></span>");
            } else {
                column.append("<span class=\"" + sortClass + "\"></span>");
            }
        });
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
        if (params.UserSelectedProductIds === "") {
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
        this.$pageIndex.val(params.PageIndex);
        this.$pageSize.val(params.PageSize);
        this.$fromDate.val(params.FromDate);
        this.$toDate.val(params.ToDate);
        this.$userSelectedDateRangeOption.val(params.UserSelectedDateRangeOption);
        this.$userSelectedProductIds.val(params.UserSelectedProductIds);

        if (alphaReports.screenSmallThan768()) {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.slideUp("fast");
        }
    };
}

const winLossDetailView = new WinLossDetailView($);

$(function () {
    winLossDetailView.init();
});
