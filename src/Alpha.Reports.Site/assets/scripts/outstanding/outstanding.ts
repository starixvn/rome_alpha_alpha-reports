/// <reference path="../core.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class OutstandingView {
    $: JQueryStatic;

    sortComponent: SortComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;

    $tableReport: JQuery;
    $reportForm: JQuery;
    $buttonProduct: JQuery;

    constructor(
        $: JQueryStatic,
        sortComponent: SortComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.sortComponent = sortComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init = () => {
        this.cacheElements();

        this.sortComponent.render({
            sortContainer: this.$tableReport,
            canSort: parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            hasAllDataOnClient: true,
            headers: {
                0: { sorter: "text" }
            },
            afterSortFunction: null
        });

        this.$tableReport.stickyTableHeaders();

        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.linkBuilderComponent.render();

        this.$reportForm.submit(this.onSubmitReport);
    };
    cacheElements = () => {
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();

        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        if (selectedProducts.length === 0) {
            this.showValidateProductError();
            this$.unblockUI();
            e.preventDefault();

            return;
        }
    };
    showValidateProductError = () => {
        this.$buttonProduct.addClass("error");
    };
}

$(function () {
    const outstandingView = new OutstandingView(
        $,
        new SortComponent($),
        new ProductMultiSelectionComponent($),
        new LinkBuilderComponent($));

    outstandingView.init();
});
