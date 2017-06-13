class StatementDetailView {
    $: JQueryStatic;

    $exportExcelForm: JQuery;
    $tableReport: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();

        this.cacheElements();
        this.registerEvents();

        this.$tableReport.stickyTableHeaders();

        this.$.unblockUI();
    };
    cacheElements = () => {
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$tableReport = this.$("#tbl-report");
    };
    registerEvents = () => {
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };
}

$(() => {
    const statementDetailView = new StatementDetailView($);

    statementDetailView.init();
});