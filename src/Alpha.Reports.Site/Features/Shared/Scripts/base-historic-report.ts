class BaseHistoricReport extends BaseReportNew {
    $excelIcon: JQuery;
    $exportExcelForm: JQuery;
    $fromDate: JQuery;
    $historicReport: JQuery;
    $isHistoricReport: JQuery;
    $toDate: JQuery;
    $userSelectedDateRangeOption: JQuery;

    dateRangePickerNewComponent: DateRangePickerNewComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerNewComponent: DateRangePickerNewComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.dateRangePickerNewComponent = dateRangePickerNewComponent;
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$excelIcon = this.$(".icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDate = this.$("#from-date");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$toDate = this.$("#to-date");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
    }

    public registerEvents() {
        super.registerEvents();

        this.$excelIcon.on("click", () => this.onExportExcelEvent());
        this.$historicReport.on("click", () => this.onSubmitHistoricReportEvent());
        this.$reportForm.submit(event => this.onSubmitReportEvent(event));
    }

    public checkReady = () => this.$("#IsHistoricReport").length > 0;

    public onExportExcelEvent() {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    }

    public onSubmitHistoricReportEvent() { }

    public onSubmitReportEvent(event: any) { }
}