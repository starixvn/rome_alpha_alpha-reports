"use strict";

interface DateRangePickerJQueryOption {
    maxDays?: number,
    onClosed: Function
}

class DateRangePickerJQueryComponent {
    private dateFormat = "MM/DD/YYYY";
    private locale = {
        "th-TH": "th",
        "vi-VN": "vi",
        "zh-CN": "cn",
        "zh-TW": "tc"
    };
    private fromDate: string;
    private toDate: string;

    render(options: DateRangePickerJQueryOption) {
        const dateRangePicker = $("#daterange-picker");
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        const customShortcuts = this.buildCustomShortcuts(dateRangePicker);
        let defaults = {
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
            .dateRangePicker($.extend({}, defaults, options)).bind("datepicker-change", (_, obj) => {
                this.fromDate = moment(obj.date1).format(this.dateFormat);
                this.toDate = moment(obj.date2).format(this.dateFormat);
            })
            .bind("datepicker-closed", () => options.onClosed(this.getDateRangeValues()))
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    }

    private buildCustomShortcuts($dateRangePicker: JQuery) {
        const minDate = moment($dateRangePicker.data("mindate"), this.dateFormat);
        const today = moment($dateRangePicker.data("today"), this.dateFormat);
        const todayValue = today.toDate();
        const yesterday = moment(today).subtract(1, "d");
        const yesterdayValue = yesterday.toDate();
        const thisWeekStartOfDay = moment(today).startOf("isoWeek");
        const lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
        const thisMonthStartOfDay = moment(today).startOf("M");
        const lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        const lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate();
        const last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M");
        const realLastMonthStartOfDay =
            lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        const realLast3MonthsStartOfDay =
            last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();

        const shortcuts = [];
        this.addShortcut($dateRangePicker, shortcuts, "labeltoday", todayValue, todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labelyesterday", yesterdayValue, yesterdayValue);
        this.addShortcut(
            $dateRangePicker,
            shortcuts,
            "labelthisweek",
            thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(),
            todayValue
        );
        this.addShortcut(
            $dateRangePicker,
            shortcuts,
            "labellastweek",
            lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(),
            moment(thisWeekStartOfDay).subtract(1, "d").toDate()
        );
        this.addShortcut(
            $dateRangePicker,
            shortcuts,
            "labelthismonth",
            thisMonthStartOfDay.diff(minDate) >= 0 ? thisMonthStartOfDay.toDate() : minDate.toDate(),
            todayValue
        );
        this.addShortcut($dateRangePicker, shortcuts, "labellastmonth", realLastMonthStartOfDay, lastMonthEndOfDay);
        this.addShortcut(
            $dateRangePicker,
            shortcuts,
            "labelsincelastmonth",
            lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
            todayValue
        );
        this.addShortcut($dateRangePicker, shortcuts, "labellast3months", realLast3MonthsStartOfDay, lastMonthEndOfDay);

        return shortcuts;
    }

    private addShortcut($dateRangePicker, shortcuts, labelName, startDate, endDate) {
        const name = $dateRangePicker.data(labelName);
        name &&
            shortcuts.push({
                name: name,
                dates: function () {
                    return [startDate, endDate];
                }
            });
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
