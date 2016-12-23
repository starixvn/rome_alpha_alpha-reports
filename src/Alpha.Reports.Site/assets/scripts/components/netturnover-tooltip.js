/// <reference path="../core.ts" />
"use strict";
var NetTurnoverTooltipOption = (function () {
    function NetTurnoverTooltipOption(tooltipId, closeTooltipId) {
        if (tooltipId === void 0) { tooltipId = "netturnovertooltip"; }
        if (closeTooltipId === void 0) { closeTooltipId = "close-netturnover-tooltip"; }
        this.tooltipId = tooltipId;
        this.closeTooltipId = closeTooltipId;
    }
    return NetTurnoverTooltipOption;
}());
var NetTurnoverTooltipComponent = (function () {
    function NetTurnoverTooltipComponent($) {
        var _this = this;
        this.options = new NetTurnoverTooltipOption();
        this.cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
        this.timeToShowNetTurnoverTooltip = 10000;
        this.render = function (options) {
            if (options === void 0) { options = new NetTurnoverTooltipOption(); }
            var pickerOptions = _this.$.extend({}, _this.options, options);
            var netTurnoverTooltip = _this.$("#" + pickerOptions.tooltipId);
            var closeNetTurnoverTooltipButton = _this.$("#" + pickerOptions.closeTooltipId);
            var self = _this;
            var netTurnoverTooltipShowCount = parseInt(alphaReports.localStorage.get(_this.cookieNetTurnoverTooltip), 0) || 0;
            if (netTurnoverTooltipShowCount &&
                netTurnoverTooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
                netTurnoverTooltip.remove();
            }
            else {
                _this.positionNetTurnoverTooltip(netTurnoverTooltip);
                netTurnoverTooltip.modal({ show: true, backdrop: false });
                closeNetTurnoverTooltipButton.on("click", function () {
                    alphaReports.localStorage.set(self.cookieNetTurnoverTooltip, alphaReports.maxTimesToShowTooltip);
                    self.closeNetTurnoverTooltip(netTurnoverTooltip);
                });
                alphaReports.localStorage.set(self.cookieNetTurnoverTooltip, netTurnoverTooltipShowCount + 1);
                setTimeout(function () {
                    self.closeNetTurnoverTooltip(netTurnoverTooltip);
                }, _this.timeToShowNetTurnoverTooltip);
            }
            _this.registerEvents(netTurnoverTooltip);
            return netTurnoverTooltip;
        };
        this.positionNetTurnoverTooltip = function (netTurnoverTooltip) {
            if (!netTurnoverTooltip) {
                return;
            }
            var netTurnoverHeader = _this.$(".col-netturnover-header");
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
                }
                else {
                    netTurnoverTooltip.css({
                        left: netTurnoverPosition.left + netTurnoverHeader.outerWidth() - 16,
                        right: "auto",
                        bottom: "auto",
                        top: netTurnoverPosition.top - 22,
                        width: "auto"
                    });
                }
            }
        };
        this.registerEvents = function (netTurnoverTooltip) {
            var self = _this;
            _this.$(window).resize(function () {
                self.positionNetTurnoverTooltip(netTurnoverTooltip);
            });
        };
        this.closeNetTurnoverTooltip = function (netTurnoverTooltip) {
            netTurnoverTooltip.modal("hide");
        };
        this.$ = $;
    }
    return NetTurnoverTooltipComponent;
}());
var netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
//# sourceMappingURL=netturnover-tooltip.js.map