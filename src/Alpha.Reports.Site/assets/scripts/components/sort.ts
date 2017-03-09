/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    tablesorter(option: any): void;
}

class SortOption {
    constructor(
        public sortContainer: any = null,
        public canSort: boolean = true,
        public hasAllDataOnClient: boolean = false,
        public afterSortFunction: any = null,
        public headers: any = null,
        public reportForm: JQuery = null) {
    }
}

class SortComponent {
    $: JQueryStatic;
    $sortContainer: JQuery;
    linkBuilderComponent: LinkBuilderComponent;
    $reportForm: JQuery;
    options = new SortOption();

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    render = (options: SortOption = new SortOption()) => {
        this.options = <SortOption>this.$.extend({}, this.options, options);
        this.$sortContainer = this.options.sortContainer;
        this.$reportForm = this.options.reportForm;

        let sortColumns = this.$sortContainer.find("[data-sortedname]");

        if (!this.options.canSort) {
            sortColumns.removeAttr("data-sortedname").removeClass("sorting");
            return;
        }

        let self = this;

        if (!this.options.hasAllDataOnClient) {
            this.$sortContainer
                .find("[data-sortedname='" + this.$sortContainer.data("sortedcolumn") + "']")
                .addClass(this.getSortClassWhenSortOnServer());

            sortColumns
                .off("click.sortcomponent")
                .on("click.sortcomponent",
                function () {
                    var $sortColumn = self.$(this);

                    self.onSort($sortColumn.data("sortedname"), self.getSortClassWhenSortOnServer() === "sorting-asc");
                });
        }

        this.bindTableSorter(this.$sortContainer.data("sortedcolumn"));

        return sortColumns;
    };

    getSortClassWhenSortOnServer = () => {
        let isDecendingSort = this.$sortContainer.data("descendingsort").toString() === "1"
            || this.$sortContainer.data("descendingsort").toLowerCase() === "true";

        return isDecendingSort ? "sorting-desc" : "sorting-asc";
    };

    onSort = (sortedColumnName: string, isDecendingSort: boolean) => {
        this.$sortContainer
            .data("descendingsort", isDecendingSort ? "1" : "0")
            .data("sortedcolumn", sortedColumnName)

        if (this.options.afterSortFunction &&
            typeof this.options.afterSortFunction === "function") {
            this.options.afterSortFunction();
        }
    };

    getSortInfo = () => {
        return {
            sortPropertyName: this.$sortContainer.data("sortedcolumn"),
            isDescendingSort: this.$sortContainer.data("descendingsort").toString() === "1"
            || this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true"
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

        let sortList = [];
        if (sortedColumnName && sortedColumnName !== "") {
            let sortedColumnHeader = this.$sortContainer.find("[data-sortedname='" + sortedColumnName + "']" +
                (this.$sortContainer.hasClass("stack-mode") ? ":not([class*='hidden-stack-mode'])" : ":not([class*='col-combine'])"))[0];
            let isDescendingSort = this.$sortContainer.data("descendingsort").toString() === "1"
                || this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true";

            sortList = [[this.$(sortedColumnHeader).data("colindex"), isDescendingSort ? 1 : 0]];
        }

        try {
            this.$sortContainer.tablesorter(
                {
                    defaultParser: 1,
                    headers: this.options.headers,
                    sortList: sortList,
                    textExtraction: this.textExtraction
                });
        }
        catch (e) {
            console.log(e);
        }

        this.$sortContainer.off("sortEnd").on("sortEnd", () => {
            var $sortedcolumn = this.$sortContainer
                .find("thead.tableFloatingHeaderOriginal th.sorting.sorting-asc:visible,thead.tableFloatingHeaderOriginal th.sorting.sorting-desc:visible");

            var modeFilterClass = this.$sortContainer.hasClass("stack-mode")
                ? ".hidden-stack-mode"
                : ".visible-stack-mode";
            $sortedcolumn = $sortedcolumn.not(modeFilterClass);

            if ($sortedcolumn.length === 1) {
                this.onSort($sortedcolumn.attr("data-sortedname"), $sortedcolumn.hasClass("sorting-desc"));
            }
        });
    };

    textExtraction = (node: any) => {
        let $node = this.$(node);

        if ($node.data("islink")) {
            return $node.find("a").html();
        }

        var html = $node.hasClass("combinedcell") ?
            $node.find("ul>li:first-child").html() :
            $node.html();

        return html;
    };

    afterSort = () => {
        var this$ = this.$;
        if (this.options.hasAllDataOnClient) {
            var sortedColumn = this$(".sorting-asc:visible");
            var direction = !sortedColumn.length;
            if (sortedColumn.length === 0) {
                sortedColumn = this$(".sorting-desc");
            }

            var sortedColumnName = sortedColumn.data("sortedname");

            this.linkBuilderComponent.updateParam("SortPropertyName", sortedColumnName || sortedColumn.parent().data("sortedname"));
            this.linkBuilderComponent.updateParam("IsDescendingSort", direction.toString());

            alphaReports.correctEvenRowClass(this.options.sortContainer);
        } else {
            this.$reportForm.trigger("submit");
        }
    };
}