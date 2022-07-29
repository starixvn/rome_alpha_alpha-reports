/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
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
var WinLossByProductView = /** @class */ (function (_super) {
    __extends(WinLossByProductView, _super);
    function WinLossByProductView($, dateRangePickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent) {
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
            _this.$isAgentLevel = _this.$("#IsAgentLevel");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$reportForm = _this.$("#report-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".icon-switchmode");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
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
                var isAgentLevel = _this.$isAgentLevel.val().toLocaleLowerCase() === "true";
                _this.combineColumns(isAgentLevel);
                _this.isFirstSwitchMode = false;
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.dateRangePickerComponent.render();
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
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
        };
        _this.onClickProduct = function (productRow) {
            var productId = productRow.data("product-id");
            var isHide = alphaReports.convertAttributeToBool(productRow.data("is-hide"));
            var columnUsername = productRow.find(".col-username");
            var sportRows = _this.$(".sport-of-" + productId);
            var betTypeRows = _this.$(".bet-type-of-product-" + productId);
            if (isHide) {
                columnUsername.addClass("active");
                columnUsername.find(".icon-arrow-right").removeClass("icon-arrow-right").addClass("icon-arrow-down");
                sportRows.find(".col-username").addClass("active");
                sportRows.find(".col-username .icon-arrow-right").removeClass("icon-arrow-right").addClass("icon-arrow-down");
                sportRows.data("is-hide", "false");
                sportRows.removeClass("hide");
                betTypeRows.removeClass("hide");
            }
            else {
                columnUsername.removeClass("active");
                columnUsername.find(".icon-arrow-down").removeClass("icon-arrow-down").addClass("icon-arrow-right");
                sportRows.find(".col-username").removeClass("active");
                sportRows.find(".col-username .icon-arrow-down").removeClass("icon-arrow-down").addClass("icon-arrow-right");
                sportRows.data("is-hide", "true");
                sportRows.addClass("hide");
                betTypeRows.addClass("hide");
            }
            productRow.data("is-hide", !isHide);
        };
        _this.onClickSport = function (sportRow) {
            var productId = sportRow.data("product-id");
            var sportId = sportRow.data("sport-id");
            var isHide = alphaReports.convertAttributeToBool(sportRow.data("is-hide"));
            var columnUsername = sportRow.find(".col-username");
            var betTypeRows = _this.$(".bet-type-of-product-" + productId + ".bet-type-of-sport-" + sportId);
            if (isHide) {
                columnUsername.addClass("active");
                columnUsername.find(".icon-arrow-right").removeClass("icon-arrow-right").addClass("icon-arrow-down");
                betTypeRows.removeClass("hide");
            }
            else {
                columnUsername.removeClass("active");
                columnUsername.find(".icon-arrow-down").removeClass("icon-arrow-down").addClass("icon-arrow-right");
                betTypeRows.addClass("hide");
            }
            sportRow.data("is-hide", !isHide);
        };
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) {
            var isAgentLevel = _this.$isAgentLevel.val().toLocaleLowerCase() === "true";
            isSwitchToNormalMode ? _this.splitColumns() : _this.combineColumns(isAgentLevel);
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
                if (level == "master") {
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
            if (level == "master") {
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
            var tableReport = _this.$tableReport;
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                CustId: tableReport.data("custid"),
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
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    WinLossByProductView.prototype.registerEvents = function () {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
        var self = this;
        this.$(".product-row").on("click", function () {
            self.onClickProduct(self.$(this));
        });
        this.$(".sport-row").on("click", function () {
            self.onClickSport(self.$(this));
        });
    };
    return WinLossByProductView;
}(BaseReport));
var winLossByProductView = new WinLossByProductView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new ReportTooltip($), new LinkBuilderComponent($));
$(function () {
    winLossByProductView.init();
});
//# sourceMappingURL=winlossbyproduct.js.map