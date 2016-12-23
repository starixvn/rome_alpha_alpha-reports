"use strict";
var AlphaReports = (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.splitCell = function (firstCell, cellClass) {
            if (cellClass === void 0) { cellClass = ""; }
            var self = _this;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = self.$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " bold" : "") + "\">" +
                        self.$(oldValues[i]).html() +
                        "</td>";
                }
                self.$(combinedCells[index]).before(oldCells);
            });
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var self = _this;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = self.$(firstCells[index]);
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    currentCell = currentCell.next();
                }
                newCell += "</ul>\
                        </td>";
                currentCell.prev().after(newCell);
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                // if internet Explorer, return version number
                return true;
            }
            return false;
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + "&" + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.$ = $;
        this.store = store;
    }
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map