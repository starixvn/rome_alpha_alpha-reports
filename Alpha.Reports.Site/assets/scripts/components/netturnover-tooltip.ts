/// <reference path="../core.ts" />

"use strict";

class NetTurnoverTooltipComponent {
    options = {
        tooltipId: "netturnovertooltip",
        closeTooltipId: "close-netturnover-tooltip"
    };

    cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
    timeToShowNetTurnoverTooltip = 10000;

    netTurnoverTooltip: any;
    closeNetTurnoverTooltipButton: any;

    render = (options: any = {}) => {
        var pickerOptions = $.extend({}, this.options, options);

        this.netTurnoverTooltip = $("#" + pickerOptions.tooltipId);
        this.closeNetTurnoverTooltipButton = $("#" + pickerOptions.closeTooltipId);

        var $this = this;
        var netTurnoverTooltipShowCount = alphaReports.localStorage.get(this.cookieNetTurnoverTooltip) || 0;

        if (netTurnoverTooltipShowCount &&
            netTurnoverTooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            this.netTurnoverTooltip.remove();
        } else {
            this.positionNetTurnoverTooltip();
            this.netTurnoverTooltip.modal({ show: true, backdrop: false });

            this.closeNetTurnoverTooltipButton.on("click", function () {
                alphaReports.localStorage.set($this.cookieNetTurnoverTooltip, alphaReports.maxTimesToShowTooltip);
                $this.closeNetTurnoverTooltip();
            });

            alphaReports.localStorage.set($this.cookieNetTurnoverTooltip, netTurnoverTooltipShowCount + 1);
            setTimeout(function () {
                $this.closeNetTurnoverTooltip();
            }, this.timeToShowNetTurnoverTooltip);
        }

        this.registerEvents();

        return this.netTurnoverTooltip;
    };
    positionNetTurnoverTooltip = () => {
        var netTurnoverPosition = $(".col-netturnover-header").position();
        var rightPosition = $(".col-netturnover-header").width();

        this.netTurnoverTooltip.css({
            left: netTurnoverPosition.left + rightPosition - 10,
            top: netTurnoverPosition.top - 22
        });
    };
    registerEvents = () => {
        var $this = this;

        $(window).resize(function () {
            $this.positionNetTurnoverTooltip();
        });
    };
    closeNetTurnoverTooltip = () => {
        this.netTurnoverTooltip.modal("hide");
    };
}

const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent();