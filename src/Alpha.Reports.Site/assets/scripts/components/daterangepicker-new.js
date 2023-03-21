"use strict";
var DateRangePickerNewComponent = /** @class */ (function () {
    function DateRangePickerNewComponent($) {
        this.$ = $;
    }
    DateRangePickerNewComponent.prototype.render = function () {
        var _this = this;
        var dateRangePicker = this.$("#daterange-picker");
        var localeFormat = "MM/DD/YYYY";
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        return $("#daterange-picker").daterangepicker({
            autoApply: true,
            ranges: this.getRangeDates(dateRangePicker),
            locale: {
                format: localeFormat,
                customRangeLabel: dateRangePicker.data("label-specificdate"),
            },
            linkedCalendars: false,
            alwaysShowCalendars: true,
            startDate: this.fromDate,
            endDate: this.toDate,
            minDate: dateRangePicker.data("mindate"),
            maxDate: dateRangePicker.data("maxdate"),
        }, function (start, end, label) {
            _this.fromDate = start.format(localeFormat);
            _this.toDate = end.format(localeFormat);
            console.log("New date range selected: " + start.format(localeFormat) + " to " + end.format(localeFormat) + " (predefined range: " + label + ")");
        });
    };
    DateRangePickerNewComponent.prototype.getRangeDates = function (dateRangePicker) {
        var today = moment(dateRangePicker.data("currentdate"));
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var ranges = {};
        ranges[dateRangePicker.data("label-today")] = [today, today];
        ranges[dateRangePicker.data("label-yesterday")] = [yesterday, yesterday];
        ranges[dateRangePicker.data("label-thisweek")] = [thisWeekStartOfDay, today];
        ranges[dateRangePicker.data("label-lastweek")] = [moment(thisWeekStartOfDay).subtract(1, "w"), moment(thisWeekStartOfDay).subtract(1, "d")];
        ranges[dateRangePicker.data("label-thismonth")] = [moment(today).startOf("M"), today];
        ranges[dateRangePicker.data("label-lastmonth")] = [lastMonthStartOfDay, moment(lastMonthStartOfDay).endOf("M")];
        ranges[dateRangePicker.data("label-sincelastmonth")] = [lastMonthStartOfDay, today];
        return ranges;
    };
    DateRangePickerNewComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerNewComponent;
}());
//# sourceMappingURL=daterangepicker-new.js.map