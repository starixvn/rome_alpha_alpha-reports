/// <reference path="../core.ts" />

"use strict";

class MonthPickerComponent {
    options = {
        selectorId: "dropdown-month",
        longMonthsInYear: null
    };

    render = (options: any = {}) => {
        var pickerOptions = $.extend({}, this.options, options);

        var dropdownMonth = $("#" + pickerOptions.selectorId);

        var maxDate = dropdownMonth.data("maxdate");

        dropdownMonth.monPickr({
            longMonthsInYear: pickerOptions.longMonthsInYear,
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
}

const monthPickerComponent = new MonthPickerComponent();