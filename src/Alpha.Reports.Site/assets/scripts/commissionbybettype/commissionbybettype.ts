/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class CommissionByBettype extends BaseReport {
    levelTypeDropdown: SingleSelectionDropdownComponent;
    dateRangePicker: DateRangePickerComponent;
    sorter: SortComponent;

    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $reportForm: JQuery;
    $isFilterVisible: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $tableReport: JQuery;
    $pageTitle: JQuery;

    constructor(
        $: JQueryStatic,
        levelTypeDropdown: SingleSelectionDropdownComponent,
        dateRangePicker: DateRangePickerComponent,
        sorter: SortComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);
        this.levelTypeDropdown = levelTypeDropdown;
        this.dateRangePicker = dateRangePicker;
        this.sorter = sorter;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();

        this.levelTypeDropdown.render(new SingleSelectionDropdownOption("dropdown-level-type"));
        this.dateRangePicker.render();

        const sortHeaders = {
            0: { sorter: "text" }
        };
        if (this.levelTypeDropdown.$dropdown.length) {
            sortHeaders[1] = { sorter: "text" }
        }
        this.sorter.render(new SortOption(
            this.$tableReport,
            parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            true,
            this.sorter.afterSort,
            sortHeaders));
        this.$tableReport.stickyTableHeaders();

        this.initHelp();
    };

    cacheDOMElements = () => {
        super.cacheElements();

        this.$excelIcon = this.$("#icon-excel");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
    };

    public registerEvents() {
        super.registerEvents();

        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$reportForm.submit(this.onSubmitReport);
    }

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
    const linkBuilderComponent = new LinkBuilderComponent($);
    const commissionByBettype = new CommissionByBettype(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent);
    commissionByBettype.init();
});