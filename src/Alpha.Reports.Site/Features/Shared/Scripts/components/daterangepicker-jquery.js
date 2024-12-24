"use strict";
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
        this.dateFormat = "MM/DD/YYYY";
        this.locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc",
            "ja-JP": "ja"
        };
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#daterange-picker");
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        var customShortcuts = this.buildCustomShortcuts(dateRangePicker);
        var defaults = {
            format: this.dateFormat,
            separator: " - ",
            language: this.locale[dateRangePicker.data("language")] || "default",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: customShortcuts.length > 0,
            shortcuts: null,
            customShortcuts: customShortcuts,
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true
        };
        dateRangePicker
            .dateRangePicker($.extend({}, defaults, options)).bind("datepicker-change", function (_, obj) {
            _this.fromDate = moment(obj.date1).format(_this.dateFormat);
            _this.toDate = moment(obj.date2).format(_this.dateFormat);
        })
            .bind("datepicker-closed", function () { return options.onClosed(_this.getDateRangeValues()); })
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.buildCustomShortcuts = function ($dateRangePicker) {
        var minDate = moment($dateRangePicker.data("mindate"), this.dateFormat);
        var today = moment($dateRangePicker.data("today"), this.dateFormat);
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var yesterdayValue = yesterday.toDate();
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
        var thisMonthStartOfDay = moment(today).startOf("M");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate();
        var last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M");
        var realLastMonthStartOfDay = lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        var realLast3MonthsStartOfDay = last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();
        var shortcuts = [];
        this.addShortcut($dateRangePicker, shortcuts, "labeltoday", todayValue, todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labelyesterday", yesterdayValue, yesterdayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labelthisweek", thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellastweek", lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(), moment(thisWeekStartOfDay).subtract(1, "d").toDate());
        this.addShortcut($dateRangePicker, shortcuts, "labelthismonth", thisMonthStartOfDay.diff(minDate) >= 0 ? thisMonthStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellastmonth", realLastMonthStartOfDay, lastMonthEndOfDay);
        this.addShortcut($dateRangePicker, shortcuts, "labelsincelastmonth", lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellast3months", realLast3MonthsStartOfDay, lastMonthEndOfDay);
        return shortcuts;
    };
    DateRangePickerJQueryComponent.prototype.addShortcut = function ($dateRangePicker, shortcuts, labelName, startDate, endDate) {
        var name = $dateRangePicker.data(labelName);
        name &&
            shortcuts.push({
                name: name,
                dates: function () {
                    return [startDate, endDate];
                }
            });
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map