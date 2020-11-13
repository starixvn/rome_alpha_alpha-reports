"use strict";

class EnterResultViewNew extends BaseHistoricReport {
    constructor(
        $: JQueryStatic,
        dateRangePickerNewComponent: DateRangePickerNewComponent) {
        super($, dateRangePickerNewComponent, null);
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        super.init();

        this.dateRangePickerNewComponent.render();
        this.initDataTable();

        this.registerEvents();
        this.$.unblockUI();
    };

    initDataTable = () => {
        super.initDataTable({
            order: []
        });
    };

    renderDetail = (rowIdx: any, data: any) => {
        const toDate = `<li><b>${this.$('#ToDate').val()}</b></li>`;

        return data ?
            this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(toDate, data) :
            false;
    };

    drawCallback = () => { };

    onSubmitReportEvent = (event: any) => {
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
        new DateRangePickerNewComponent($));
    enterResultViewNew.init();
});