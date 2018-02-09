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
    $custLevelId: JQuery;

    isFirstSwitchMode = true;
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
                new ReportTooltipOption("icon-normal-column", parseInt(this.$custId.val(), 0))
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
        this.$custLevelId = this.$("#CustLevelId");
        this.$loadLegendURL = this.$("#LoadLegendURL");
        this.$iconInformation = this.$("#icon-information");
        this.$reportLegend = this.$("#report-legend");
    };

    public registerEvents() {
        super.registerEvents();

        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$iconInformation.on("click", this.showLegend);
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

        let this$ = this.$;

        if (this.isFirstSwitchMode) {
            if (currentStackModeState) {
                alphaReports.splitCell("col-contribution-share");
            } else {
                var custLevelId = parseInt(this.$custLevelId.val());
                alphaReports.combineConsecutiveCells(
                    this$("td.col-agent-contribution-share"),
                    custLevelId - 1,
                    "text-right col-agent-contribution-share-combined");
            }
        }

        this.isFirstSwitchMode = false;

        this.$.unblockUI();
    };

    setStackMode = () => {
        if (parseInt(this.$custLevelId.val()) == 2) {
            return;
        }

        this.$.blockUI();

        let tableReport = this.$tableReport;
        this.$pageTitle.addClass("stack-mode");
        tableReport.addClass("stack-mode");

        let this$ = this.$;

        var custLevelId = parseInt(this.$custLevelId.val());
        alphaReports.combineConsecutiveCells(
            this$("td.col-agent-contribution-share"),
            custLevelId - 1,
            "text-right col-agent-contribution-share-combined");

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