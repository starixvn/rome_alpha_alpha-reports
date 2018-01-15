/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />

"use strict";
class ProgressiveContributionView extends BaseReport {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    $buttonProduct: JQuery;
    $isSetDefaultStackMode: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;

    $fromDate: JQuery;
    $toDate: JQuery;
    $custId: JQuery;
    isFirstSwitchMode = true;
    levels = ["member", "agent", "master", "super"];
    isLoading = true;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.cacheDOMElements();

        this.bindFilter();

        this.$tableReport.stickyTableHeaders();
        this.registerEvents();
        this.initHelp();
        this.isLoading = false;
    };

    checkReady = () => {
        return this.$("#dropdown-product").length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }

        this.dateRangePickerComponent.render();

        if (this.$isFilterVisible.val().toString().toLowerCase() === "false") {
            this.hideFilter();
        }
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    cacheDOMElements = () => {
        this.cacheElements();
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$custId = this.$("#CustId");
    };

    public registerEvents() {
        super.registerEvents();
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
    };

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    onSubmitReport = (e: any) => {
        this.isLoading = true;

        this.$.blockUI();

        let params = this.getParams();

        if (!params.IsAnyProductSelected) {
            this.$buttonProduct.addClass("error");
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isStackMode.val(String(params.IsStackMode));
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
}

$(() => {
    const progressiveContributionView = new ProgressiveContributionView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new LinkBuilderComponent($));

    progressiveContributionView.init();
});