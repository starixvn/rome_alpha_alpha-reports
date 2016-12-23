/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/netturnover-tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />
"use strict";
var WinLossDetailView = (function () {
    function WinLossDetailView($, dateRangePickerComponent, pageComponent, productMultiSelectionComponent, sortComponent, reportTooltip, monthPickerComponent) {
        var _this = this;
        this.isFirstSwitchMode = true;
        this.isHistoricReport = false;
        this.isSwitchView = false;
        this.hasAllDataOnClient = false;
        this.levels = ["member", "agent", "master", "super"];
        this.init = function () {
            _this.$.blockUI();
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === "true";
            _this.isHistoricReport
                ? _this.monthPickerComponent.render()
                : _this.dateRangePickerComponent.render();
            _this.productMultiSelectionComponent.render();
            _this.cacheElements();
            _this.$netTurnoverTooltip = netTurnoverTooltipComponent.render();
            _this.pagerComponent.render();
            _this.hasAllDataOnClient = _this.pagerComponent.hasAllDataOnClient();
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render({
                sortContainer: _this.$tableReport,
                canSort: canSort,
                hasAllDataOnClient: _this.hasAllDataOnClient,
                headers: {
                    0: { sorter: "text" }
                },
                afterSortFunction: _this.afterSort
            });
            _this.$tableReport.stickyTableHeaders();
            if (_this.$hasSubmittedParams.val().toString().toLowerCase() !== "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.registerEvents();
            _this.$.unblockUI();
        };
        this.cacheElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$drilldownLinks = _this.$(".downline-link");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
            _this.$reportForm = _this.$("#report-form");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.$reportSwitch = _this.$("#report-switch");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$iconNormalColumn = _this.$("#icon-normal-column");
            _this.$hasSubmittedParams = _this.$("#HasSubmittedParams");
            _this.$breadcrumbLinks = _this.$(".breadcrumb-link");
        };
        this.registerEvents = function () {
            _this.$reportSwitch.on("click", _this.onSwitchReport);
            _this.$(".icon-filter").on("click", _this.onClickIconFilter);
            _this.$(".icon-switchmode").on("click", _this.onSwitchMode);
            _this.$reportForm.submit(_this.onSubmitReport);
            _this.$("#icon-excel").on("click", _this.onExportExcel);
            _this.$(".icon-back").on("click", _this.onClickBackIcon);
        };
        this.onSwitchReport = function () {
            _this.isSwitchView = true;
            _this.isHistoricReport = !_this.$reportSwitch.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            var $netTurnoverTooltip = _this.$netTurnoverTooltip;
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
                netTurnoverTooltipComponent.positionNetTurnoverTooltip($netTurnoverTooltip);
            }
            else {
                _this.hideFilter();
            }
            _this.replaceHrefParam("IsFilterVisible", currentHideState);
        };
        this.setStackMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            _this.$pageTitle.addClass("stack-mode");
            tableReport.addClass("stack-mode");
            _this.replaceHrefParam("IsStackMode", true);
            var this$ = _this.$;
            alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");
            var levels = _this.levels;
            this$.each(levels, function (index) {
                var level = levels[index];
                var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
            });
            _this.isFirstSwitchMode = false;
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
            _this.$.unblockUI();
        };
        this.onSwitchMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            var currentStackModeState = tableReport.hasClass("stack-mode");
            if (currentStackModeState) {
                _this.$pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            }
            else {
                _this.$pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }
            _this.replaceHrefParam("IsStackMode", !currentStackModeState);
            var levels = _this.levels;
            var this$ = _this.$;
            if (_this.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-turnover");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                    });
                }
                else {
                    alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                        alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
                    });
                }
                _this.isFirstSwitchMode = false;
            }
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
            _this.$.unblockUI();
        };
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onClickBackIcon = function () {
            window.history.back();
        };
        this.afterSort = function () {
            var this$ = _this.$;
            if (_this.hasAllDataOnClient) {
                var sortedColumn = this$(".sorting-asc");
                var direction = !sortedColumn.length;
                if (sortedColumn.length === 0) {
                    var sortedColumn = this$(".sorting-desc");
                }
                _this.replaceHrefParam("SortPropertyName", sortedColumn.data("sortedname"));
                _this.replaceHrefParam("IsDescendingSort", direction);
                var isEvenRow = false;
                _this.$tableReport.find("tbody > tr").each(function () {
                    var $row = this$(this);
                    if ($row.hasClass("even-row") && !isEvenRow) {
                        $row.removeClass("even-row");
                    }
                    else if (!$row.hasClass("even-row") && isEvenRow) {
                        $row.addClass("even-row");
                    }
                    isEvenRow = !isEvenRow;
                });
            }
            else {
                _this.$reportForm.trigger("submit");
            }
        };
        this.hideFilter = function () {
            var $netTurnoverTooltip = _this.$netTurnoverTooltip;
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
            netTurnoverTooltipComponent.positionNetTurnoverTooltip($netTurnoverTooltip);
        };
        this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass("error");
        };
        this.clearError = function () {
            _this.$multiSelectButton.removeClass("error");
        };
        this.replaceHrefParam = function (paramName, paramValue) {
            var this$ = _this.$;
            _this.$drilldownLinks.each(function () {
                var queryString = this$(this).attr("href");
                var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
                this$(this).attr("href", newQueryString);
            });
            _this.$breadcrumbLinks.each(function () {
                var queryString = this$(this).attr("href");
                var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
                this$(this).attr("href", newQueryString);
            });
        };
        this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (_this.isSwitchView) {
                _this.$fromDate.val("");
                _this.$toDate.val("");
            }
            else if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible);
            _this.$isDescendingSort.val(params.IsDescendingSort);
            _this.$isHistoricReport.val(params.IsHistoricReport);
            _this.$isStackMode.val(params.IsStackMode);
            _this.$sortPropertyName.val(params.SortPropertyName);
        };
        this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
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
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pageComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
    }
    return WinLossDetailView;
}());
$(function () {
    var winLossDetailView = new WinLossDetailView($, new DateRangePickerComponent($), new PagerComponent($), new ProductMultiSelectionComponent($), new SortComponent($), new ReportTooltip($), new MonthPickerComponent($));
    winLossDetailView.init();
});
//# sourceMappingURL=winlossdetail.js.map