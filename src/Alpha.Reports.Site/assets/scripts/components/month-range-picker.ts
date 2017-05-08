/// <reference path="../core.ts" />

"use strict";

interface Jquery {
    monPickr(option: any): void;
    monPickr(methodName: string, format: any): string;
}

class MonthRangePickerOption {
    constructor(
        public fromSelectorId: string = "from-monthpicker",
        public toSelectorId: string = "to-monthpicker",
        public reportFormId: string = "report-form") {
    }
}

class MonthRangePickerComponent {
    $: JQueryStatic;
    $fromDate: JQuery;
    $toDate: JQuery;
    $dropdownFromMonth: JQuery;
    $dropdownToMonth: JQuery;

    options = new MonthRangePickerOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MonthRangePickerOption = new MonthRangePickerOption()) => {
        this.options = this.$.extend({}, this.options, options);
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");

        this.$dropdownFromMonth = this.renderDropdown(this.options.fromSelectorId);
        this.$dropdownToMonth = this.renderDropdown(this.options.toSelectorId);
    };

    renderDropdown = (dropdownId: string) => {
        var dropdownMonth = this.$("#" + dropdownId);

        var maxDate = dropdownMonth.data("maxdate");

        dropdownMonth.monPickr({
            longMonthsInYear: dropdownMonth.data("longmonths-inyear").split(","),
            maxDate: maxDate,
            minDate: dropdownMonth.data("mindate"),
            month: new Date(dropdownMonth.data("dateinmonth")),

            showDateRange: false,
            monthFormat: alphaReports.format.monthFormat,
            fromDayId: "ddl-fromday",
            toDayId: "ddl-today",
            fromDayLabel: "",
            toDayLabel: "-",
            sortDesc: true,
            log: this.logMessage,
            afterChangeMonth: this.onChange
        });

        return dropdownMonth;
    };

    logMessage = (msg: string) => {
        console.warn(msg);
    };

    onChange = () => {
        this.$fromDate.val(this.$dropdownFromMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker));
        this.$toDate.val(this.$dropdownToMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker));
    };

    isLeast3Month = () => {
        let fromDate = new Date(this.$fromDate.val());
        let toDate = new Date(this.$toDate.val());

        return this.getMonthDiff(fromDate, toDate) >= 2;
    };

    getMonthDiff = (fromDate: Date, toDate: Date) => {
        return toDate.getMonth() - fromDate.getMonth() + 12 * (toDate.getFullYear() - fromDate.getFullYear());
    };
}