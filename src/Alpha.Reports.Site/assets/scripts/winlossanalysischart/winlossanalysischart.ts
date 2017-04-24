/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />

"use strict";

class WinLossAnalysisChartView extends BaseReport {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $canvasChart: JQuery;

    $multiSelectButton: JQuery;
    $isFilterVisible: JQuery;
    $reportForm: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $filterIcon: JQuery;
    $submitButton: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    monthRangePickerComponent: MonthRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        monthRangePickerComponent: MonthRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.monthRangePickerComponent = monthRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.bindFilter();

        this.registerEvents();

        this.initHelp();
        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$canvasChart = this.$("#chart-winloss-analysis")
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportForm = this.$("#report-form");
        this.$filterIcon = this.$(".icon-filter");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.monthRangePickerComponent.render();

        if (this.$isFilterVisible.val().toString().toLowerCase() === "false") {
            this.hideFilter();
        }
    };

    public registerEvents() {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
    };

    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidaProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
    };

    onFilterEvent = () => this.$reportForm.hasClass("hide") ? this.showFilter() : this.hideFilter();

    showValidaProductError = () => this.$multiSelectButton.addClass("error");

    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        var canvasChart = this.$canvasChart;

        return {
            CustId: canvasChart.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            HideFilter: this.$reportForm.is(":hidden")
        };
    };
}

const winLossAnalysisChartView = new WinLossAnalysisChartView(
    $,
    new MonthRangePickerComponent($),
    new ProductMultiSelectionComponent($),
    new ReportTooltip($),
    new LinkBuilderComponent($));

$(function () {
    winLossAnalysisChartView.init();
});