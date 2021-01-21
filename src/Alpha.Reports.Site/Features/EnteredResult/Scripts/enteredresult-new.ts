"use strict";

class EnterResultViewNew extends BaseHistoricReport {
    $isMobileView: JQuery;
    isMobileView: boolean;

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

        this.isMobileView = JSON.parse(this.$isMobileView.val().toLowerCase());
        this.dateRangePickerComponent.render();
        this.initDataTable();

        this.registerEvents();
        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        super.cacheDOMElements();

        this.$isMobileView = this.$("#IsMobileView");
    }

    initDataTable = () => {
        super.initDataTable(this.isMobileView
            ? { order: [] }
            : { responsive: false, columnDefs: [], order: [] });
    };

    renderDetail = (rowIdx: any, data: any) => {
        const toDate = `<li><b>${this.$('#ToDate').val()}</b></li>`;

        return data ?
            this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(toDate, data) :
            false;
    };

    drawCallback = () => {
        this.$tableReport.removeClass('hidden');
    };

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
        new DateRangePickerComponent($));
    enterResultViewNew.init();
});