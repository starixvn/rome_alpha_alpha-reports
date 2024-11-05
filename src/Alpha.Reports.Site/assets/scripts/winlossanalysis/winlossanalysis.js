/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />
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
var WinLossAnalysisView = /** @class */ (function (_super) {
    __extends(WinLossAnalysisView, _super);
    function WinLossAnalysisView($, dateRangePickerComponent, productMultiSelectionComponent, sortComponent, reportTooltip, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
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
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$reportForm = _this.$("#report-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".ic-switchmode");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$toDate = _this.$("#to-date");
            _this.$reportTypeId = _this.$("#ReportTypeId");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
        };
        _this.checkReady = function () { return _this.$("#IsSetDefaultStackMode").length > 0; };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.dateRangePickerComponent.render();
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
            }
        };
        _this.bindSorting = function () {
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, true, _this.afterSort, { 0: { sorter: _this.getSorterName() } }));
        };
        _this.getSorterName = function () {
            switch (_this.$reportTypeId.val()) {
                case "1": return "shortDate";
                case "2": return "shortMonth";
                default: return "text";
            }
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
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isStackMode.val(String(params.IsStackMode));
            _this.$sortPropertyName.val(_this.getSortPropertyName(_this.$sortPropertyName.val()));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$reportTypeId.val(params.ReportTypeId);
        };
        _this.getSortPropertyName = function (sortPropertyName) { return _this.$reportTypeId.val() === 3 && sortPropertyName === "ReportTypeName" ? "" : sortPropertyName; };
        _this.onFilterEvent = function () { return _this.$reportForm.hasClass("hide") ? _this.showFilter() : _this.hideFilter(); };
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) { return isSwitchToNormalMode ? _this.splitColumns() : _this.combineColumns(); };
        _this.showValidaProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.clearError = function () { return _this.$multiSelectButton.removeClass("error"); };
        _this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.onSwitchModeEvent = function () {
            var isStackMode = _this.$tableReport.hasClass("stack-mode");
            _this.setCssBySwitchMode(isStackMode);
            if (_this.isFirstSwitchMode) {
                _this.renderAtFirstSwitchMode(isStackMode);
                _this.isFirstSwitchMode = false;
            }
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
        };
        _this.setStackMode = function () {
            _this.$.blockUI();
            _this.setCssBySwitchMode(false);
            _this.renderAtFirstSwitchMode(false);
            _this.isFirstSwitchMode = false;
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
            this$.each(levels, function (index) {
                var level = levels[index];
                alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
            });
        };
        _this.combineColumns = function () {
            var levels = _this.levels;
            var this$ = _this.$;
            _this.$.each(levels, function (index) {
                var level = levels[index];
                var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
            });
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
                IsStackMode: tableReport.hasClass("stack-mode"),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                HideFilter: _this.$reportForm.is(":hidden"),
                ReportTypeId: _this.$('input[name=reportType]:checked').val()
            };
        };
        _this.afterSort = function () {
            var this$ = _this.$;
            var sortedColumn = this$(".sorting-asc:visible");
            var direction = !sortedColumn.length;
            if (sortedColumn.length === 0) {
                sortedColumn = this$(".sorting-desc");
            }
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
        };
        _this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
                return sortedColumn;
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
        _this.sortComponent = sortComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    WinLossAnalysisView.prototype.registerEvents = function () {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
        var self = this;
        this.$(':radio[name=reportType]').change(function () {
            self.onSubmitReportEvent(this.form);
            self.$reportForm.submit();
        });
    };
    ;
    return WinLossAnalysisView;
}(BaseReport));
var winLossAnalysisView = new WinLossAnalysisView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new SortComponent($, new LinkBuilderComponent($)), new ReportTooltip($), new LinkBuilderComponent($));
$(function () {
    winLossAnalysisView.init();
});
//# sourceMappingURL=winlossanalysis.js.map