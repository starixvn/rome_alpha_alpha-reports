/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="../components/tooltip.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var WinLossDetailView = /** @class */ (function (_super) {
    __extends(WinLossDetailView, _super);
    function WinLossDetailView($, dateRangePickerComponent, pageComponent, productMultiSelectionComponent, sortComponent, reportTooltip, monthPickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
        _this.isHistoricReport = false;
        _this.isSwitchView = false;
        _this.hasAllDataOnClient = false;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheElements();
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === "true";
            if (_this.isHistoricReport) {
                _this.$reportForm.removeClass("hide");
                _this.monthPickerComponent.render();
                if (_this.$isFilterVisible.val().toLowerCase() !== "true") {
                    _this.$reportForm.addClass("hide");
                }
            }
            else {
                _this.dateRangePickerComponent.render();
            }
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.pagerComponent.render();
            _this.hasAllDataOnClient = _this.pagerComponent.hasAllDataOnClient();
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, _this.hasAllDataOnClient, _this.sortComponent.afterSort, { 0: { sorter: "text" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.checkReady = function () {
            return _this.$("#IsHistoricReport").length > 0;
        };
        _this.onSwitchReport = function () {
            _this.isSwitchView = true;
            _this.isHistoricReport = !_this.$reportSwitch.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        _this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.hideFilter();
            }
            _this.replaceHrefParam("IsFilterVisible", currentHideState);
        };
        _this.setStackMode = function () {
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
        _this.onSwitchMode = function () {
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
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass("error");
        };
        _this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.onSubmitReport = function (e) {
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
            else if (!params.IsAnyProductDetailSelected) {
                _this.showValidaProductDetailError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$isStackMode.val(String(params.IsStackMode));
            _this.$sortPropertyName.val(params.SortPropertyName);
        };
        _this.getSortedColumnName = function () {
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
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.pagerComponent = pageComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.sortComponent = sortComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    WinLossDetailView.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$drilldownLinks = this.$(".downline-link");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportSwitch = this.$("#historic-report");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$iconNormalColumn = this.$("#icon-normal-column");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$breadcrumbLinks = this.$(".breadcrumb-link");
    };
    WinLossDetailView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportSwitch.on("click", this.onSwitchReport);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    return WinLossDetailView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var winLossDetailView = new WinLossDetailView($, new DateRangePickerComponent($), new PagerComponent($), new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new SortComponent($, linkBuilderComponent), new ReportTooltip($), new MonthPickerComponent($), linkBuilderComponent);
    winLossDetailView.init();
});
//# sourceMappingURL=winlossdetail.js.map