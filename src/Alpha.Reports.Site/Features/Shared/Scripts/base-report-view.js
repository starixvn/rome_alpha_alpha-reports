"use strict";
var CustomerLevel;
(function (CustomerLevel) {
    CustomerLevel[CustomerLevel["Super"] = 4] = "Super";
    CustomerLevel[CustomerLevel["Master"] = 3] = "Master";
    CustomerLevel[CustomerLevel["Agent"] = 2] = "Agent";
})(CustomerLevel || (CustomerLevel = {}));
var BaseReportView = /** @class */ (function () {
    function BaseReportView($) {
        this.$ = $;
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl, width, height) {
        this.$("#icon-report-legend").click(function () {
            top.window["popupManager"].open(legendUrl, undefined, width, height, "modal-mobile-friendly");
        });
    };
    BaseReportView.prototype.isNotUndefined = function (value) {
        return !this.isUndefined(value);
    };
    BaseReportView.prototype.isUndefined = function (value) {
        return typeof value === "undefined";
    };
    BaseReportView.prototype.formatInt = function (value) {
        return $.fn.dataTable.render.number(",", ".", 0).display(value);
    };
    BaseReportView.prototype.formatDecimal = function (value) {
        var text = $.fn.dataTable.render.number(",", ".", 2).display(Math.round(Math.abs(value) * 100) / 100);
        return value >= 0 ? text : "-" + text;
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map