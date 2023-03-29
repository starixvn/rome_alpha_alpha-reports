"use strict";

class DateRangePickerJQueryOption {
    constructor(
        public onClosed = Function(),
        public selectorId = "daterange-picker",
        public dateFormat = "MM/DD/YYYY") {
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
}

class DateRangePickerJQueryComponent {
    private $: JQueryStatic;
    private fromDate: string;
    private toDate: string;
    private isHistoricReport = false;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: DateRangePickerJQueryOption) {
        this.renderDateRangePicker(this.$("#" + options.selectorId), options);
    }

    private renderDateRangePicker(dateRangePicker: JQuery, options: DateRangePickerJQueryOption, raiseOnClosedEvent = false) {
        const olderThan3Months = dateRangePicker.data("label-olderthan3months");
        const backToCurrentReport = dateRangePicker.data("label-backtocurrentreport");

        dateRangePicker
            .dateRangePicker(this.getDateRangeOptions(dateRangePicker, options))
            .bind("datepicker-change", (_, obj) => {
                if (obj.value === olderThan3Months || obj.value === backToCurrentReport) {
                    this.isHistoricReport = obj.value === olderThan3Months;
                    dateRangePicker.off();
                    dateRangePicker.data("dateRangePicker").destroy();
                    this.renderDateRangePicker(dateRangePicker, options, true);
                } else {
                    this.fromDate = moment(obj.date1).format(options.dateFormat);
                    this.toDate = moment(obj.date2).format(options.dateFormat);
                }
            }).bind("datepicker-closed", function () {
                options.onClosed();
            }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);

        raiseOnClosedEvent && options.onClosed();
    }

    private getDateRangeOptions(dateRangePicker: JQuery, options: DateRangePickerJQueryOption) {
        let dateRangeOptions: any;
        if (this.isHistoricReport) {
            const today = moment(dateRangePicker.data("currentdate"));
            this.fromDate = moment(today).startOf("M").format(options.dateFormat);
            this.toDate = today.format(options.dateFormat);
            dateRangeOptions = {
                startDate: dateRangePicker.data("historicmindate"),
                endDate: moment(today).endOf("M").format(options.dateFormat),
                customShortcuts: this.getHistoricPredefinedRanges(dateRangePicker),
                //batchMode: "month",
                //monthSelect: true,
                //yearSelect: true,
            };
        } else {
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
    }

    private getPredefinedRanges(dateRangePicker: JQuery) {
        const today = moment(dateRangePicker.data("currentdate"));
        const todayValue = today.toDate();
        const yesterday = moment(today).subtract(1, "d");
        const thisWeekStartOfDay = moment(today).startOf("isoWeek");
        const lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M")

        const ranges = [
            {
                name: dateRangePicker.data("label-today"),
                dates: function () {
                    return [todayValue, todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-yesterday"),
                dates: function () {
                    const yesterdayValue = yesterday.toDate();
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
    }

    private getHistoricPredefinedRanges(dateRangePicker: JQuery) {
        return [{
            name: dateRangePicker.data("label-backtocurrentreport"),
            dates: function () {
                return [undefined, undefined];
            }
        }];
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate, isHistoricReport: this.isHistoricReport };
    }
}
