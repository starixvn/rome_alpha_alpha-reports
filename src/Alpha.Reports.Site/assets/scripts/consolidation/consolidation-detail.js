/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/link-builder.ts" />
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
var ConsolidationDetailView = /** @class */ (function (_super) {
    __extends(ConsolidationDetailView, _super);
    function ConsolidationDetailView($, productMultiSelectionComponent, dateRangePickerComponent, reportTooltip, linkBuilderComponent, pageComponent, sortComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
        _this.hasAllDataOnClient = false;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheElements();
            _this.productMultiSelectionComponent.render();
            _this.$productButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.dateRangePickerComponent.render();
            _this.pagerComponent.render();
            _this.hasAllDataOnClient = _this.pagerComponent.hasAllDataOnClient();
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, _this.hasAllDataOnClient, _this.sortComponent.afterSort, { 0: { sorter: "text" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.initHelp();
            _this.saveConsolidationParam();
            _this.$.unblockUI();
        };
        _this.checkReady = function () {
            return _this.$("#IsFilterVisible").length > 0;
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
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                this$.unblockUI();
                if (e) {
                    e.preventDefault();
                }
                return;
            }
            if (!params.IsAnyProductDetailSelected) {
                _this.showValidaProductDetailError();
                this$.unblockUI();
                if (e) {
                    e.preventDefault();
                }
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isStackMode.val(String(params.IsStackMode));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$consolidationFromDate.val(params.FromDate);
            _this.$consolidationToDate.val(params.ToDate);
            _this.$consolidationUserSelectedDateRangeOption.val(params.UserSelectedDateRangeOption);
            _this.$consolidationProductIds.val(params.UserSelectedProductIds);
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.showValidateProductError = function () {
            _this.$productButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
                return sortedColumn;
            }
            sortedColumn = _this.sortComponent.mapSortedColumnToStackMode(sortedColumn, [["Turnover", "NetTurnover", "GrossComm"],
                ["MemberNetWinLoss.WinLoss", "MemberNetWinLoss.Comm", "MemberNetWinLoss.Total"],
                ["AgentNetWinLoss.WinLoss", "AgentNetWinLoss.Comm", "AgentNetWinLoss.Total"],
                ["MasterNetWinLoss.WinLoss", "MasterNetWinLoss.Comm", "MasterNetWinLoss.Total"],
                ["SuperNetWinLoss.WinLoss", "SuperNetWinLoss.Comm", "SuperNetWinLoss.Total"]]);
            return sortedColumn;
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                IsStackMode: _this.$tableReport.hasClass("stack-mode"),
                IsFilterVisible: !_this.$reportForm.is(":hidden"),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                FromDate: _this.$("#from-date").val(),
                ToDate: _this.$("#to-date").val(),
                UserSelectedDateRangeOption: _this.$("#hidden-range-option").val(),
                UserSelectedProductIds: _this.$("#product-ids").val()
            };
        };
        _this.saveConsolidationParam = function () {
            _this.linkBuilderComponent.updateParam("ConsolidationFromDate", _this.$consolidationFromDate.val());
            _this.linkBuilderComponent.updateParam("ConsolidationToDate", _this.$consolidationToDate.val());
            _this.linkBuilderComponent.updateParam("ConsolidationUserSelectedDateRangeOption", _this.$consolidationUserSelectedDateRangeOption.val());
            _this.linkBuilderComponent.updateParam("ConsolidationProductIds", _this.$consolidationProductIds.val());
            var firstBreadcrumb = _this.$(".breadcrumb > li:first-child").find("a");
            var currentUrl = firstBreadcrumb.attr("href");
            currentUrl = alphaReports.replaceQueryStringValue(currentUrl, "FromDate", _this.$consolidationFromDate.val());
            currentUrl = alphaReports.replaceQueryStringValue(currentUrl, "ToDate", _this.$consolidationToDate.val());
            currentUrl = alphaReports.replaceQueryStringValue(currentUrl, "UserSelectedDateRangeOption", _this.$consolidationUserSelectedDateRangeOption.val());
            currentUrl = alphaReports.replaceQueryStringValue(currentUrl, "UserSelectedProductIds", _this.$consolidationProductIds.val());
            firstBreadcrumb.attr("href", currentUrl);
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.reportTooltip = reportTooltip;
        _this.pagerComponent = pageComponent;
        _this.sortComponent = sortComponent;
        return _this;
    }
    ConsolidationDetailView.prototype.cacheElements = function () {
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$iconNormalColumn = this.$("#icon-normal-column");
        this.$consolidationFromDate = this.$("#ConsolidationFromDate");
        this.$consolidationToDate = this.$("#ConsolidationToDate");
        this.$consolidationUserSelectedDateRangeOption = this.$("#ConsolidationUserSelectedDateRangeOption");
        this.$consolidationProductIds = this.$("#ConsolidationProductIds");
    };
    ConsolidationDetailView.prototype.registerEvents = function () {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    return ConsolidationDetailView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var consolidationDetailView = new ConsolidationDetailView($, new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new DateRangePickerComponent($), new ReportTooltip($), linkBuilderComponent, new PagerComponent($), new SortComponent($, linkBuilderComponent));
    consolidationDetailView.init();
});
//# sourceMappingURL=consolidation-detail.js.map