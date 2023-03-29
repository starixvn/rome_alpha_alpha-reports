"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, selectorId, dateFormat) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD/YYYY"; }
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent($) {
        this.isHistoricReport = false;
        this.$ = $;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        this.renderDateRangePicker(this.$("#" + options.selectorId), options);
    };
    DateRangePickerJQueryComponent.prototype.renderDateRangePicker = function (dateRangePicker, options, raiseOnClosedEvent) {
        var _this = this;
        if (raiseOnClosedEvent === void 0) { raiseOnClosedEvent = false; }
        var olderThan3Months = dateRangePicker.data("label-olderthan3months");
        var backToCurrentReport = dateRangePicker.data("label-backtocurrentreport");
        dateRangePicker
            .dateRangePicker(this.getDateRangeOptions(dateRangePicker, options))
            .bind("datepicker-change", function (_, obj) {
            if (obj.value === olderThan3Months || obj.value === backToCurrentReport) {
                _this.isHistoricReport = obj.value === olderThan3Months;
                dateRangePicker.off();
                dateRangePicker.data("dateRangePicker").destroy();
                _this.renderDateRangePicker(dateRangePicker, options, true);
            }
            else {
                _this.fromDate = moment(obj.date1).format(options.dateFormat);
                _this.toDate = moment(obj.date2).format(options.dateFormat);
            }
        }).bind("datepicker-closed", function () {
            options.onClosed();
        }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
        raiseOnClosedEvent && options.onClosed();
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeOptions = function (dateRangePicker, options) {
        var dateRangeOptions;
        if (this.isHistoricReport) {
            var today = moment(dateRangePicker.data("currentdate"));
            this.fromDate = moment(today).startOf("M").format(options.dateFormat);
            this.toDate = today.format(options.dateFormat);
            dateRangeOptions = {
                startDate: dateRangePicker.data("historicmindate"),
                endDate: moment(today).endOf("M").format(options.dateFormat),
                customShortcuts: this.getHistoricPredefinedRanges(dateRangePicker),
                batchMode: "month",
            };
        }
        else {
            this.fromDate = dateRangePicker.data("fromdate");
            this.toDate = dateRangePicker.data("todate");
            dateRangeOptions = {
                startDate: dateRangePicker.data("mindate"),
                endDate: dateRangePicker.data("maxdate"),
                customShortcuts: this.getPredefinedRanges(dateRangePicker)
            };
        }
        return this.$.extend({
            format: options.dateFormat,
            separator: " - ",
            language: "en",
            showShortcuts: true,
            shortcuts: null,
            duration: 0,
            singleMonth: true
        }, dateRangeOptions);
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
        if (dateRangePicker.data("isshowhistoricreport") === 1) {
            ranges.push({
                name: dateRangePicker.data("label-olderthan3months"),
                dates: function () {
                    return [undefined, undefined];
                }
            });
        }
        return ranges;
    };
    DateRangePickerJQueryComponent.prototype.getHistoricPredefinedRanges = function (dateRangePicker) {
        return [{
                name: dateRangePicker.data("label-backtocurrentreport"),
                dates: function () {
                    return [undefined, undefined];
                }
            }];
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate, isHistoricReport: this.isHistoricReport };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map