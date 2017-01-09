/// <reference path="../core.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class OutstandingView {
    $tableReport: any;

    sortComponent: SortComponent;

    constructor(sortComponent: SortComponent) {
        this.sortComponent = sortComponent;
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
    };
}

const outstandingView = new OutstandingView(new SortComponent($));

$(function () {
    outstandingView.init();
});
