"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onChange, selectorId, dateFormat) {
        if (onChange === void 0) { onChange = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD/YYYY"; }
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent($) {
        this.$ = $;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = this.$("#" + options.selectorId);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        var labelOlderThan3Months = dateRangePicker.data("label-olderthan3months");
        $("#daterange-picker").dateRangePicker({
            autoClose: true,
            format: options.dateFormat,
            separator: " - ",
            language: "auto",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: true,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            //batchMode: "month",
            duration: 0,
            singleMonth: true,
            //customTopBar: "",
            //showTopbar: false,
            //monthSelect: false,
            //yearSelect: false,
            setValue: function (s) {
                var $this = $(this);
                if (!$this.is(":disabled") && s !== $this.val()) {
                    $this.val(s);
                }
            }
        }).bind("datepicker-change", function (event, obj) {
            _this.fromDate = moment(obj.date1).format(options.dateFormat);
            _this.toDate = moment(obj.date2).format(options.dateFormat);
            options.onChange();
        }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var today = moment(dateRangePicker.data("currentdate"));
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var ranges = [
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
                    return [thisWeekStartOfDay.toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    return [moment(thisWeekStartOfDay).subtract(1, "w").toDate(), moment(thisWeekStartOfDay).subtract(1, "d").toDate()];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    return [moment(today).startOf("M").toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastmonth"),
                dates: function () {
                    return [lastMonthStartOfDay.toDate(), moment(lastMonthStartOfDay).endOf("M").toDate()];
                }
            },
            {
                name: dateRangePicker.data("label-sincelastmonth"),
                dates: function () {
                    return [lastMonthStartOfDay.toDate(), todayValue];
                }
            }
        ];
        if (dateRangePicker.data("isshowhistory") === 1) {
            ranges.push({
                name: dateRangePicker.data("label-olderthan3months"),
                dates: function () {
                    return [undefined, undefined];
                }
            });
        }
        return ranges;
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map