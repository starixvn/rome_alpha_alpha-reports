/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class RacingCommission {
    $: JQueryStatic;
    dateRangePicker: DateRangePickerComponent;
    sorter: SortComponent;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $tableReport: any;

    constructor($: JQueryStatic, dateRangePicker: DateRangePickerComponent, sorter: SortComponent) {
        this.$ = $;
        this.dateRangePicker = dateRangePicker;
        this.sorter = sorter;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();

        this.dateRangePicker.render();

        this.sorter.render({
            sortContainer: this.$tableReport,
            canSort: parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            hasAllDataOnClient: true,
            headers: {
                0: { sorter: "text" }
            },
            afterSortFunction: function () { }
        });
    };

    cacheDOMElements = () => {
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report");
    }

    registerEvents = () => {
        this.$excelIcon.on("click", this.onExportExcelEvent);
    }

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
    };
}

const racingCommission = new RacingCommission($, new DateRangePickerComponent($), new SortComponent($));

$(function () {
    racingCommission.init();
});