class BaseHistoricReport extends BaseReportNew {
    $excelIcon: JQuery;
    $exportExcelForm: JQuery;
    $fromDate: JQuery;
    $historicReport: JQuery;
    $isHistoricReport: JQuery;
    $toDate: JQuery;
    $userSelectedDateRangeOption: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.dateRangePickerComponent = dateRangePickerComponent;
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

    public initDataTable(options = {}) {
        super.initDataTable(options);

        this.doAdjustDataTable();
    }

    public doAdjustDataTable() {
        const browserWidth = this.$(window).width(),
            deviceWidth = screen.width || window.outerWidth,
            columnCount = this.$dataTable.columns()[0].length;

        if (deviceWidth < 768 && browserWidth >= 768) {
            for (let i = 3; i < columnCount - 1; i++) {
                this.$(this.$dataTable.column(i).header()).removeClass("desktop min-tablet-l mobile").addClass("mobile");
            }
        }

        if (this.$tableReport.width() > browserWidth) {
            this.$tableReport.find("tbody tr td:first-child").css("white-space", "nowrap");
            this.$(this.$dataTable.column(2).header()).removeClass("all").addClass("min-tablet-l");
        }

        this.$dataTable.responsive.rebuild();
        this.$dataTable.responsive.recalc();
    }

    public registerEvents() {
        super.registerEvents();

        this.$excelIcon.on("click", () => this.onExportExcelEvent());
        this.$historicReport.on("click", () => this.onSubmitHistoricReportEvent());
        this.$reportForm.submit(event => this.onSubmitReportEvent(event));
        this.$fromDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
        this.$toDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
    }

    public doAdjustDatePickerCalendar = () => {
        if (alphaReports.screenSmallThan768() && !alphaReports.screenLandscapeMode()) {
            let width = this.$(window).width(),
                widget = this.$(".bootstrap-datetimepicker-widget.dropdown-menu");
            widget.css({ width });
        }
    };

    public checkReady = () => this.$("#IsHistoricReport").length > 0;

    public onExportExcelEvent() {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    }

    public onSubmitHistoricReportEvent() { }

    public onSubmitReportEvent(event: any) { }
}