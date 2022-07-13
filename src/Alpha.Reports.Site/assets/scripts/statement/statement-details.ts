class StatementDetailView {
    $: JQueryStatic;
    $popup: JQuery;

    $exportExcelForm: JQuery;
    $tableReport: JQuery;

    constructor($: JQueryStatic, $popup: JQuery) {
        this.$ = $;
        this.$popup = $popup;
    }

    init = () => {
        this.cacheElements();
        this.registerEvents();

        this.$tableReport.stickyTableHeaders();
    };
    cacheElements = () => {
        this.$exportExcelForm = this.$popup.find("#export-excel-form");
        this.$tableReport = this.$popup.find("#tbl-report");
    };
    registerEvents = () => {
        this.$popup.find("#icon-excel").off("click").on("click", this.onExportExcel);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };
}
$(() => {
    const statementDetailView = new StatementDetailView($, $(document));

    statementDetailView.init();
});