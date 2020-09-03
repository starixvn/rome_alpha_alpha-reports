"use strict";

class EnterResultViewNew extends BaseReportNew {
    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent) {
        super($, dateRangePickerComponent, null);
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        super.init();

        this.dateRangePickerComponent.render();
        this.initDataTable();

        this.registerEvents();
        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        super.cacheDOMElements();

        this.$tableReport = this.$("#tbl-enteredresult-report");
    };

    registerEvents = () => {
        super.registerEvents();

        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$historicReport.on("click", this.onSubmitHistoricReportEvent);
        this.$reportForm.submit(this.onSubmitNormalReportEvent);
    };

    initDataTable = () => {
        super.initDataTable({
            order: [[0, 'asc']]
        });
    };

    renderDetail = (rowIdx: any, data: any) => {
        const toDate = `<li><b>${this.$('#ToDate').val()}</b></li>`;

        return data ?
            this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(toDate, data) :
            false;
    };

    drawCallback = () => { };

    onSubmitNormalReportEvent = () => {
        this.setHiddenValue();
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val("");
        this.$toDate.val("");
        this.$isHistoricReport.val("true");
        this.setHiddenValue();
        this.$reportForm.trigger("submit");
    };

    setHiddenValue = () => {
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
        this.$.blockUI();
    };
}

$(function () {
    const enterResultViewNew = new EnterResultViewNew(
        $,
        new DateRangePickerComponent($));
    enterResultViewNew.init();
});