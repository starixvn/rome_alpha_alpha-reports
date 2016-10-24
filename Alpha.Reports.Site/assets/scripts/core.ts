"use strict";
var alphaReports = {
    format: {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash: "MM/DD/YYYY"
    },
    keyCode: {
        enter: 13
    },
    numberDisplayedProductsOnFilter: 1,
    pageList: [10, 50, 100, 500],
    screenSmallThan768: function () {
        var currentWindowWidth = window.screen.width;

        return currentWindowWidth < 768;
    },

    combineColumns: function (
        columns: Array<string>,
        combinedColumnClass: string,
        combinedColumnColSpan: number) {
        var columnsLength = columns ? columns.length : 0;
        if (columnsLength <= 1) {
            return;
        }

        var firstColumn = $(columns[0]);

        firstColumn.each(function (index) {

            var newColumn = "<td class=\"" + combinedColumnClass + "\" colspan=\"" + combinedColumnColSpan + "\">\
                                <ul>";

            for (var i = 0; i < columnsLength; i++) {
                var nextColumn = $($(columns[i])[index]);
                newColumn += "<li>" + nextColumn.html() + "</li>";
            }

            newColumn += "</ul>\
                        </td>";

            var endColumn = $($(columns[columnsLength - 1])[index]);
            endColumn.after(newColumn);
        });
    },

    splitColumn: function (
        columns: Array<string>,
        combinedColumn: string) {
        var columnsLength = columns ? columns.length : 0;
        if (columnsLength <= 1) {
            return;
        }

        var combinedColumns = $(combinedColumn);
        var oldValues = combinedColumns.find("li");

        combinedColumns.each(function (index) {
            var oldColumns = "";

            for (var i = 0; i < columnsLength; i++) {
                oldColumns += "<td class=\"" + columns[i] + "\">" + $(oldValues[i]).html() + "</td>";
            }

            $($(combinedColumn)[index]).before(oldColumns);
        });
    }
};