"use strict";

class AlphaReports {
    $: any;
    store: any;

    constructor($: any, store: any) {
        this.$ = $;
        this.store = store;
    }

    format = {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash_DatePicker: "MM/DD/YYYY",
        shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
    };

    sort = {
        none: 0,
        ascending: 1,
        descending: -1
    };

    keyCode = {
        enter: 13
    };
    numberDisplayedProductsOnFilter = 1;
    maxTimesToShowTooltip = 5;
    screenSmallThan768 = () => {
        var currentWindowWidth = screen.width || this.$(window).width();

        return currentWindowWidth < 768;
    };

    splitCell = (firstCell: string, cellClass: string = "") => {
        var this$ = this.$;

        var combinedCells = this.$("." + firstCell + "-combined");

        combinedCells.each(function (index: number) {

            var oldValues = this$(this).find("li");
            var cellsLength = oldValues.length;
            var oldCells = "";

            for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                    (i === lastCellIndex ? " bold" : "") + "\">" +
                    this$(oldValues[i]).html() +
                    "</td>";
            }

            this$(combinedCells[index]).before(oldCells);
        });
    };

    combineConsecutiveCells = (
        firstCells: any,
        numberOfCombinedCells: number,
        combinedCellClass: string,
        combinedCellColSpan: number = 1) => {

        if (numberOfCombinedCells <= 1) {
            return;
        }

        var this$ = this.$;

        firstCells.each(function (index: number) {

            var newCell =
                "<td class=\"visible-stack-mode combinedcell " +
                combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";

            var currentCell = this$(firstCells[index]);

            let isLastCell = false;
            for (var i = 0; i < numberOfCombinedCells; i++) {
                newCell += "<li>" + currentCell.html() + "</li>";

                if (currentCell.next().length) {
                    currentCell = currentCell.next();
                } else {
                    isLastCell = true;
                }
            }

            newCell += "</ul>\
                        </td>";

            if (isLastCell) {
                currentCell.after(newCell);
            } else {
                currentCell.prev().after(newCell);
            }
        });
    };

    localStorage = {
        get: (key: string) => {
            return this.store.enabled ? this.store.get(key) : this.$.cookie(key);
        },

        set: (key: string, value: any) => {
            this.store.enabled ? this.store.set(key, value) : this.$.cookie(key, value, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };
    isIe = () => {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            // if internet Explorer, return version number
            return true;
        }

        return false;
    };
    replaceQueryStringValue = (queryString: string, paramName: string, paramValue: any) => {
        var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");

        var newQueryString = regex.test(queryString)
            ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
            : queryString + "&" + paramName + "=" + paramValue;

        return newQueryString;
    };
}

const alphaReports = new AlphaReports($, store);