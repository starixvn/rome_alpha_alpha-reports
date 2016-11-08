"use strict";
declare var store: any;
declare var $: any;

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
    pageList = [1000, 2000, 3000, 5000];
    maxTimesToShowTooltip = 5;
    screenSmallThan768 = () => {
        var currentWindowWidth = this.$(window).width();

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

        var firstColumn = this.$(columns[0]);

        firstColumn.each(function (index: number) {

            var newColumn = "<td class=\"" + combinedColumnClass + "\" colspan=\"" + combinedColumnColSpan + "\">\
                                <ul>";

            for (var i = 0; i < columnsLength; i++) {
                var nextColumn = this.$(this.$(columns[i])[index]);
                newColumn += "<li>" + nextColumn.html() + "</li>";
            }

            newColumn += "</ul>\
                        </td>";

            var endColumn = this.$(this.$(columns[columnsLength - 1])[index]);
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

        var combinedColumns = this.$(combinedColumn);

        combinedColumns.each(function (index: number) {

            var oldValues = this.$(this).find("li");
            var oldColumns = "";

            for (var i = 0; i < columnsLength; i++) {
                oldColumns += "<td class=\"" + columns[i] + "\">" + this.$(oldValues[i]).html() + "</td>";
            }

            this.$(this.$(combinedColumn)[index]).before(oldColumns);
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