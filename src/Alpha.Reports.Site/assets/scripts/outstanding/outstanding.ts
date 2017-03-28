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

    $sortPropertyName: JQuery;
    $isDescendingSort: JQuery;
    $isFilterVisible: JQuery;
    $pageTitle: JQuery;

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

        this.sortComponent.render(new SortOption(
            this.$tableReport,
            parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            true,
            this.afterSort,
            { 0: { sorter: "text" } },
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();

        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.linkBuilderComponent.render();

        this.registerEvents();
    };
    cacheElements = () => {
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");

        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
    };
    registerEvents = () => {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let sortInfo = this.sortComponent.getSortInfo();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$buttonProduct.addClass("error");
    };
    afterSort = () => {
        var this$ = this.$;
        var sortedColumn = this$(".sorting-asc:visible");
        var direction = !sortedColumn.length;
        if (sortedColumn.length === 0) {
            sortedColumn = this$(".sorting-desc");
        }

        var sortedColumnName = sortedColumn.data("sortedname");

        this.replaceHrefParam("SortPropertyName", sortedColumnName);
        this.replaceHrefParam("IsDescendingSort", direction);

        var isEvenRow = false;
        this.$tableReport.find("tbody > tr").each(function () {
            var $row = this$(this);
            if ($row.hasClass("even-row") && !isEvenRow) {
                $row.removeClass("even-row");
            } else if (!$row.hasClass("even-row") && isEvenRow) {
                $row.addClass("even-row");
            }

            isEvenRow = !isEvenRow;
        });
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
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

        this.replaceHrefParam("IsFilterVisible", currentHideState);
    };
}

$(function () {
    let linkBuilderComponent = new LinkBuilderComponent($);
    const outstandingView = new OutstandingView(
        $,
        new SortComponent($, linkBuilderComponent),
        new ProductMultiSelectionComponent($),
        linkBuilderComponent);

    outstandingView.init();
});
