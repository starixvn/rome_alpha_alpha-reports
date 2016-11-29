/// <reference path="../core.ts" />

"use strict";
class ReportTooltipOption {
    constructor(
        public tooltipId: string = "tooltip",
        public custId: number = 0) {
    }
}

class ReportTooltip {
    $: any;

    constructor($: any) {
        this.$ = $;
    }

    tooltipCookieId = "TooltipShowCount";
    timeToShowTooltip = 5000;

    show = (tooltipOption: ReportTooltipOption = new ReportTooltipOption()) => {
        var tooltipCookieId = this.tooltipCookieId + "-" + tooltipOption.tooltipId + "-" + tooltipOption.custId;
        var tooltipShowCount = alphaReports.localStorage.get(tooltipCookieId) || 0;
        if (tooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            return;
        }

        var tooltipElement = this.$("#" + tooltipOption.tooltipId);

        tooltipElement.tooltip({ placement: 'bottom' });
        tooltipElement.tooltip("show");
        alphaReports.localStorage.set(tooltipCookieId, tooltipShowCount + 1);

        setTimeout(function () {
            tooltipElement.tooltip("hide");
        }, this.timeToShowTooltip);
    };
}
