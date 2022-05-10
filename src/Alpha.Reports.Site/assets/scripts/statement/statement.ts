/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />

class StatementView extends BaseReport {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: ClientPagerComponent;
    topWindow: any;

    $tableReport: JQuery;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $exportExcelForm: JQuery;
    $statementDetailsURL: JQuery;

    custId: string;
    custName: string;

    popupStatementDetails: JQuery;
    popupBodyStatementDetails: JQuery;
    statementDetailView: StatementDetailView;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
        this.topWindow = topWindow;
    }

    public init() {
        this.cacheElements();
        this.dateRangePickerComponent.render();

        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
        this.initHelp();
    }
    public registerEvents() {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);

        let self = this;
        this.statementDetailView = new StatementDetailView(this.$, this.popupStatementDetails);
        this.$(".column-transfer-amount").on("click", function () {
            self.loadStatementDetail(self.$(this).data("transfer-date"));
        });
        this.popupStatementDetails.draggable({ handle: ".modal-header" });
    }
    public cacheElements() {
        super.cacheElements();

        this.$statementDetailsURL = this.$("#StatementDetailsURL");
        this.custId = this.$("#CustId").val();
        this.custName = this.$("#CustName").val();

        this.popupStatementDetails = this.$("#popup-statement-details");
        this.popupBodyStatementDetails = this.popupStatementDetails.find(".modal-body");
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };
    onSubmitReport = () => {
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !this.$reportForm.is(":hidden")));
        this.$.blockUI();
    };

    loadStatementDetail = (transferDate: string) => {
        let param = {
            TransferDate: transferDate,
            CustId: this.custId,
            CustName: this.custName
        };

        this.$.blockUI();
        this.$.get(
            this.$statementDetailsURL.val() + "?" + this.$.param(param),
            (html) => {
                this.$.unblockUI();
                this.popupBodyStatementDetails.html(html);
                this.statementDetailView.init();
                this.popupStatementDetails.modal({ show: true, backdrop: false });
            }).always(this.$.unblockUI);
    };
}