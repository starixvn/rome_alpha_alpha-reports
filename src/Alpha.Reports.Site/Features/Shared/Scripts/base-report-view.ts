"use strict";

class BaseReportView {
    protected initReportLegend(legendUrl: string | Function, width: number, height: number) {
        $("#icon-report-legend").click(function () {
            top.window["popupManager"].open(
                typeof legendUrl === "function" ? legendUrl() : legendUrl,
                undefined,
                width,
                height,
                "modal-mobile-friendly")
        });
    }
}