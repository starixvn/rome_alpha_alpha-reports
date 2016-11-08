/// <reference path="../core.ts" />
"use strict";

class SortOption {
    sortContainer: any;
    canSort: boolean;
    afterSortFunction: any;

    constructor(sortContainer: any = null,
        canSort: boolean = true,
        afterSortFunction: any = null) {
        this.sortContainer = sortContainer;
        this.canSort = canSort;
        this.afterSortFunction = afterSortFunction;
    }
}

class SortComponent {
    $: any;

    options = new SortOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: any = {}) => {
        var componentOptions = this.$.extend({}, this.options, options);
        var sortContainer = componentOptions.sortContainer;

        var self = this;
        var sortColumns = sortContainer.find("[data-sortedname]");

        if (!componentOptions.canSort) {
            sortColumns.removeAttr("data-sortedname");
            return;
        } else {
            sortColumns.on("click",
                function () {
                    var sortedColumn = self.$(this).data("sortedname");

                    var descendingsort =
                        sortedColumn !== sortContainer.data("sortedcolumn") ||
                        !(sortContainer.data("descendingsort").toLowerCase() === "true");

                    sortContainer.data("descendingsort", descendingsort);
                    sortContainer.data("sortedcolumn", sortedColumn);

                    if (componentOptions.afterSortFunction &&
                        typeof (componentOptions.afterSortFunction) === "function") {
                        componentOptions.afterSortFunction();
                    }
                });
        }

        var sortedColumn = sortContainer.data("sortedcolumn");
        var descendingsort = sortContainer.data("descendingsort").toLowerCase() === "true";

        var sortClass = descendingsort
            ? "icon-menu-down descSorting"
            : "icon-menu-up ascSorting";

        var columns = sortContainer.find("[data-sortedname='" + sortedColumn + "']");
        columns.each(function () {
            var column = self.$(this);
            var isCombinedColumn = column.hasClass("col-combine");

            if (isCombinedColumn) {
                column.find(".prior-sortedcolumn").append("<span class=\"" + sortClass + "\"></span>");
            } else {
                column.append("<span class=\"" + sortClass + "\"></span>");
            }
        });

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