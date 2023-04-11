"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl, width, height) {
        $("#icon-report-legend").click(function () {
            top.window["popupManager"].open(typeof legendUrl === "function" ? legendUrl() : legendUrl, undefined, width, height, "modal-mobile-friendly");
        });
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map