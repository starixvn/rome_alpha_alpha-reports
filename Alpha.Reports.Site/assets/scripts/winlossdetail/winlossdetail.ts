/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/netturnover-tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/productmultiselection.ts" />

"use strict";

declare var $: any;

class WinLossDetailView {
    domElements = {
        dateRangePicker: null,
        dropdownMonth: null,
        pager: null,
        dropdownProduct: null,

        longMonthsInYear: null,
        historicReportSwitch: null,
        currentReportSwitch: null,
        filter: null,

        iconFilter: null,
        pageTitle: null,
        iconSwitchMode: null,
        buttonSubmit: null,

        tableReport: null,

        isHistoricReport: null,
        downlineLink: null,
        dropdownDateRange: null,
        sortColumns: null,
        multiSelectButton: null,

        drilldownLinks: null,
        hideFilter: null,
        isStackMode: null,
        isDescendingSort: null,
        sortPropertyName: null,
        pageIndex: null,
        pageSize: null,
        fromDate: null,
        toDate: null,
        reportForm: null,
        userSelectedProductIds: null,
        userSelectedDateRangeOption: null,
    };

    isFirstSwitchMode = true;

    sortedColumn = "";
    descendingsort = true;

    isHistoricReport = false;

    slideAnimationTime = 200;

    init = () => {
        this.domElements.dateRangePicker = dateRangePickerComponent.render();

        this.renderDropdownProduct();
        this.cacheDOMElements();

        if (this.isHistoricReport) {
            this.domElements.dropdownMonth = monthPickerComponent.render({ longMonthsInYear: this.getLongMonthsInYear() });
        }

        this.domElements.pager = pagerComponent.render({ onSelectPage: this.onSubmitReportEvent });
        netTurnoverTooltipComponent.render();

        var tableReport = this.domElements.tableReport;
        tableReport.stickyTableHeaders();

        if (this.domElements.filter.data("ishide") === 1) {
            this.hideFilter();
        }

        this.renderSortedColumn();

        this.sortedColumn = tableReport.data("sortedcolumn");
        this.descendingsort = tableReport.data("descendingsort");

        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.domElements.longMonthsInYear = $("#longmonths-inyear");
        this.domElements.historicReportSwitch = $("#historicreport-switch");
        this.domElements.currentReportSwitch = $("#currentreport-switch");

        this.domElements.pageTitle = $(".page-title ");
        this.domElements.filter = $("#filter");
        this.domElements.iconFilter = $(".icon-filter");
        this.domElements.iconSwitchMode = $(".icon-switchmode");

        this.domElements.buttonSubmit = $(".btn-submit");

        this.domElements.tableReport = $("#tbl-report");

        this.domElements.isHistoricReport = $("#IsHistoricReport");
        this.isHistoricReport = this.domElements.isHistoricReport.val() === "True";

        this.domElements.dropdownDateRange = $("#dropdown-daterange");

        this.domElements.sortColumns = this.domElements.tableReport.find("th[data-sortedname]");

        this.domElements.drilldownLinks = $(".downline-link");
        this.domElements.hideFilter = $("#HideFilter");
        this.domElements.isStackMode = $("#IsStackMode");
        this.domElements.isDescendingSort = $("#IsDescendingSort");
        this.domElements.sortPropertyName = $("#SortPropertyName");
        this.domElements.pageIndex = $("#PageIndex");
        this.domElements.pageSize = $("#PageSize");
        this.domElements.fromDate = $("#FromDate");
        this.domElements.toDate = $("#ToDate");
        this.domElements.reportForm = $("#report-form");
        this.domElements.userSelectedDateRangeOption = $("#UserSelectedDateRangeOption");
        this.domElements.userSelectedProductIds = $("#UserSelectedProductIds");
    };

    getLongMonthsInYear = () => {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = $.parseJSON(this.domElements.longMonthsInYear.val());
        return longMonthsInYear;
    };

