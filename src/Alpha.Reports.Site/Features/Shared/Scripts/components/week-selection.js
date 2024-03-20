"use strict";
var WeekSelectionOption = /** @class */ (function () {
    function WeekSelectionOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "week-selection"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return WeekSelectionOption;
}());
var WeekSelectionComponent = /** @class */ (function () {
    function WeekSelectionComponent() {
    }
    WeekSelectionComponent.prototype.init = function (options) {
        this.weekSelectionOption = options;
        this.bindEvents();
    };
    WeekSelectionComponent.prototype.bindEvents = function () {
        var _this = this;
        this.weekRangeContainer = $("#" + this.weekSelectionOption.selectorId);
        var labelWeeks = this.weekRangeContainer.find(".label-week:not(.disabled)");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekSelectionOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekSelectionComponent.prototype.getActiveWeek = function () {
        var activeWeek = this.weekRangeContainer.find(".label-week.active");
        if (activeWeek.length) {
            var firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data("fromdate"),
                weekToDate: firstActiveWeek.data("todate"),
            };
        }
        return {};
    };
    WeekSelectionComponent.prototype.hide = function () {
        this.weekRangeContainer.addClass("hide");
    };
    WeekSelectionComponent.prototype.show = function () {
        this.weekRangeContainer.removeClass("hide");
    };
    return WeekSelectionComponent;
}());
//# sourceMappingURL=week-selection.js.map