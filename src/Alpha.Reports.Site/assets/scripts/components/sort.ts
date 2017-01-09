/// <reference path="../core.ts" />
"use strict";

class SortOption {
    constructor(
        public sortContainer: any = null,
        public canSort: boolean = true,
        public hasAllDataOnClient: boolean = false,
        public afterSortFunction: any = null,
        public headers: any = null) {
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
            sortColumns.removeAttr("data-sortedname").removeClass("sorting");
            return;
        }

        let self = this;

        if (!this.options.hasAllDataOnClient) {
            let isDecendingSort = this.$sortContainer.data("descendingsort").toString() === "1"
                || this.$sortContainer.data("descendingsort").toLowerCase() === "true";
            let sortClass = isDecendingSort ? "sorting-desc" : "sorting-asc";

            this.$sortContainer
                .find("[data-sortedname='" + this.$sortContainer.data("sortedcolumn") + "']")
                .addClass(sortClass);

            sortColumns
                .off("click.sortcomponent")
                .on("click.sortcomponent",
                function () {
                    self.onSort(self.$(this).data("sortedname"));
                });
        }

        this.bindTableSorter(this.$sortContainer.data("sortedcolumn"));

        return sortColumns;
    };

    onSort = (sortedColumnName: string) => {
        let descendingsort = sortedColumnName !== this.$sortContainer.data("sortedcolumn")
            || this.$sortContainer.data("descendingsort").toString().toLowerCase() !== "true"
            && this.$sortContainer.data("descendingsort") !== "1";

        this.$sortContainer
            .data("descendingsort", descendingsort ? "1" : "0")
            .data("sortedcolumn", sortedColumnName);

        if (this.options.afterSortFunction &&
            typeof this.options.afterSortFunction === "function") {
            this.options.afterSortFunction();
        }
    };

    getSortInfo = () => {
        return {
            sortPropertyName: this.$sortContainer.data("sortedcolumn"),
            isDescendingSort: this.$sortContainer.data("descendingsort").toString() === "1" ||
            this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true"
        };
    };
    bindTableSorter = (sortedColumnName: string) => {
        if (!this.options.canSort || !this.options.hasAllDataOnClient) {
            return;
        }

        let self = this;

        let disabledSortColumns = this.$sortContainer.find("th[data-disabledsort='true']");
        disabledSortColumns.each(function () {
            self.options.headers[this.cellIndex] = { sorter: false };
        });

        let sortedColumnHeader = this.$sortContainer.find("[data-sortedname='" + sortedColumnName + "']" +
            (this.$sortContainer.hasClass("stack-mode") ? ":not([class*='hidden-stack-mode'])" : ":not([class*='col-combine'])"))[0];
        let isDescendingSort = this.$sortContainer.data("descendingsort").toString() === "1" ||
            this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true";

        this.$sortContainer.tablesorter({
            defaultParser: 1,
            headers: this.options.headers,
            sortList: [[this.$(sortedColumnHeader).data("colindex"), isDescendingSort ? 1 : 0]],
            textExtraction: this.textExtraction
        });

        this.$sortContainer.off("sortEnd").on("sortEnd", this.options.afterSortFunction);
    };
    textExtraction = (node: any) => {
        let $node = this.$(node);

        if ($node.hasClass("combinedcell")) {
            return $node.find("ul>li:first-child").html();
        }

        if ($node.data("islink")) {
            return $node.find("a").html();
        }

        return $node.html();
    };
}