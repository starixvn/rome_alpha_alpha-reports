/// <reference path="../core.ts" />
"use strict";

class SortOption {
    sortContainer: any;
    canSort: boolean;
    hasAllDataOnClient: boolean;
    afterSortFunction: any;

    constructor(sortContainer: any = null,
        canSort: boolean = true,
        hasAllDataOnClient: boolean = false,
        afterSortFunction: any = null) {
        this.sortContainer = sortContainer;
        this.canSort = canSort;
        this.hasAllDataOnClient = hasAllDataOnClient;
        this.afterSortFunction = afterSortFunction;
    }
}

class SortComponent {
    $: any;

    options = new SortOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: SortOption = new SortOption()) => {
        var componentOptions = this.$.extend({}, this.options, options);
        var sortContainer = componentOptions.sortContainer;

        var self = this;
        var sortColumns = sortContainer.find("[data-sortedname]");

        if (!componentOptions.canSort) {
            sortColumns.removeAttr("data-sortedname");
            return;
        } else {
            sortColumns.off("click.sortcomponent").on("click.sortcomponent",
                function () {
                    var sortedColumn = self.$(this).data("sortedname");

                    var descendingsort =
                        sortedColumn === sortContainer.data("sortedcolumn") &&
                        !(sortContainer.data("descendingsort").toString().toLowerCase() === "true");

                    sortContainer.data("descendingsort", descendingsort);
                    sortContainer.data("sortedcolumn", sortedColumn);

                    if (componentOptions.afterSortFunction &&
                        typeof componentOptions.afterSortFunction === "function") {
                        componentOptions.afterSortFunction();
                    }
                });
        }

        if (!componentOptions.hasAllDataOnClient) {
            var descendingsort = sortContainer.data("descendingsort").toLowerCase() === "true";

            var sortClass = descendingsort ? "sorting-desc" : "sorting-asc";

            var columns = sortContainer.find("[data-sortedname='" + sortContainer.data("sortedcolumn") + "']").addClass(sortClass);
        }

        return sortColumns;
    };
    getSortInfo = (sortContainer: any) => {
        return {
            SortPropertyName: sortContainer.data("sortedcolumn"),
            IsDescendingSort: sortContainer.data("descendingsort")
        };
    };
}

const sortComponent = new SortComponent($);