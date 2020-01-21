/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" /> 
/// <reference path="../components/link-builder.ts" />

"use strict";

class DatePickerOption {
    constructor(
        public minDate: null,
        public allowInputToggle: boolean = false) {
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
}

class RiskAnalysisIPMatching {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

  

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent) {

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        this.$("SELECT").on("change", (event) => {
            this.$('#MemberCount').val((<HTMLInputElement>event.target).value);
        });

        let options = new DatePickerOption(this.$('#MinDate').val(), true);

        this.$('#dpkContainer').datepicker(options).data("DateTimePicker").maxDate(this.$('#Today').val());


        //.on("changeDate", function (event) {
        //    this.$('#WinlostDate').val((<HTMLInputElement>event.target).value);
        //})
    };
}

$(function () {
    const riskAnalysisIPMatching = new RiskAnalysisIPMatching(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, new LinkBuilderComponent($)));
    riskAnalysisIPMatching.init();
});