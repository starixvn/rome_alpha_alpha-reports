"use strict";

class BaseReportView {
    protected initReportLegend(legendUrl: string) {
        $.get(legendUrl, function (html) {
            $("body").append(html);
            $("#icon-report-legend").click(function () {
                $("#report-legend").modal({ show: true, backdrop: false });
            });
        });
    }
}