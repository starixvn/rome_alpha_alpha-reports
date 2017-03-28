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
    $reportForm: JQuery;
    $isFilterVisible: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $tableReport: JQuery;

    constructor($: JQueryStatic, dateRangePicker: DateRangePickerComponent, sorter: SortComponent) {
        this.$ = $;
        this.dateRangePicker = dateRangePicker;
        this.sorter = sorter;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();

        this.dateRangePicker.render();

        this.sorter.render(new SortOption(
            this.$tableReport,
            parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            true,
            { 0: { sorter: "text" } },
            this.sorter.afterSort));
    };

    cacheDOMElements = () => {
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");

        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
    };

    registerEvents = () => {
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$reportForm.submit(this.onSubmitReport);
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();
        let params = this.getParams();

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$sortPropertyName.val(params.SortPropertyName);
    };
    getParams = () => {
        let sortInfo = this.sorter.getSortInfo();

        return {
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
}


$(function () {
    const racingCommission = new RacingCommission(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, new LinkBuilderComponent($)));
    racingCommission.init();
});