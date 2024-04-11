"use strict";

class DateRangePickerJQueryOption {
    constructor(
        public onClosed = Function(),
        public language = "default",
        public selectorId = "daterange-picker") {
        this.onClosed = onClosed;
        this.language = language;
        this.selectorId = selectorId;
    }
}

class DateRangePickerJQueryComponent {
    private fromDate: string;
    private toDate: string;

    render(options: DateRangePickerJQueryOption) {
        const dateRangePicker = $("#" + options.selectorId);
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
            }).bind("datepicker-change", (_, obj) => {
                this.fromDate = moment(obj.date1).format(alphaReports.format.shortDateWithSlash_DatePicker);
                this.toDate = moment(obj.date2).format(alphaReports.format.shortDateWithSlash_DatePicker);
            })
            .bind("datepicker-closed", () => options.onClosed(this.getDateRangeValues()))
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    }

    private getPredefinedRanges(dateRangePicker: JQuery) {
        const minDate = moment(dateRangePicker.data("mindate"));
        const today = moment(dateRangePicker.data("currentdate"));
        const todayValue = today.toDate();
        const yesterday = moment(today).subtract(1, "d");
        const thisWeekStartOfDay = moment(today).startOf("isoWeek");
        const lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M")
        const lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate()
        const last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M")

        const realLastMonthStartOfDay = lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        const realLast3MonthsStartOfDay = last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();

        const shortcuts = [
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
                    return [
                        thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    const lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
                    return [
                        lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(),
                        moment(thisWeekStartOfDay).subtract(1, "d").toDate()
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    const thisMonthStartOfDay = moment(today).startOf("M");
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
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
