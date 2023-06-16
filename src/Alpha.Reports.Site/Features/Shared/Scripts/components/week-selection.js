"use strict";
var WeekSelectionOption = /** @class */ (function () {
    function WeekSelectionOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "week-selection"; }
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
    return WeekSelectionOption;
}());
var WeekSelectionComponent = /** @class */ (function () {
    function WeekSelectionComponent() {
    }
    WeekSelectionComponent.prototype.init = function (options) {
        this.weekSelectionOption = options;
        this.weekRangeContainer = $("#" + options.selectorId);
    };
    WeekSelectionComponent.prototype.render = function (dateRange, activeWeekRange) {
        var _this = this;
        moment.locale(this.weekSelectionOption.language);
        this.weekRangeContainer.html(this.buildWeekRangeHtml(dateRange, activeWeekRange));
        var labelWeeks = this.weekRangeContainer.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekSelectionOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekSelectionComponent.prototype.buildWeekRangeHtml = function (dateRange, activeWeekRange) {
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