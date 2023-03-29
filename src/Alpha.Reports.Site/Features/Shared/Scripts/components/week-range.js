"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "labelWeekRange"; }
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
    function WeekRangeComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    WeekRangeComponent.prototype.init = function (options) {
        this.weekRangeOption = options;
        this.labelWeekRange = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (formData) {
        var _this = this;
        this.moment.locale(this.weekRangeOption.language);
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        var labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = _this.$(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick($this.data("fromdate"), $this.data("todate"));
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (fromDate, toDate) {
        var weekRange = [];
        var startDate = this.moment(fromDate);
        var endDate = this.moment(toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = this.moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            weekRange.push("<a href=\"javascript:;\" class=\"label-week\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = this.moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map