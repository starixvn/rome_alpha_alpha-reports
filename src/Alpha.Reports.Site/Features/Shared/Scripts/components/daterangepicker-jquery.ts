"use strict";

class DateRangePickerJQueryOption {
    constructor(
        public onChange = Function(),
        public selectorId = "daterange-picker",
        public dateFormat = "MM/DD/YYYY") {
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
}

class DateRangePickerJQueryComponent {
    private $: JQueryStatic;
    private fromDate: string;
    private toDate: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: DateRangePickerJQueryOption) {
        const dateRangePicker = this.$("#" + options.selectorId);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        const labelOlderThan3Months = dateRangePicker.data("label-olderthan3months");

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
                const $this = $(this);
                if (!$this.is(":disabled") && s !== $this.val()) {
                    $this.val(s);
                }
            }
        }).bind("datepicker-change", (event, obj) => {
            this.fromDate = moment(obj.date1).format(options.dateFormat);
            this.toDate = moment(obj.date2).format(options.dateFormat);
            options.onChange();
        }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
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

        if (dateRangePicker.data("isshowhistory") === 1) {
            ranges.push({
                name: dateRangePicker.data("label-olderthan3months"),
                dates: function () {
                    return [undefined, undefined];
                }
            });
        }

        return ranges;
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
