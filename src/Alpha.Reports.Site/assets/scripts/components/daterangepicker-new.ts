"use strict";
class DateRangePickerNewComponent {
    private $: JQueryStatic;
    private fromDate: string;
    private toDate: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render() {
        const dateRangePicker = this.$("#daterange-picker");
        const localeFormat = "MM/DD/YYYY";
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
            drops: "auto"
        }, (start, end, label) => {
            this.fromDate = start.format(localeFormat);
            this.toDate = end.format(localeFormat);
            console.log(`New date range selected: ${start.format(localeFormat)} to ${end.format(localeFormat)} (predefined range: ${label})`);
        });
    }

    private getRangeDates(dateRangePicker: JQuery) {
        const today = moment(dateRangePicker.data("currentdate"));
        const yesterday = today.clone().subtract(1, "d");
        const thisWeekStartOfDay = today.clone().startOf("isoWeek");
        const lastMonthStartOfDay = today.clone().subtract(1, "M").startOf("M")

        const ranges = {};
        ranges[dateRangePicker.data("label-today")] = [today, today];
        ranges[dateRangePicker.data("label-yesterday")] = [yesterday, yesterday];
        ranges[dateRangePicker.data("label-thisweek")] = [thisWeekStartOfDay, today];
        ranges[dateRangePicker.data("label-lastweek")] = [thisWeekStartOfDay.clone().subtract(7, "d"), thisWeekStartOfDay.clone().subtract(1, "d")];
        ranges[dateRangePicker.data("label-thismonth")] = [today.clone().startOf("M"), today];
        ranges[dateRangePicker.data("label-lastmonth")] = [lastMonthStartOfDay, lastMonthStartOfDay.clone().endOf("M")];
        ranges[dateRangePicker.data("label-sincelastmonth")] = [lastMonthStartOfDay, today];

        return ranges;
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}