"use strict";
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    WeekRangeComponent.prototype.render = function (fromDate, toDate, onClick) {
        var _this = this;
        var weekRange = [];
        var TextDateFormat = "MMM YYYYY";
        var ValueDateFormat = "MM/DD/YYYYY";
        var fromDateStartOfWeek = this.moment(fromDate).startOf("isoWeek");
        var toDateStartOfWeek = this.moment(toDate).startOf("isoWeek");
        if (!fromDateStartOfWeek.isSame(toDateStartOfWeek)) {
            var startDate = moment(fromDate);
            var endDate = moment(toDate);
            while (endDate.diff(startDate, "d") >= 0) {
                var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
                var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
                var fromDateValue = startDateOrEndDateStartOfWeek.format(ValueDateFormat);
                var toDateValue = endDate.format(ValueDateFormat);
                var label = startDateOrEndDateStartOfWeek.format(TextDateFormat) + " - " + endDate.format(TextDateFormat);
                weekRange.push("<span class=\"label label-default label-week-range\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</span>");
                endDate = moment(endDateStartOfWeek).subtract(1, "d");
            }
        }
        this.$("#labelWeekRange")
            .html(weekRange.join(""))
            .find(".label-week-range")
            .off()
            .click(function (e) {
            var $this = _this.$(e.target);
            onClick && onClick($this.data("fromdate"), $this.data("todate"));
        });
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map