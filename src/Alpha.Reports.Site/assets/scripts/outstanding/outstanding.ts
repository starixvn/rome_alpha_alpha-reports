/// <reference path="../core.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class OutstandingView {
    $tableReport: any;

    sortComponent: SortComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(sortComponent: SortComponent, productMultiSelectionComponent: ProductMultiSelectionComponent, linkBuilderComponent: LinkBuilderComponent) {
        this.sortComponent = sortComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init = () => {
        this.$tableReport = $("#tbl-report");

        this.sortComponent.render({
            sortContainer: this.$tableReport,
            canSort: parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            hasAllDataOnClient: true,
            headers: {
                0: { sorter: "text" }
            },
            afterSortFunction: function () { }
        });

        this.$tableReport.stickyTableHeaders();

        this.productMultiSelectionComponent.render();
        this.linkBuilderComponent.render();
    };
}

const outstandingView = new OutstandingView(new SortComponent($), new ProductMultiSelectionComponent($), new LinkBuilderComponent($));

$(function () {
    outstandingView.init();
});
