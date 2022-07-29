/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
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
var WinLossAnalysisChartView = /** @class */ (function (_super) {
    __extends(WinLossAnalysisChartView, _super);
    function WinLossAnalysisChartView($, monthRangePickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent, dualAxesChartComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.dualAxisChartComponent.render(new DualAxisChartOption(_this.isDarkMode));
            _this.registerEvents();
            _this.initHelp();
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheElements.call(_this);
            _this.$canvasChart = _this.$("#chart-winloss-analysis");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$msgError = _this.$("#msg-error");
            _this.isDarkMode = _this.$("#hidden-isDarkMode").val() === "1";
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.$reportForm.removeClass("hide");
            _this.monthRangePickerComponent.render();
            if (!alphaReports.convertAttributeToBool(_this.$isFilterVisible.val())) {
                _this.$reportForm.addClass("hide");
            }
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            var isValid = true;
            if (!params.IsAnyProductSelected) {
                _this.showValidationProductError();
                isValid = false;
            }
            if (!params.IsLeast3MonthToChart) {
                _this.showValidationMonthRangeError();
                isValid = false;
            }
            if (!isValid) {
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.hideValidationMonthRangeError();
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        };
        _this.showValidationProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidationMonthRangeError = function () { return _this.$msgError.removeClass("hide"); };
        _this.hideValidationMonthRangeError = function () { return _this.$msgError.addClass("hide"); };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var canvasChart = _this.$canvasChart;
            return {
                CustId: canvasChart.data("custid"),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsLeast3MonthToChart: _this.monthRangePickerComponent.isLeast3Month(),
                HideFilter: _this.$reportForm.is(":hidden")
            };
        };
        _this.$ = $;
        _this.monthRangePickerComponent = monthRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        _this.dualAxisChartComponent = dualAxesChartComponent;
        return _this;
    }
    WinLossAnalysisChartView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportForm.submit(this.onSubmitReportEvent);
    };
    return WinLossAnalysisChartView;
}(BaseReport));
//# sourceMappingURL=winlossanalysischart.js.map