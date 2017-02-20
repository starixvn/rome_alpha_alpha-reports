/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/league-multiselection.ts" />

"use strict";
class MatchWinLossDetailView {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    leagueMultiSelectionComponent: LeagueMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    $pageTitle: JQuery;
    $tableReport: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;

    $buttonProduct: JQuery;
    $buttonLeague: JQuery;
    $isSetDefaultStackMode: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;

    $fromDate: JQuery;
    $toDate: JQuery;
    $leagueContainer: JQuery;
    $loadLeagueURL: JQuery;
    $custId: JQuery;

    isFirstSwitchMode = true;
    levels = ["member", "agent", "master", "super"];
    getLeaguesByCustomerParam = {
        custId: "",
        fromDate: "",
        toDate: "",
        products: ""
    };

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        leagueMultiSelectionComponent: LeagueMultiSelectionComponent,
        reportTooltip: ReportTooltip) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.leagueMultiSelectionComponent = leagueMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheElements();

        this.dateRangePickerComponent.render();
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");

        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.leagueMultiSelectionComponent.render();
        this.$buttonLeague = this.$("#dropdown-league").parent().find(".multiselect.btn");

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0)));
        }

        this.$tableReport.stickyTableHeaders();
        this.registerEvents();

        this.getLeaguesByCustomerParam = this.getLeagueParam();

        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#dropdown-product").length > 0;
    };
    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");

        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$leagueContainer = this.$("#dropdown-league-container");
        this.$loadLeagueURL = this.$("#LoadLeagueURL");
        this.$custId = this.$("#CustId");
    };
    registerEvents = () => {
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$buttonLeague.on("click", this.showLeagues);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;
        this$.blockUI();

        let params = this.getParams();

        if (!params.IsAnyProductSelected) {
            this.$buttonProduct.addClass("error");
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isStackMode.val(String(params.IsStackMode));
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    };
    onSwitchMode = () => {
        this.$.blockUI();
        let tableReport = this.$tableReport;
        let currentStackModeState = tableReport.hasClass("stack-mode");

        if (currentStackModeState) {
            this.$pageTitle.removeClass("stack-mode");
            tableReport.removeClass("stack-mode");
        } else {
            this.$pageTitle.addClass("stack-mode");
            tableReport.addClass("stack-mode");
        }

        let levels = this.levels;
        let this$ = this.$;

        if (this.isFirstSwitchMode) {
            if (currentStackModeState) {
                alphaReports.splitCell("col-turnover", "");
                this$.each(levels, function (index: number) {
                    let level = levels[index];
                    alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                });
                alphaReports.splitCell("col-company-winloss", "");
            } else {
                alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 2, "col-turnover-combined");
                this$.each(levels, function (index: number) {
                    let level = levels[index];
                    let cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                    alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
                });
                alphaReports.combineConsecutiveCells(this$("td.col-company-winloss"), 2, "col-company-winloss-combined");
            }

            this.isFirstSwitchMode = false;
        }

        this.$.unblockUI();
    };
    setStackMode = () => {
        this.$.blockUI();

        let tableReport = this.$tableReport;
        this.$pageTitle.addClass("stack-mode");
        tableReport.addClass("stack-mode");

        let this$ = this.$;

        let levels = this.levels;
        alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 2, "col-turnover-combined");

        this$.each(levels, function (index: number) {
            let level = levels[index];
            let cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
            alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
        });

        alphaReports.combineConsecutiveCells(this$("td.col-company-winloss"), 2, "col-company-winloss-combined");

        this.isFirstSwitchMode = false;
        this.$.unblockUI();
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let tableReport = this.$tableReport;

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    getLeagueParam = () => {
        return {
            custId: this.$custId.val(),
            fromDate: this.$fromDate.val(),
            toDate: this.$toDate.val(),
            products: this.productMultiSelectionComponent.getSelectedItems().join(",")
        };
    };
    showLeagues = (event: any) => {
        if (alphaReports.convertAttributeToBool(this.$buttonLeague.attr("aria-expanded"))) {
            return;
        }

        let this$ = this.$;
        this$.blockUI();

        let param = this.getLeagueParam();
        if (param.fromDate === this.getLeaguesByCustomerParam.fromDate
            && param.toDate === this.getLeaguesByCustomerParam.toDate
            && param.products === this.getLeaguesByCustomerParam.products) {
            this$.unblockUI();
            return;
        }

        if (param.products === "") {
            this$.unblockUI();
            return;
        }

        event.stopPropagation();
        event.preventDefault();
        this.getLeaguesByCustomerParam = param;
        this.$.ajax({
            method: "GET",
            url: this.$loadLeagueURL.val(),
            cache: false,
            data: param,
            success: this.renderLeagues,
            complete: this$.unblockUI
        });
    };
    renderLeagues = (html: string) => {
        this.$leagueContainer.empty().append(this.$(html).html());
        this.leagueMultiSelectionComponent.render();
        this.$buttonLeague = this.$("#dropdown-league").parent().find(".multiselect.btn");
        this.$buttonLeague.on("click", this.showLeagues);
        this.$buttonLeague.click();
    };
}

$(() => {
    const matchWinLossDetailView = new MatchWinLossDetailView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new LeagueMultiSelectionComponent($),
        new ReportTooltip($));

    matchWinLossDetailView.init();
});