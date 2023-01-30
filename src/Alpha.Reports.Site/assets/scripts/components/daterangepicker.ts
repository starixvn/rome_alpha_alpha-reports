/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    constructor(
        public selectorId: string = "daterange-picker",
        public reportFormId: string = "report-form",
        public onChange: Function = null,
        public onBeforeQuickChooseChange: Function = null) {
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
    }
}

class DateRangePickerComponent {
    $: JQueryStatic;

    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7,
        olderThan3Months: 8
    };
    isCompleteInitDateRangePicker = false;
    $reportForm: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: DateRangePickerOption = new DateRangePickerOption()) => {
        this.options = this.$.extend({}, this.options, options);

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
            onSelectDatePicker: () => {
                if (this.options.onChange) {
                    this.options.onChange();
                }
            },
            onQuickChooseChange: (selectValue: number) => {
                this.options.onBeforeQuickChooseChange && this.options.onBeforeQuickChooseChange();

                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    if (selectValue === this.quickChooseType.olderThan3Months) {
                        this.$reportForm.find("input[id=IsHistoricReport]").val("true");
                        this.$reportForm.find("a[id=historic-report]").trigger("click");
                    }
                    else {
                        this.$reportForm.submit();
                    }
                }
            }
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}