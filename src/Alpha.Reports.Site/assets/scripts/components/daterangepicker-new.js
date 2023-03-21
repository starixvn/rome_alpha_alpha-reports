"use strict";
var DateRangePickerNewOption = /** @class */ (function () {
    function DateRangePickerNewOption(onChange, selectorId, localeFormat) {
        if (onChange === void 0) { onChange = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (localeFormat === void 0) { localeFormat = "MM/DD/YYYY"; }
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.localeFormat = localeFormat;
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.localeFormat = localeFormat;
    }
    return DateRangePickerNewOption;
}());
var DateRangePickerNewComponent = /** @class */ (function () {
    function DateRangePickerNewComponent($) {
        this.$ = $;
    }
    DateRangePickerNewComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = this.$("#" + options.selectorId);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        var labelOlderThan3months = dateRangePicker.data("label-olderthan3months");
        return $("#daterange-picker").daterangepicker({
            autoApply: true,
            ranges: this.getRangeDates(dateRangePicker),
            locale: {
                format: options.localeFormat,
                customRangeLabel: dateRangePicker.data("label-specificdate"),
            },
            linkedCalendars: false,
            alwaysShowCalendars: true,
            startDate: this.fromDate,
            endDate: this.toDate,
            minDate: dateRangePicker.data("mindate"),
            maxDate: dateRangePicker.data("maxdate"),
        }, function (start, end, label) {
            _this.fromDate = start.format(options.localeFormat);
            _this.toDate = end.format(options.localeFormat);
            options.onChange(_this.fromDate, _this.toDate, label === labelOlderThan3months);
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
        if (dateRangePicker.data("isshowhistory") === 1) {
            ranges[dateRangePicker.data("label-olderthan3months")] = [undefined, undefined];
        }
        return ranges;
    };
    DateRangePickerNewComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerNewComponent;
}());
//# sourceMappingURL=daterangepicker-new.js.map