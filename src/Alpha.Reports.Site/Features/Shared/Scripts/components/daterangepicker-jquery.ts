"use strict";

class DateRangePickerJQueryOption {
    constructor(
        public onClosed = Function(),
        public selectorId = "daterange-picker") {
        this.onClosed = onClosed;
        this.selectorId = selectorId;
    }
}

class DateRangePickerJQueryComponent {
    private fromDate: string;
    private toDate: string;

    render(options: DateRangePickerJQueryOption) {
        const dateRangePicker = $("#" + options.selectorId);
        const historicPivotDate = moment(dateRangePicker.data("historicpivotdate"));
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");

        dateRangePicker
            .dateRangePicker({
                format: alphaReports.format.shortDateWithSlash_DatePicker,
                separator: " - ",
                language: "en",
                startDate: dateRangePicker.data("mindate"),
                endDate: dateRangePicker.data("maxdate"),
                showShortcuts: true,
                shortcuts: null,
                customShortcuts: this.getPredefinedRanges(dateRangePicker),
                duration: 0,
                singleMonth: true,
                monthSelect: true,
                yearSelect: true
            }).bind("datepicker-change", (_, obj) => {
                const startDate = moment(obj.date1)
                const endDate = moment(obj.date2);
                if (endDate.isBefore(historicPivotDate) && endDate.diff(startDate, "d") > 30) {
                    alert("Out of date range: from and to is between 31 days.");
                    dateRangePicker.data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
                    return;
                }

                if (startDate.isBefore(historicPivotDate) && endDate.isSameOrAfter(historicPivotDate)) {
                    alert("Out of date range: gap between normal and history mode.");
                    dateRangePicker.data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
                    return;
                }

                this.fromDate = startDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
                this.toDate = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
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

        return [
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
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        moment(lastMonthStartOfDay).endOf("M").toDate()
                    ];
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
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
