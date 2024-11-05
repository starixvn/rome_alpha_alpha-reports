/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
/// <reference path="../components/currency-selection.ts" />
/// <reference path="../components/daterangepicker.ts" />
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
var ConsolidationView = /** @class */ (function (_super) {
    __extends(ConsolidationView, _super);
    function ConsolidationView($, productMultiSelectionComponent, currencySelectionComponent, dateRangePickerComponent, reportTooltip, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheElements();
            _this.productMultiSelectionComponent.render();
            _this.$productButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.currencySelectionComponent.render();
            _this.$currencyButton = _this.$("#dropdown-currencies");
            _this.dateRangePickerComponent.render();
            _this.$tableReport.stickyTableHeaders();
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.checkReady = function () {
            return _this.$("#IsSetDefaultStackMode").length > 0;
        };
        _this.setStackMode = function () {
            _this.$pageTitle.addClass("stack-mode");
            _this.$tableReport.addClass("stack-mode");
            _this.replaceHrefParam("IsStackMode", true);
            alphaReports.combineConsecutiveCells(_this.$("td.col-member-winloss"), 3, "col-member-winloss-combined");
            alphaReports.combineConsecutiveCells(_this.$("td.col-owner-turnover"), 4, "col-member-owner-turnover-combined");
            _this.isFirstSwitchMode = false;
        };
        _this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
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
            if (_this.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-member-winloss");
                    alphaReports.splitCell("col-owner-turnover");
                }
                else {
                    alphaReports.combineConsecutiveCells(_this.$("td.col-member-winloss"), 3, "col-member-winloss-combined");
                    alphaReports.combineConsecutiveCells(_this.$("td.col-owner-turnover"), 4, "col-member-owner-turnover-combined");
                }
                _this.isFirstSwitchMode = false;
            }
            _this.$.unblockUI();
        };
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            _this.currencySelectionComponent.updateSelectedValue();
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
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                IsStackMode: _this.$tableReport.hasClass("stack-mode"),
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$productButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.onChangeReportType = function (actionElement) {
            var $reportForm = _this.$reportForm;
            var action = actionElement.val();
            $reportForm.attr("action", action);
            $reportForm.submit();
        };
        _this.collapseExpandRows = function ($usernameRow) {
            var custId = $usernameRow.data("custid");
            var $collapseExpandIcon = $usernameRow.find("#icon-" + custId);
            if ($collapseExpandIcon.hasClass("ic-arrow-right")) {
                $collapseExpandIcon.removeClass().addClass("ic-arrow-down");
                _this.$tableReport.find(".child-row-" + custId).removeClass("hide");
            }
            else {
                $collapseExpandIcon.removeClass().addClass("ic-arrow-right");
                _this.$tableReport.find(".child-row-" + custId).addClass("hide");
            }
        };
        _this.viewProductPTList = function ($icon) {
            _this.$("#product-pt-list-username").html($icon.data("username"));
            _this.$("#product-pt-list-content").html(_this.buildProductPTListHtml($icon.data("custid")));
            _this.$("#product-pt-list-hint").html(_this.productPTListHint.replace("{0}", $icon.data("userlevel")));
            _this.$productPTListPopup.modal({ show: true, backdrop: false });
        };
        _this.buildProductPTListHtml = function (custId) {
            var html = [];
            var self = _this;
            _this.$tableReport.find(".child-row-" + custId).each(function (index) {
                var $this = self.$(this);
                if ($this.hasClass("consolidation-pt-row")) {
                    if (index !== 0) {
                        html.splice(-1);
                        html.push("</li>");
                    }
                    html.push("<li>");
                    html.push($this.find(".group-pt").html());
                    html.push(": ");
                }
                else {
                    html.push($this.find(".col-product").html());
                    html.push(", ");
                }
            });
            html.splice(-1);
            html.push("</li>");
            return html.join("");
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.currencySelectionComponent = currencySelectionComponent;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    ConsolidationView.prototype.cacheElements = function () {
        this.$tableReport = this.$("#tbl-report");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$productPTListPopup = this.$("#consolidation-product-pt-list-popup");
        this.productPTListHint = this.$("#product-pt-list-hint").html();
    };
    ConsolidationView.prototype.registerEvents = function () {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".ic-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
        var self = this;
        var this$ = this.$;
        this.$("[name=\"report-type\"]").click(function () {
            self.onChangeReportType(this$(this));
        });
        this.$tableReport.find(".username-row").click(function () {
            self.collapseExpandRows(this$(this));
        });
        this.$tableReport.find(".downline-link").click(function (e) {
            e.stopPropagation();
        });
        this.$tableReport.find(".view-product-pt-list").click(function (e) {
            e.stopPropagation();
            self.viewProductPTList(this$(this));
        });
    };
    return ConsolidationView;
}(BaseReport));
$(function () {
    var consolidationView = new ConsolidationView($, new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new CurrencySelectionComponent($), new DateRangePickerComponent($), new ReportTooltip($), new LinkBuilderComponent($));
    consolidationView.init();
});
//# sourceMappingURL=consolidation.js.map