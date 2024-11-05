/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/league-multiselection.ts" />
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
var MatchWinLossDetailView = /** @class */ (function (_super) {
    __extends(MatchWinLossDetailView, _super);
    function MatchWinLossDetailView($, dateRangePickerComponent, productMultiSelectionComponent, leagueMultiSelectionComponent, reportTooltip) {
        var _this = _super.call(this, $, new LinkBuilderComponent($)) || this;
        _this.isFirstSwitchMode = true;
        _this.levels = ["member", "agent", "master", "super"];
        _this.getLeaguesByCustomerParam = {
            custId: "",
            fromDate: "",
            toDate: "",
            products: ""
        };
        _this.isLoading = true;
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheElements();
            _this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", _this.showButtonLoadLeague));
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.productMultiSelectionComponent.render(new ProductMultiSelectionOption("dropdown-product", true, "product-ids", _this.showButtonLoadLeague));
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.$userSelectedLeagueIds = _this.$("#league-ids");
            if (_this.$buttonLoadLeague.hasClass("hide") &&
                (_this.$userSelectedLeagueIds.val() !== "" ||
                    alphaReports.convertAttributeToBool(_this.$isUserSelectsAllLeagues.val()))) {
                _this.leagueMultiSelectionComponent.render();
                _this.$buttonLeague = _this.leagueMultiSelectionComponent.$multiSelectButton;
            }
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.getLeaguesByCustomerParam = _this.getLeagueParam();
            _this.initHelp();
            _this.$.unblockUI();
            _this.isLoading = false;
        };
        _this.checkReady = function () {
            return _this.$("#dropdown-product").length > 0;
        };
        _this.cacheElements = function () {
            _this.$pageTitle = _this.$(".page-title");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$reportForm = _this.$("#report-form");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$leagueContainer = _this.$("#dropdown-league-container");
            _this.$loadLeagueURL = _this.$("#LoadLeagueURL");
            _this.$custId = _this.$("#CustId");
            _this.$buttonLoadLeague = _this.$("#btn-loadleague");
            _this.$isUserSelectsAllLeagues = _this.$("#IsUserSelectsAllLeagues");
        };
        _this.registerEvents = function () {
            _this.$("#icon-excel").on("click", _this.onExportExcel);
            _this.$reportForm.submit(_this.onSubmitReport);
            _this.$(".icon-filter").on("click", _this.onClickIconFilter);
            _this.$(".ic-switchmode").on("click", _this.onSwitchMode);
            _this.$buttonLoadLeague.on("click", _this.loadLeagues);
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
        };
        _this.onSubmitReport = function (e) {
            _this.isLoading = true;
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.$buttonProduct.addClass("error");
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            if (!params.IsAnyLeagueSelected) {
                _this.$buttonLeague.addClass("error");
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isStackMode.val(String(params.IsStackMode));
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
            var levels = _this.levels;
            var this$ = _this.$;
            if (_this.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-turnover", "");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                    });
                    alphaReports.splitCell("col-company-winloss", "");
                }
                else {
                    alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 2, "col-turnover-combined");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                        alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
                    });
                    alphaReports.combineConsecutiveCells(this$("td.col-company-winloss"), 2, "col-company-winloss-combined");
                }
                _this.isFirstSwitchMode = false;
            }
            _this.$.unblockUI();
        };
        _this.setStackMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            _this.$pageTitle.addClass("stack-mode");
            tableReport.addClass("stack-mode");
            var this$ = _this.$;
            var levels = _this.levels;
            alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 2, "col-turnover-combined");
            this$.each(levels, function (index) {
                var level = levels[index];
                var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
            });
            alphaReports.combineConsecutiveCells(this$("td.col-company-winloss"), 2, "col-company-winloss-combined");
            _this.isFirstSwitchMode = false;
            _this.$.unblockUI();
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyLeagueSelected: _this.$userSelectedLeagueIds.val() !== "-1",
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.getLeagueParam = function () {
            return {
                custId: _this.$custId.val(),
                fromDate: _this.$fromDate.val(),
                toDate: _this.$toDate.val(),
                products: _this.productMultiSelectionComponent.getSelectedItems().join(",")
            };
        };
        _this.loadLeagues = function () {
            var this$ = _this.$;
            this$.blockUI();
            var param = _this.getLeagueParam();
            _this.getLeaguesByCustomerParam = param;
            _this.$.ajax({
                method: "GET",
                url: _this.$loadLeagueURL.val(),
                cache: false,
                data: param,
                success: _this.renderLeagues,
                complete: _this.$.unblockUI
            });
        };
        _this.renderLeagues = function (html) {
            _this.$leagueContainer.empty().append(_this.$(html).html());
            _this.leagueMultiSelectionComponent.render();
            _this.$buttonLoadLeague = _this.$("#btn-loadleague");
            _this.$buttonLoadLeague.on("click", _this.loadLeagues);
            _this.$buttonLeague = _this.leagueMultiSelectionComponent.$multiSelectButton;
            _this.$userSelectedLeagueIds = _this.$("#league-ids");
            _this.$userSelectedLeagueIds.val("");
            _this.$buttonLoadLeague.addClass("hide");
            _this.$isUserSelectsAllLeagues = _this.$("#IsUserSelectsAllLeagues");
            _this.$buttonLeague.click();
        };
        _this.showButtonLoadLeague = function () {
            if (_this.isLoading) {
                return;
            }
            _this.$userSelectedLeagueIds.val("");
            _this.$buttonLoadLeague.removeClass("hide");
            _this.$leagueContainer.find("> .btn-group:not(.btn-loadleague-container)").addClass("hide");
            _this.$isUserSelectsAllLeagues.val("false");
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.leagueMultiSelectionComponent = leagueMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    return MatchWinLossDetailView;
}(BaseReport));
$(function () {
    var matchWinLossDetailView = new MatchWinLossDetailView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new LeagueMultiSelectionComponent($), new ReportTooltip($));
    matchWinLossDetailView.init();
});
//# sourceMappingURL=matchwinlossdetail.js.map