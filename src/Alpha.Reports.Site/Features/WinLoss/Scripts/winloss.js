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
var WinLossView = /** @class */ (function (_super) {
    __extends(WinLossView, _super);
    function WinLossView($, dateRangePickerComponent, productMultiSelectionComponent, levelTypeDropdown, sortComponent, reportTooltip, monthPickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
        _this.isHistoricReport = false;
        _this.isSwitchView = false;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.bindSorting();
            _this.$tableReport.stickyTableHeaders();
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$drilldownLinks = _this.$(".downline-link");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$isAgentLevel = _this.$("#IsAgentLevel");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$reportForm = _this.$("#report-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$historicReport = _this.$("#historic-report");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".ic-switchmode");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
        };
        _this.checkReady = function () {
            return _this.$("#IsHistoricReport").length > 0;
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.levelTypeDropdown.render(new SingleSelectionDropdownOption("dropdown-level-type"));
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
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
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
            }
        };
        _this.bindSorting = function () {
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            var sortHeaders = {
                0: { sorter: "text" }
            };
            if (_this.levelTypeDropdown.$dropdown.length) {
                sortHeaders[1] = { sorter: "text" };
            }
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, true, _this.sortComponent.afterSort, sortHeaders));
        };
        _this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass("hide");
            currentHideState ? _this.showFilter() : _this.hideFilter();
            _this.replaceDrilldownParam("IsFilterVisible", currentHideState);
        };
        _this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.isSwitchView = true;
            _this.isHistoricReport = !_this.$historicReport.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        _this.onSwitchModeEvent = function () {
            var isStackMode = _this.$tableReport.hasClass("stack-mode");
            _this.replaceDrilldownParam("IsStackMode", !isStackMode);
            _this.setCssBySwitchMode(isStackMode);
            if (_this.isFirstSwitchMode) {
                _this.renderAtFirstSwitchMode(isStackMode);
                _this.isFirstSwitchMode = false;
            }
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
        };
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) {
            var isAgentLevel = _this.$isAgentLevel.val().toLocaleLowerCase() === "true";
            isSwitchToNormalMode ? _this.splitColumns() : _this.combineColumns(isAgentLevel);
        };
        _this.setStackMode = function () {
            _this.$.blockUI();
            _this.setCssBySwitchMode(false);
            _this.renderAtFirstSwitchMode(false);
            _this.isFirstSwitchMode = false;
            _this.replaceDrilldownParam("IsStackMode", true);
            _this.$.unblockUI();
        };
        _this.setCssBySwitchMode = function (isSwitchToNormalMode) {
            if (isSwitchToNormalMode) {
                _this.$pageTitle.removeClass("stack-mode");
                _this.$tableReport.removeClass("stack-mode");
            }
            else {
                _this.$pageTitle.addClass("stack-mode");
                _this.$tableReport.addClass("stack-mode");
            }
        };
        _this.splitColumns = function () {
            var this$ = _this.$;
            var levels = _this.levels;
            alphaReports.splitCell("col-turnover");
            this$.each(levels, function (index) {
                var level = levels[index];
                if (level === "master") {
                    alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? "" : " altercol");
                }
                else {
                    alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                }
            });
        };
        _this.combineColumns = function (isAgentLevel) {
            var levels = _this.levels;
            var self = _this;
            isAgentLevel ? _this.combineToTurnOverColumnAgentLevel() : _this.combineToTurnOverColumnSuperMasterLevel();
            _this.$.each(levels, function (index) {
                self.combineToWinLossColumn(levels[index], index);
            });
        };
        _this.combineToTurnOverColumnAgentLevel = function () {
            alphaReports.combineConsecutiveCells(_this.$("td.col-turnover"), 2, "text-right col-turnover-combined");
        };
        _this.combineToTurnOverColumnSuperMasterLevel = function () {
            alphaReports.combineConsecutiveCells(_this.$("td.col-turnover"), 3, "text-right col-turnover-combined");
        };
        _this.combineToWinLossColumn = function (level, index) {
            var cellCss = "text-right col-" + level + "-winloss-combined";
            if (level === "master") {
                cellCss = cellCss + (index % 2 ? "" : " altercol");
            }
            else {
                cellCss = cellCss + (index % 2 ? " altercol" : "");
            }
            alphaReports.combineConsecutiveCells(_this.$("td.col-" + level + "-winloss"), 3, cellCss);
        };
        _this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var tableReport = _this.$tableReport;
            var sortInfo = _this.sortComponent.getSortInfo();
            return {
                CustId: tableReport.data("custid"),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                HideFilter: _this.$reportForm.is(":hidden")
            };
        };
        _this.showValidaProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass("error");
        };
        _this.replaceDrilldownParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidaProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            if (!params.IsAnyProductDetailSelected) {
                _this.showValidaProductDetailError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$isStackMode.val(String(params.IsStackMode));
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
        };
        _this.getSortedColumnName = function () {
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
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.levelTypeDropdown = levelTypeDropdown;
        _this.sortComponent = sortComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    WinLossView.prototype.registerEvents = function () {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$historicReport.on("click", this.onSubmitHistoricReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
    };
    return WinLossView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var winLossView = new WinLossView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new SingleSelectionDropdownComponent($), new SortComponent($, linkBuilderComponent), new ReportTooltip($), new MonthPickerComponent($), linkBuilderComponent);
    winLossView.init();
});
//# sourceMappingURL=winloss.js.map