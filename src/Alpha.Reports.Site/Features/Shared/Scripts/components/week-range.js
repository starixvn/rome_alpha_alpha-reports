"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, selectorId, textFromDateFormat, textToDateFormat, valueDateFormat) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "labelWeekRange"; }
        if (textFromDateFormat === void 0) { textFromDateFormat = "DD"; }
        if (textToDateFormat === void 0) { textToDateFormat = "DD MMM"; }
        if (valueDateFormat === void 0) { valueDateFormat = "MM/DD/YYYY"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.textFromDateFormat = textFromDateFormat;
        this.textToDateFormat = textToDateFormat;
        this.valueDateFormat = valueDateFormat;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.textFromDateFormat = textFromDateFormat;
        this.textToDateFormat = textToDateFormat;
        this.valueDateFormat = valueDateFormat;
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
            var fromDateValue = startDateOrEndDateStartOfWeek.format(this.weekRangeOption.valueDateFormat);
            var toDateValue = endDate.format(this.weekRangeOption.valueDateFormat);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(this.weekRangeOption.textToDateFormat)
                : startDateOrEndDateStartOfWeek.format(this.weekRangeOption.textFromDateFormat) + " - " + endDate.format(this.weekRangeOption.textToDateFormat);
            weekRange.push("<a href=\"javascript:;\" class=\"label-week\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = this.moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map