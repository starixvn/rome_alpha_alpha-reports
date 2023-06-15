"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl) {
        $.get(legendUrl, function (html) {
            $("body").append(html);
            $("#icon-report-legend").click(function () {
                $("#report-legend").modal({ show: true, backdrop: false });
            });
        });
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map