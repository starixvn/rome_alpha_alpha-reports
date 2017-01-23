/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class RacingCommission {
    $: JQueryStatic;
    dateRangePicker: DateRangePickerComponent;

    constructor($: JQueryStatic, dateRangePicker: DateRangePickerComponent) {
        this.dateRangePicker = dateRangePicker;
    }

    init = () => {
        this.dateRangePicker.render();
    };

}

const racingCommission = new RacingCommission($, new DateRangePickerComponent($));

$(function () {
    racingCommission.init();
});