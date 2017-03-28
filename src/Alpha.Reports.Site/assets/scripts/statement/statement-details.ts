class StatementDetailView {
    $: JQueryStatic;

    $exportExcelForm: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();

        this.cacheElements();
        this.registerEvents();

        this.$.unblockUI();
    };
    cacheElements = () => {
        this.$exportExcelForm = this.$("#export-excel-form");
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