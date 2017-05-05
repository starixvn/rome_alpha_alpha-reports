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
    $msgError: JQuery;

    monthRangePickerComponent: MonthRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    linkBuilderComponent: LinkBuilderComponent;
    dualAxisChartComponent: DualAxisChartComponent;

    constructor(
        $: JQueryStatic,
        monthRangePickerComponent: MonthRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent,
        dualAxesChartComponent: DualAxisChartComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.monthRangePickerComponent = monthRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
        this.dualAxisChartComponent = dualAxesChartComponent;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.bindFilter();
        this.dualAxisChartComponent.render();

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
        this.$msgError = this.$("#msg-error")
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

        let params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidationProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        if (!params.IsLeast3MonthToChart) {
            this.showValidationMonthRangeError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }
        else {
            this.hideValidationMonthRangeError();
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
    };

    onFilterEvent = () => this.$reportForm.hasClass("hide") ? this.showFilter() : this.hideFilter();

    showValidationProductError = () => this.$multiSelectButton.addClass("error");

    showValidationMonthRangeError = () => this.$msgError.removeClass("hide");
    hideValidationMonthRangeError = () => this.$msgError.addClass("hide");

    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let canvasChart = this.$canvasChart;

        return {
            CustId: canvasChart.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            IsLeast3MonthToChart: this.monthRangePickerComponent.isLeast3Month(),
            HideFilter: this.$reportForm.is(":hidden")
        };
    };
}