    renderDropdownProduct = () => {
        var $this = this;

        this.domElements.dropdownProduct = $("#dropdown-product");
        var dropdownProduct = this.domElements.dropdownProduct;
        var hasProduct = parseInt(dropdownProduct.data("total-product")) > 0;

        dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasProduct ?
                dropdownProduct.data("labelselectproducts")
                : dropdownProduct.data("labelthereisnoproduct"),
            nSelectedText: " " + dropdownProduct.data("labelselected"),
            allSelectedText: dropdownProduct.data("labelallproducts"),
            includeSelectAllOption: hasProduct,
            onChange: function (option: any, checked: boolean) {
                if ($this.domElements.multiSelectButton &&
                    $this.domElements.dropdownProduct.find("option:selected").length &&
                    $this.domElements.multiSelectButton.hasClass("error")) {
                    $this.domElements.multiSelectButton.removeClass("error");
                }
            },
            onSelectAll: function () {
                if ($this.domElements.multiSelectButton &&
                    $this.domElements.dropdownProduct.find("option:selected").length &&
                    $this.domElements.multiSelectButton.hasClass("error")) {
                    $this.domElements.multiSelectButton.removeClass("error");
                }
            },
            language: dropdownProduct.data("language")
        });

        this.domElements.multiSelectButton = $(".multiselect.btn");
    };

    registerEvents = () => {
        var $this = this;

        this.domElements.historicReportSwitch.on("click", function () {
            $this.isHistoricReport = true;
            $this.domElements.reportForm.trigger("submit");
        });

        this.domElements.currentReportSwitch.on("click", function () {
            $this.isHistoricReport = false;
            $this.domElements.reportForm.trigger("submit");
        });

        this.domElements.iconFilter.on("click", function () {
            var filter = $this.domElements.filter;
            var currentHideState = filter.is(":hidden");

            if (currentHideState) {
                $this.domElements.pageTitle.removeClass("hide-filter");
                filter.slideDown("fast");
            } else {
                $this.hideFilter();
            }

            $this.replaceDrilldownParam("HideFilter", !currentHideState);

            setTimeout(function () {
                netTurnoverTooltipComponent.positionNetTurnoverTooltip();
            }, $this.slideAnimationTime);
        });

        this.domElements.iconSwitchMode.on("click", function () {
            var tableReport = $this.domElements.tableReport;
            var currentStackModeState = tableReport.hasClass("stack-mode");

            if (currentStackModeState) {
                $this.domElements.pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            } else {
                $this.domElements.pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }

            $this.replaceDrilldownParam("IsStackMode", !currentStackModeState);

            if ($this.isFirstSwitchMode) {
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

                $this.isFirstSwitchMode = false;
            }
        });

        this.domElements.reportForm.submit(this.onSubmitReportEvent);
    };

    hideFilter = () => {
        this.domElements.pageTitle.addClass("hide-filter");
        this.domElements.filter.slideUp("fast");
    };
    getParams = () => {
        var tableReport = this.domElements.tableReport;

        var isHistoricReport = this.isHistoricReport;
        var dateRange = isHistoricReport
            ? {
                FromDate: this.domElements.dropdownMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker),
                ToDate: this.domElements.dropdownMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker)
            }
            : this.domElements.dateRangePicker.nexDatePicker("getDateRange");

        var selected = [];
        var selectedOptions = this.domElements.dropdownProduct.find("option:selected");
        selectedOptions.each(function () {
            selected.push([$(this).val()]);
        });

        return {
            CustId: tableReport.data("custid"),
            PageIndex: this.domElements.pager.data("pageindex"),
            PageSize: this.domElements.pager.data("pagesize"),
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            UserSelectedProductIds: selected.join(","),
            SortPropertyName: this.sortedColumn,
            IsDescendingSort: this.descendingsort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: isHistoricReport,
            HideFilter: this.domElements.filter.is(":hidden") ? true : false,
            UserSelectedDateRangeOption: this.domElements.dropdownDateRange.find("option:selected").val(),
            TimeStamp: new Date()
        };
    };
    getParamsSwitchView = () => {
        var tableReport = this.domElements.tableReport;

        var isHistoricReport = this.isHistoricReport;
        var dateRange = isHistoricReport
            ? {
                FromDate: null,
                ToDate: null
            }
            : this.domElements.dateRangePicker.nexDatePicker("getDateRange");

        var selected = [];
        var selectedOptions = this.domElements.dropdownProduct.find("option:selected");
        selectedOptions.each(function () {
            selected.push([$(this).val()]);
        });

        return {
            CustId: tableReport.data("custid"),
            PageIndex: this.domElements.pager.data("pageindex"),
            PageSize: this.domElements.pager.data("pagesize"),
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            UserSelectedProductIds: selected.join(","),
            SortPropertyName: this.sortedColumn,
            IsDescendingSort: this.descendingsort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: isHistoricReport,
            HideFilter: this.domElements.filter.is(":hidden") ? true : false,
            UserSelectedDateRangeOption: isHistoricReport ? this.domElements.dropdownDateRange.find("option:selected").val() : 0,
            TimeStamp: new Date()
        };
    };

    renderSortedColumn = () => {
        var $this = this;
        var tableReport = this.domElements.tableReport;

        if (parseInt(tableReport.data("totalrecords"), 0) <= 1) {
            this.domElements.sortColumns.removeAttr("data-sortedname");

            return;
        } else {
            this.domElements.sortColumns.on("click",
                function () {
                    var sortedColumn = $(this).data("sortedname");

                    if (sortedColumn !== $this.sortedColumn) {
                        $this.descendingsort = true;
                    } else {
                        $this.descendingsort = !$this.descendingsort;
                    }

                    $this.sortedColumn = sortedColumn;
                    $this.domElements.reportForm.trigger("submit");
                });
        }

        var sortedColumn = tableReport.data("sortedcolumn");
        var descendingsort = tableReport.data("descendingsort");

        var sortClass = descendingsort
            ? "icon-menu-down descSorting"
            : "icon-menu-up ascSorting";

        var columns = tableReport.find("th[data-sortedname='" + sortedColumn + "']");
        columns.each(function () {
            var column = $(this);
            var isCombinedColumn = column.hasClass("col-combine");

            if (isCombinedColumn) {
                column.find(".prior-sortedcolumn").append("<span class=\"" + sortClass + "\"></span>");
            } else {
                column.append("<span class=\"" + sortClass + "\"></span>");
            }
        });
    };

    showValidaProductError = () => {
        this.domElements.multiSelectButton.addClass("error");
    };
    clearError = () => {
        this.domElements.multiSelectButton.removeClass("error");
    };
    replaceDrilldownParam = (paramName, paramValue) => {
        this.domElements.drilldownLinks.each(function () {
            var queryString = $(this).attr("href");
            var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
            $(this).attr("href", newQueryString);
        });
    };

    onSubmitReportEvent = (e) => {
        $.blockUI();

        var params = winLossDetailView.getParams();
        if (params.UserSelectedProductIds === "") {
            winLossDetailView.showValidaProductError();
            $.unblockUI();
            e.preventDefault();

            return;
        }

        this.domElements.hideFilter.val(params.HideFilter);
        this.domElements.isDescendingSort.val(params.IsDescendingSort);
        this.domElements.isHistoricReport.val(params.IsHistoricReport);
        this.domElements.isStackMode.val(params.IsStackMode);
        this.domElements.sortPropertyName.val(params.SortPropertyName);
        this.domElements.pageIndex.val(params.PageIndex);
        this.domElements.pageSize.val(params.PageSize);
        this.domElements.fromDate.val(params.FromDate);
        this.domElements.toDate.val(params.ToDate);
        this.domElements.userSelectedDateRangeOption.val(params.UserSelectedDateRangeOption);
        this.domElements.userSelectedProductIds.val(params.UserSelectedProductIds);

        if (alphaReports.screenSmallThan768()) {
            this.domElements.pageTitle.addClass("hide-filter");
            this.domElements.filter.slideUp("fast");
        }
    }
};

const winLossDetailView = new WinLossDetailView();

$(function () {
    winLossDetailView.init();
});
