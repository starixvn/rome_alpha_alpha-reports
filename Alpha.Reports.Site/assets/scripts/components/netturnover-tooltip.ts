/// <reference path="../core.ts" />

"use strict";
class NetTurnoverTooltipOption {
    constructor(
        public tooltipId: string = "netturnovertooltip",
        public closeTooltipId: string = "close-netturnover-tooltip") {
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
        var pickerOptions = <NetTurnoverTooltipOption>this.$.extend({}, this.options, options);

        var netTurnoverTooltip = this.$("#" + pickerOptions.tooltipId);
        var closeNetTurnoverTooltipButton = this.$("#" + pickerOptions.closeTooltipId);

        var self = this;
        var netTurnoverTooltipShowCount = parseInt(alphaReports.localStorage.get(this.cookieNetTurnoverTooltip), 0) || 0;

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
        if (!netTurnoverTooltip) {
            return;
        }

        var netTurnoverHeader = this.$(".col-netturnover-header");
        var netTurnoverPosition = netTurnoverHeader.position();

        if (netTurnoverPosition.top) {
            if (alphaReports.screenSmallThan768()) {
                netTurnoverTooltip.css({
                    left: 0,
                    right: 0,
                    bottom: "auto",
                    top: netTurnoverPosition.top - 22,
                    width: "auto"
                });
            } else {
                netTurnoverTooltip.css({
                    left: netTurnoverPosition.left + netTurnoverHeader.outerWidth() - 16, // Next to net turnover label
                    right: "auto",
                    bottom: "auto",
                    top: netTurnoverPosition.top - 22, // verical align: middle with header
                    width: "auto"
                });
            }
        }
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