/// <reference path="../core.ts" />

"use strict";

class DateRangePickerOption {
    selectorId: string;

    constructor(selectorId: string = "daterange-picker") {
        this.selectorId = selectorId;
    }
}

class DateRangePickerComponent {
    $: any;

    options = new DateRangePickerOption();

    constructor($: any, options: DateRangePickerOption = new DateRangePickerOption()) {
        this.$ = $;
        this.options = this.$.extend({}, this.options, options);
    }

    render = () => {
        var dateRangePicker = this.$("#" + this.options.selectorId);
        var currentDate = dateRangePicker.data("currentdate");
        var minDate = dateRangePicker.data("mindate");
        var maxDate = dateRangePicker.data("maxdate");

        dateRangePicker.nexDatePicker({
            datePickerFormat: {
                dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
            },
            selectedItem: dateRangePicker.data("selecteddaterange"),
            minDate: new Date(minDate),
            maxDate: maxDate === "" ? null : maxDate,
            currentDate: new Date(currentDate)
        });

        this.$("#dropdown-daterange").multiselect({
            maxHeight: 300
        });

        return dateRangePicker;
    };
}