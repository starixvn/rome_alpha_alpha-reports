/// <reference path="../core.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
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
var ProgressiveContributionView = /** @class */ (function (_super) {
    __extends(ProgressiveContributionView, _super);
    function ProgressiveContributionView($, dateRangePickerComponent, monthPickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isHistoricReport = false;
        _this.isFirstSwitchMode = true;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.initHelp();
        };
        _this.checkReady = function () {
            return _this.$("#dropdown-product").length > 0;
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render(undefined);
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$custId.val(), 0)));
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
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.cacheDOMElements = function () {
            _this.cacheElements();
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.$isAgentLevel = _this.$("#IsAgentLevel");
            _this.$custId = _this.$("#CustId");
            _this.$custLevelId = _this.$("#CustLevelId");
            _this.$historicReport = _this.$("#historic-report");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".icon-switchmode");
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
        };
        _this.onSubmitReport = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.$buttonProduct.addClass("error");
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$isStackMode.val(String(params.IsStackMode));
        };
        _this.onSubmitHistoricReport = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.isHistoricReport = !_this.$historicReport.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        _this.onSwitchMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            var isStackMode = tableReport.hasClass("stack-mode");
            _this.setCssBySwitchMode(isStackMode);
            if (_this.isFirstSwitchMode) {
                _this.renderAtFirstSwitchMode(isStackMode);
                _this.isFirstSwitchMode = false;
            }
            _this.$.unblockUI();
        };
        _this.setStackMode = function () {
            var isAgentLevel = _this.$isAgentLevel.val().toLocaleLowerCase() === "true";
            if (isAgentLevel) {
                return;
            }
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            _this.setCssBySwitchMode(false);
            _this.renderAtFirstSwitchMode(false);
            _this.isFirstSwitchMode = false;
            _this.$.unblockUI();
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
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
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) {
            var custLevelId = parseInt(_this.$custLevelId.val());
            var level = _this.levels[custLevelId - 1];
            if (isSwitchToNormalMode) {
                alphaReports.splitCell("col-" + level + "-contribution-share", "", "");
            }
            else {
                alphaReports.combineConsecutiveCells(_this.$("td.col-" + level + "-contribution-share"), custLevelId - 1, "text-right col-" + level + "-contribution-share-combined");
            }
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.monthPickerComponent = monthPickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    ProgressiveContributionView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$historicReport.on("click", this.onSubmitHistoricReport);
        this.$switchModeIcon.on("click", this.onSwitchMode);
    };
    ;
    return ProgressiveContributionView;
}(BaseReport));
$(function () {
    var progressiveContributionView = new ProgressiveContributionView($, new DateRangePickerComponent($), new MonthPickerComponent($), new ProductMultiSelectionComponent($), new ReportTooltip($), new LinkBuilderComponent($));
    progressiveContributionView.init();
});
//# sourceMappingURL=progressivecontribution.js.map