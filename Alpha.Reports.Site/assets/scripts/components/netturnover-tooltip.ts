/// <reference path="../core.ts" />

"use strict";

class NetTurnoverTooltipComponent {
    $: any;

    constructor($: any) {
        this.$ = $;
    }

    options = {
        tooltipId: "netturnovertooltip",
        closeTooltipId: "close-netturnover-tooltip"
    };

    cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
    timeToShowNetTurnoverTooltip = 10000;

    render = (options: any = {}) => {
        var pickerOptions = this.$.extend({}, this.options, options);

        var netTurnoverTooltip = this.$("#" + pickerOptions.tooltipId);
        var closeNetTurnoverTooltipButton = this.$("#" + pickerOptions.closeTooltipId);

        var $this = this;
        var netTurnoverTooltipShowCount = alphaReports.localStorage.get(this.cookieNetTurnoverTooltip) || 0;

        if (netTurnoverTooltipShowCount &&
            netTurnoverTooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            netTurnoverTooltip.remove();
        } else {
            this.positionNetTurnoverTooltip(netTurnoverTooltip);
            netTurnoverTooltip.modal({ show: true, backdrop: false });

            closeNetTurnoverTooltipButton.on("click", function () {
                alphaReports.localStorage.set($this.cookieNetTurnoverTooltip, alphaReports.maxTimesToShowTooltip);
                $this.closeNetTurnoverTooltip(netTurnoverTooltip);
            });

            alphaReports.localStorage.set($this.cookieNetTurnoverTooltip, netTurnoverTooltipShowCount + 1);
            setTimeout(function () {
                $this.closeNetTurnoverTooltip(netTurnoverTooltip);
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
        var $this = this;

        this.$(window).resize(function () {
            $this.positionNetTurnoverTooltip(netTurnoverTooltip);
        });
    };
    closeNetTurnoverTooltip = (netTurnoverTooltip: any) => {
        netTurnoverTooltip.modal("hide");
    };
}

const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);