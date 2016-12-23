/// <reference path="../core.ts" />
"use strict";
var ReportTooltipOption = (function () {
    function ReportTooltipOption(tooltipId, custId) {
        if (tooltipId === void 0) { tooltipId = "tooltip"; }
        if (custId === void 0) { custId = 0; }
        this.tooltipId = tooltipId;
        this.custId = custId;
    }
    return ReportTooltipOption;
}());
var ReportTooltip = (function () {
    function ReportTooltip($) {
        var _this = this;
        this.tooltipCookieId = "TooltipShowCount";
        this.timeToShowTooltip = 5000;
        this.show = function (tooltipOption) {
            if (tooltipOption === void 0) { tooltipOption = new ReportTooltipOption(); }
            var tooltipCookieId = _this.tooltipCookieId + "-" + tooltipOption.tooltipId + "-" + tooltipOption.custId;
            var tooltipShowCount = parseInt(alphaReports.localStorage.get(tooltipCookieId), 0) || 0;
            if (tooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
                return;
            }
            var tooltipElement = _this.$("#" + tooltipOption.tooltipId);
            tooltipElement.tooltip({ placement: "bottom" });
            tooltipElement.tooltip("show");
            alphaReports.localStorage.set(tooltipCookieId, tooltipShowCount + 1);
            setTimeout(function () {
                tooltipElement.tooltip("hide");
                this.$(document).off("click.tooltip");
            }, _this.timeToShowTooltip);
            _this.$(document).off("click.tooltip").on("click.tooltip", function () {
                tooltipElement.tooltip("hide");
            });
        };
        this.$ = $;
    }
    return ReportTooltip;
}());
//# sourceMappingURL=tooltip.js.map