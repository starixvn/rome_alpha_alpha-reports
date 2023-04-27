"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "week-range-container"; }
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
    return WeekRangeOption;
}());
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent() {
    }
    WeekRangeComponent.prototype.init = function (options) {
        this.weekRangeOption = options;
        this.weekRangeContainer = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (dateRange, activeWeekRange) {
        var _this = this;
        moment.locale(this.weekRangeOption.language);
        this.weekRangeContainer.html(this.buildWeekRangeHtml(dateRange, activeWeekRange));
        var labelWeeks = this.weekRangeContainer.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (dateRange, activeWeekRange) {
        var weekRange = [];
        var startDate = moment(dateRange.fromDate);
        var endDate = moment(dateRange.toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            var activeClassName = activeWeekRange
                && activeWeekRange.weekFromDate === fromDateValue
                && activeWeekRange.weekToDate === toDateValue ? " active" : "";
            weekRange.push("<a href=\"javascript:;\" class=\"label-week" + activeClassName + "\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    WeekRangeComponent.prototype.getActiveWeekRange = function () {
        var activeWeekRange = this.weekRangeContainer.find(".label-week.active");
        if (activeWeekRange.length) {
            var firstActiveWeekRange = activeWeekRange.eq(0);
            return {
                weekFromDate: firstActiveWeekRange.data("fromdate"),
                weekToDate: firstActiveWeekRange.data("todate"),
            };
        }
        return {};
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map