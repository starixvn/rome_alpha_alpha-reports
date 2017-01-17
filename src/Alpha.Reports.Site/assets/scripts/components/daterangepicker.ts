/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    selectorId: string;
    reportFormId: string;

    constructor(selectorId: string = "daterange-picker", reportFormId = "report-form") {
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
    }
}

class DateRangePickerComponent {
    $: JQueryStatic;

    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7
    };
    isCompleteInitDateRangePicker = false;
    $reportForm: JQuery;

    constructor($: JQueryStatic, options: DateRangePickerOption = new DateRangePickerOption()) {
        this.$ = $;
        this.options = this.$.extend({}, this.options, options);
    }

    render = () => {
        let dateRangePicker = this.$("#" + this.options.selectorId);
        let currentDate = dateRangePicker.data("currentdate");
        let minDate = dateRangePicker.data("mindate");
        let maxDate = dateRangePicker.data("maxdate");
        this.$reportForm = this.$("#" + this.options.reportFormId);

        dateRangePicker.nexDatePicker({
            datePickerFormat: {
                dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
            },
            selectedItem: dateRangePicker.data("selecteddaterange"),
            minDate: new Date(minDate),
            maxDate: maxDate === "" ? null : maxDate,
            currentDate: new Date(currentDate),
            onQuickChooseChange: (selectValue) => {
                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    this.$reportForm.submit();
                }
            }
        });

        this.$("#dropdown-daterange").multiselect({
            maxHeight: 300
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}