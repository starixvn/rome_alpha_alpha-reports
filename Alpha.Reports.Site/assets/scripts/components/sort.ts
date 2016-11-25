/// <reference path="../core.ts" />
"use strict";

class SortOption {
    constructor(
        public sortContainer: any = null,
        public canSort: boolean = true,
        public hasAllDataOnClient: boolean = false,
        public afterSortFunction: any = null) {
    }
}

class SortComponent {
    $: any;
    $sortContainer: any;
    options = new SortOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: SortOption = new SortOption()) => {
        this.options = <SortOption>this.$.extend({}, this.options, options);
        this.$sortContainer = this.options.sortContainer;

        let sortColumns = this.$sortContainer.find("[data-sortedname]");

        if (!this.options.canSort) {
            sortColumns.removeAttr("data-sortedname");
            return;
        }

        var self = this;
        sortColumns
            .off("click.sortcomponent")
            .on("click.sortcomponent",
            function () {
                self.onSort(self.$(this).data("sortedname"));
            });

        if (!this.options.hasAllDataOnClient) {
            let isDecendingSort = this.$sortContainer.data("descendingsort").toLowerCase() === "true";
            let sortClass = isDecendingSort ? "sorting-desc" : "sorting-asc";

            this.$sortContainer
                .find("[data-sortedname='" + this.$sortContainer.data("sortedcolumn") + "']")
                .addClass(sortClass);
        }

        return sortColumns;
    };

    onSort = (sortedColumnName) => {
        let descendingsort =
            sortedColumnName === this.$sortContainer.data("sortedcolumn") &&
            !(this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true") &&
            !(this.$sortContainer.data("descendingsort") === "1");

        this.$sortContainer
            .data("descendingsort", descendingsort ? "1" : "0")
            .data("sortedcolumn", sortedColumnName);

        if (this.options.afterSortFunction &&
            typeof this.options.afterSortFunction === "function") {
            this.options.afterSortFunction();
        }
    }

    getSortInfo = () => {
        return {
            sortPropertyName: this.$sortContainer.data("sortedcolumn"),
            isDescendingSort: this.$sortContainer.data("descendingsort") ? true : false
        };
    };
}

const sortComponent = new SortComponent($);