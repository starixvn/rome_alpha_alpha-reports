/// <reference path="../core.ts" />

"use strict";
class MonthPickerOption {
    selectorId: string;

    constructor(selectorId: string = "dropdown-month",
        longMonthsInYear: Array<string> = null) {

        this.selectorId = selectorId;
    }
}

class MonthPickerComponent {
    $: any;

    options = new MonthPickerOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: MonthPickerOption = new MonthPickerOption) => {
        var pickerOptions = this.$.extend({}, this.options, options);

        var dropdownMonth = this.$("#" + pickerOptions.selectorId);

        var maxDate = dropdownMonth.data("maxdate");

        dropdownMonth.monPickr({
            longMonthsInYear: dropdownMonth.data("longmonths-inyear"),
            maxDate: maxDate === "" ? dropdownMonth.data("currentdate") : maxDate,
            minDate: dropdownMonth.data("mindate"),
            month: new Date(dropdownMonth.data("dateinmonth")),

            fromDay: dropdownMonth.data("fromdate"),
            toDay: dropdownMonth.data("todate"),

            showDateRange: true,
            monthFormat: alphaReports.format.monthFormat,
            fromDayId: "ddl-fromday",
            toDayId: "ddl-today",
            fromDayLabel: "",
            toDayLabel: dropdownMonth.data("labelto"),
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            }
        });

        return dropdownMonth;
    };
    getDateRange = (dropdownMonth: any) => {
        return {
            FromDate: dropdownMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker),
            ToDate: dropdownMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker)
        };
    };
}

const monthPickerComponent = new MonthPickerComponent($);