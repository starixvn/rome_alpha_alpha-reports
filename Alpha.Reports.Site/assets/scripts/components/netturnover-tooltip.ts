/// <reference path="../core.ts" />

"use strict";
class NetTurnoverTooltipOption {
    tooltipId: string;
    closeTooltipId: string;

    constructor(tooltipId: string = "netturnovertooltip",
        closeTooltipId: string = "close-netturnover-tooltip") {
        this.tooltipId = tooltipId;
        this.closeTooltipId = closeTooltipId;
    }
}

class NetTurnoverTooltipComponent {
    $: any;

    options = new NetTurnoverTooltipOption();

    constructor($: any) {
        this.$ = $;
    }

    cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
    timeToShowNetTurnoverTooltip = 10000;

    render = (options: NetTurnoverTooltipOption = new NetTurnoverTooltipOption()) => {
        var pickerOptions = this.$.extend({}, this.options, options);

        var netTurnoverTooltip = this.$("#" + pickerOptions.tooltipId);
        var closeNetTurnoverTooltipButton = this.$("#" + pickerOptions.closeTooltipId);

        var self = this;
        var netTurnoverTooltipShowCount = alphaReports.localStorage.get(this.cookieNetTurnoverTooltip) || 0;

        if (netTurnoverTooltipShowCount &&
            netTurnoverTooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            netTurnoverTooltip.remove();
        } else {
            this.positionNetTurnoverTooltip(netTurnoverTooltip);
            netTurnoverTooltip.modal({ show: true, backdrop: false });

            closeNetTurnoverTooltipButton.on("click", function () {
                alphaReports.localStorage.set(self.cookieNetTurnoverTooltip, alphaReports.maxTimesToShowTooltip);
                self.closeNetTurnoverTooltip(netTurnoverTooltip);
            });

            alphaReports.localStorage.set(self.cookieNetTurnoverTooltip, netTurnoverTooltipShowCount + 1);
            setTimeout(function () {
                self.closeNetTurnoverTooltip(netTurnoverTooltip);
            }, this.timeToShowNetTurnoverTooltip);
        }

        this.registerEvents(netTurnoverTooltip);

        return netTurnoverTooltip;
    };
    positionNetTurnoverTooltip = (netTurnoverTooltip: any) => {
        var netTurnoverPosition = this.$(".col-netturnover-header").position();
        var rightPosition = this.$(".col-netturnover-header").width();

        netTurnoverTooltip.css({
            left: netTurnoverPosition.left + rightPosition - 10,
            top: netTurnoverPosition.top - 22
        });
    };
    registerEvents = (netTurnoverTooltip: any) => {
        var self = this;

        this.$(window).resize(function () {
            self.positionNetTurnoverTooltip(netTurnoverTooltip);
        });
    };
    closeNetTurnoverTooltip = (netTurnoverTooltip: any) => {
        netTurnoverTooltip.modal("hide");
    };
}

const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);