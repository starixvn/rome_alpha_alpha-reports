"use strict";
class MoveStatementHistory {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    public init() {
        this.$.blockUI();

        this.cacheElements()
        this.initComponents()
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption("daterange-picker", "report-form", null));

        this.$("#tbl-report").stickyTableHeaders();

    }

    public bindEvents() {
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").click(this.onExportExcel);
    }

    onClickIconFilter = () => {
        const isHide = this.$reportForm.hasClass("hide");
        this.$isFilterVisible.val(isHide.toString());

        if (isHide) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitReport = () => {
        this.$.blockUI();
        return true;
    }
}

$(function () {
    const moveStatementHistory = new MoveStatementHistory(
        $,
        new DateRangePickerComponent($)
    );
    moveStatementHistory.init();
});