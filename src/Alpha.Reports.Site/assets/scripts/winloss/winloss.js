/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />
"use strict";
var WinLossView = (function () {
    function WinLossView($, dateRangePickerComponent, productMultiSelectionComponent, sortComponent, monthPickerComponent) {
        var _this = this;
        this.isFirstSwitchMode = true;
        this.isHistoricReport = false;
        this.isSwitchView = false;
        this.levels = ["member", "agent", "master", "super"];
        this.init = function () {
            var self = _this;
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === "true";
            _this.isHistoricReport
                ? _this.monthPickerComponent.render()
                : _this.dateRangePickerComponent.render();
            _this.productMultiSelectionComponent.render();
            _this.cacheDOMElements();
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
            }
            _this.sortComponent.render({
                sortContainer: _this.$tableReport,
                canSort: parseInt(_this.$tableReport.data("totalrecords"), 0) > 1,
                hasAllDataOnClient: true,
                headers: {
                    0: { sorter: "text" }
                },
                afterSortFunction: function () { }
            });
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
        };
        this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$drilldownLinks = _this.$(".downline-link");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$isAgentLevel = _this.$("#IsAgentLevel");
            _this.$reportForm = _this.$("#report-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$multiSelectButton = _this.$(".multiselect.btn");
            _this.$reportSwitch = _this.$("#report-switch");
        };
        this.registerEvents = function () {
            var self = _this;
            _this.$reportSwitch.on("click", function () {
                self.isSwitchView = true;
                self.isHistoricReport = !self.$reportSwitch.data("is-historic-report");
                self.$reportForm.trigger("submit");
            });
            _this.$(".icon-filter").on("click", function () {
                var filter = self.$reportForm;
                var currentHideState = filter.is(":hidden");
                if (currentHideState) {
                    self.$pageTitle.removeClass("hide-filter");
                    filter.slideDown("fast", undefined, function () { });
                }
                else {
                    self.hideFilter();
                }
                self.replaceDrilldownParam("IsFilterVisible", !currentHideState);
            });
            _this.$("#icon-excel").on("click", function () {
                self.$("#export-excel-form").submit();
                self.$.unblockUI();
            });
            _this.$(".icon-switchmode").on("click", function () {
                var tableReport = self.$tableReport;
                var currentStackModeState = tableReport.hasClass("stack-mode");
                if (currentStackModeState) {
                    self.$pageTitle.removeClass("stack-mode");
                    tableReport.removeClass("stack-mode");
                }
                else {
                    self.$pageTitle.addClass("stack-mode");
                    tableReport.addClass("stack-mode");
                }
                self.replaceDrilldownParam("IsStackMode", !currentStackModeState);
                if (self.isFirstSwitchMode) {
                    if (currentStackModeState) {
                        alphaReports.splitCell("col-turnover");
                        self.$.each(self.levels, function (index) {
                            var level = self.levels[index];
                            alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                        });
                    }
                    else {
                        if (self.$isAgentLevel.val().toLocaleLowerCase() === "true") {
                            alphaReports.combineConsecutiveCells(self.$("td.col-turnover"), 2, "text-right col-turnover-combined");
                        }
                        else {
                            alphaReports.combineConsecutiveCells(self.$("td.col-turnover"), 3, "text-right col-turnover-combined");
                        }
                        self.$.each(self.levels, function (index) {
                            var level = self.levels[index];
                            var cellCss = "text-right col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                            alphaReports.combineConsecutiveCells(self.$("td.col-" + level + "-winloss"), 3, cellCss);
                        });
                    }
                    self.isFirstSwitchMode = false;
                }
                this.sortComponent.bindTableSorter(this.getSortedColumnName());
            });
            _this.$reportForm.submit(_this.onSubmitReportEvent);
        };
        this.hideFilter = function () {
            var self = _this;
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.slideUp("fast", undefined, function () { });
        };
        this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            var tableReport = _this.$tableReport;
            return {
                CustId: tableReport.data("custid"),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                HideFilter: _this.$reportForm.is(":hidden"),
            };
        };
        this.showValidaProductError = function () {
            _this.$multiSelectButton.addClass("error");
        };
        this.clearError = function () {
            _this.$multiSelectButton.removeClass("error");
        };
        this.replaceDrilldownParam = function (paramName, paramValue) {
            var self = _this;
            _this.$drilldownLinks.each(function () {
                var queryString = self.$(this).attr("href");
                var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
                self.$(this).attr("href", newQueryString);
            });
        };
        this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidaProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? params.HideFilter : true);
            _this.$isHistoricReport.val(params.IsHistoricReport);
            _this.$isStackMode.val(params.IsStackMode);
        };
        this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
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
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.monthPickerComponent = monthPickerComponent;
    }
    return WinLossView;
}());
var winLossView = new WinLossView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new SortComponent($), new MonthPickerComponent($));
$(function () {
    winLossView.init();
});
//# sourceMappingURL=winloss.js.map