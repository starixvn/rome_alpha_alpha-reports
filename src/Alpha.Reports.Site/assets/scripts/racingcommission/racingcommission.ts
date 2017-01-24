/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class RacingCommission {
    $: JQueryStatic;
    dateRangePicker: DateRangePickerComponent;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    constructor($: JQueryStatic, dateRangePicker: DateRangePickerComponent) {
        this.$ = $;
        this.dateRangePicker = dateRangePicker;
    }

    init = () => {
        this.dateRangePicker.render();

        this.cacheDOMElements();
        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
    }

    registerEvents = () => {
        this.$excelIcon.on("click", this.onExportExcelEvent);
    }

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
    };
}

const racingCommission = new RacingCommission($, new DateRangePickerComponent($));

$(function () {
    racingCommission.init();
});