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
var SuperWinLossDetailByPTView = /** @class */ (function (_super) {
    __extends(SuperWinLossDetailByPTView, _super);
    function SuperWinLossDetailByPTView($, dateRangePickerComponent, productMultiSelectionComponent, sortComponent, reportTooltip, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.hasAllDataOnClient = true;
        _this.isFirstSwitchMode = true;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheDOMElements();
            _this.cacheData();
            _this.bindFilter();
            _this.bindSorter();
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
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
            _this.$reportForm = _this.$("#report-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".icon-switchmode");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
            _this.$viewProductPTList = _this.$("#view-product-pt-list");
            _this.$viewProductPTListPopup = _this.$("#product-pt-list-popup");
        };
        _this.cacheData = function () {
            _this.canSort = JSON.parse(_this.$tableReport.data("cansort"));
        };
        _this.checkReady = function () {
            return _this.$("#IsSetDefaultStackMode").length > 0;
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setCssBySwitchMode(false);
                _this.combineColumns();
                _this.isFirstSwitchMode = false;
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.dateRangePickerComponent.render();
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
            }
        };
        _this.bindSorter = function () {
            _this.canSort && _this.hasAllDataOnClient
                ? _this.$tableReport.find("tbody > tr.hide").removeClass("hide")
                : _this.$tableReport.css("opacity", "");
            _this.sortComponent.render(new SortOption(_this.$tableReport, _this.canSort, _this.hasAllDataOnClient, _this.afterSort, { 0: { sorter: "digit" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
        };
        _this.afterSort = function () {
            _this.sortComponent.afterSort();
            if (_this.hasAllDataOnClient) {
                _this.$tableReport.find("tbody > tr.pt-row").each(function (index, element) {
                    var parent = _this.$(element);
                    var isHide = alphaReports.convertAttributeToBool(parent.data("is-hide"));
                    var ptValue = parent.data("pt-value");
                    var children = _this.$tableReport.find("tbody > tr.pt-product-row[data-pt-value=\"" + ptValue + "\"]");
                    children.insertAfter(parent);
                    isHide && children.addClass("hide");
                });
                _this.$tableReport.css("opacity", "");
            }
        };
        _this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass("hide");
            currentHideState ? _this.showFilter() : _this.hideFilter();
        };
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
            if (_this.canSort && _this.hasAllDataOnClient) {
                _this.$tableReport.css("opacity", 0);
                _this.$tableReport.find("tbody > tr.hide").removeClass("hide");
            }
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
        };
        _this.onClickViewProductPTList = function () {
            _this.$viewProductPTListPopup.modal({ show: true, backdrop: false });
            var position = _this.$viewProductPTList.position();
            _this.$viewProductPTListPopup.css({
                bottom: "auto",
                top: position.top + 60
            });
        };
        _this.onClickPT = function (ptRow) {
            var ptValue = ptRow.data("pt-value");
            var isHide = alphaReports.convertAttributeToBool(ptRow.data("is-hide"));
            var columnUsername = ptRow.find(".col-username");
            var ptProductRows = _this.$(".pt-product-row[data-pt-value=\"" + ptValue + "\"]");
            if (isHide) {
                columnUsername.addClass("active");
                columnUsername.find(".icon-arrow-right").removeClass("icon-arrow-right").addClass("icon-arrow-down");
                ptProductRows.removeClass("hide");
            }
            else {
                columnUsername.removeClass("active");
                columnUsername.find(".icon-arrow-down").removeClass("icon-arrow-down").addClass("icon-arrow-right");
                ptProductRows.addClass("hide");
            }
            ptRow.data("is-hide", !isHide);
        };
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) {
            isSwitchToNormalMode ? _this.splitColumns() : _this.combineColumns();
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
            alphaReports.splitCell("col-turnover");
            _this.$.each(_this.levels, function (index, level) {
                alphaReports.splitCell("col-" + level + "-winloss");
            });
        };
        _this.combineColumns = function () {
            var self = _this;
            alphaReports.combineConsecutiveCells(_this.$("td.col-turnover"), 3, "text-right col-turnover-combined");
            _this.$.each(_this.levels, function (index, level) {
                self.combineToWinLossColumn(level);
            });
        };
        _this.combineToWinLossColumn = function (level) {
            alphaReports.combineConsecutiveCells(_this.$("td.col-" + level + "-winloss"), 3, "text-right col-" + level + "-winloss-combined");
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
            var tableReport = _this.$tableReport;
            var sortInfo = _this.sortComponent.getSortInfo();
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                CustId: tableReport.data("custid"),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                IsStackMode: tableReport.hasClass("stack-mode"),
                HideFilter: _this.$reportForm.is(":hidden"),
            };
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
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
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isStackMode.val(String(params.IsStackMode));
        };
        _this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
                return sortedColumn;
            }
            if (sortedColumn === "NetTurnover" || sortedColumn === "GrossComm") {
                return "Turnover";
            }
            if (sortedColumn === "MemberComm" || sortedColumn === "MemberTotal") {
                return "MemberWinLost";
            }
            if (sortedColumn === "AgComm" || sortedColumn === "AgTotal") {
                return "AgWinLost";
            }
            if (sortedColumn === "MAgComm" || sortedColumn === "MAgTotal") {
                return "MAgWinLost";
            }
            if (sortedColumn === "SAgComm" || sortedColumn === "SAgTotal") {
                return "SAgWinLost";
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
    SuperWinLossDetailByPTView.prototype.registerEvents = function () {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
        this.$viewProductPTList.on("click", this.onClickViewProductPTList);
        var self = this;
        this.$(".pt-row").on("click", function () {
            self.onClickPT(self.$(this));
        });
    };
    return SuperWinLossDetailByPTView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var productDetailMultiSelectionComponent = new ProductDetailMultiSelectionComponent($);
    var superWinLossDetailByPTView = new SuperWinLossDetailByPTView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($, productDetailMultiSelectionComponent), new SortComponent($, linkBuilderComponent), new ReportTooltip($), linkBuilderComponent);
    superWinLossDetailByPTView.init();
});
//# sourceMappingURL=superwinlossdetailbypt.js.map