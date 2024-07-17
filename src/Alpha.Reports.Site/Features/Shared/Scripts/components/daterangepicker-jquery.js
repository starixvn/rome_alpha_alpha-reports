"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, language, selectorId) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (language === void 0) { language = "default"; }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#" + options.selectorId);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        dateRangePicker
            .dateRangePicker({
            format: alphaReports.format.shortDateWithSlash_DatePicker,
            separator: " - ",
            language: options.language,
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: true,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true,
            maxDays: 92
        }).bind("datepicker-change", function (_, obj) {
            _this.fromDate = moment(obj.date1).format(alphaReports.format.shortDateWithSlash_DatePicker);
            _this.toDate = moment(obj.date2).format(alphaReports.format.shortDateWithSlash_DatePicker);
        })
            .bind("datepicker-closed", function () { return options.onClosed(_this.getDateRangeValues()); })
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var minDate = moment(dateRangePicker.data("mindate"), alphaReports.format.shortDateWithSlash_DatePicker);
        var today = moment(dateRangePicker.data("currentdate"), alphaReports.format.shortDateWithSlash_DatePicker);
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate();
        var last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M");
        var realLastMonthStartOfDay = lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        var realLast3MonthsStartOfDay = last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();
        var shortcuts = [
            {
                name: dateRangePicker.data("label-today"),
                dates: function () {
                    return [todayValue, todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-yesterday"),
                dates: function () {
                    var yesterdayValue = yesterday.toDate();
                    return [yesterdayValue, yesterdayValue];
                }
            },
            {
                name: dateRangePicker.data("label-thisweek"),
                dates: function () {
                    return [
                        thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    var lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
                    return [
                        lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(),
                        moment(thisWeekStartOfDay).subtract(1, "d").toDate()
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    var thisMonthStartOfDay = moment(today).startOf("M");
                    return [
                        thisMonthStartOfDay.diff(minDate) >= 0 ? thisMonthStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-lastmonth"),
                dates: function () {
                    return [realLastMonthStartOfDay, lastMonthEndOfDay];
                }
            },
            {
                name: dateRangePicker.data("label-sincelastmonth"),
                dates: function () {
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            }
        ];
        if (realLastMonthStartOfDay !== realLast3MonthsStartOfDay) {
            shortcuts.push({
                name: dateRangePicker.data("label-last3months"),
                dates: function () {
                    return [realLast3MonthsStartOfDay, lastMonthEndOfDay];
                }
            });
        }
        return shortcuts;
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map