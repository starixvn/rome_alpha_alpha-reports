"use strict";
declare var store, $: any;

class AlphaReports {
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
    pageList = [1, 1000, 2000, 3000, 5000];
    maxTimesToShowTooltip = 5;
    screenSmallThan768 = () => {
        var currentWindowWidth = $(window).width();

        return currentWindowWidth < 768;
    };

    combineColumns = (
        columns: Array<string>,
        combinedColumnClass: string,
        combinedColumnColSpan: number) => {
        var columnsLength = columns ? columns.length : 0;
        if (columnsLength <= 1) {
            return;
        }

        var firstColumn = $(columns[0]);

        firstColumn.each(function (index: number) {

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
    };

    splitColumn = (
        columns: Array<string>,
        combinedColumn: string) => {
        var columnsLength = columns ? columns.length : 0;
        if (columnsLength <= 1) {
            return;
        }

        var combinedColumns = $(combinedColumn);

        combinedColumns.each(function (index: number) {

            var oldValues = $(this).find("li");
            var oldColumns = "";

            for (var i = 0; i < columnsLength; i++) {
                oldColumns += "<td class=\"" + columns[i] + "\">" + $(oldValues[i]).html() + "</td>";
            }

            $($(combinedColumn)[index]).before(oldColumns);
        });
    };

    localStorage = {
        get: (key: string) => {
            return store.enabled ? store.get(key) : $.cookie(key);
        },

        set: (key: string, value: any) => {
            store.enabled ? store.set(key, value) : $.cookie(key, value, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (alphaReports.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };
    isIe = () => {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            // If Internet Explorer, return version number
            return true;
        }

        return false;
    };
    replaceQueryStringValue = (queryString, paramName, paramValue) => {
        var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");

        var newQueryString = regex.test(queryString)
            ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
            : queryString + "&" + paramName + "=" + paramValue;

        return newQueryString;
    };
};

const alphaReports = new AlphaReports();