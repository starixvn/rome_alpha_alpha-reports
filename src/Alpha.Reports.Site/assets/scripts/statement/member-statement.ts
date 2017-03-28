/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />

class MemberStatementView extends StatementView {
    reportTypeComponent: SingleSelectionDropdownComponent;
    transactionType = {
        WinLoss: 1,
        VoidCancelled: 2,
        Outstanding: 3,
        GrandPrize: 4
    };

    betListUrl: string;
    betListTitle: string;
    custId: string;
    custName: string;
    windowLocation: any;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any,
        reportTypeComponent: SingleSelectionDropdownComponent,
        windowLocation: any,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent);

        this.reportTypeComponent = reportTypeComponent;
        this.windowLocation = windowLocation;
    }

    public init() {
        this.$.blockUI();

        super.init();

        var reportTypeComponentOption = new SingleSelectionDropdownOption("dropdown-report-type", "ReportType", this.changeReportType);
        this.reportTypeComponent.render(reportTypeComponentOption);

        this.betListUrl = this.$("#BetListUrl").val();
        this.betListTitle = this.$("#BetListTitle").val();

        this.custId = this.$("#CustId").val();
        this.custName = this.$("#CustName").val();

        this.$.unblockUI();
    }

    public registerEvents() {
        super.registerEvents();

        let self = this;

        this.$tableReport.find(".col-void-amount").click(function () {
            self.viewBetList(self.$(this), self.transactionType.VoidCancelled);
        });
        this.$tableReport.find(".col-outstanding").click(function () {
            self.viewBetList(self.$(this), self.transactionType.Outstanding);
        });
        this.$tableReport.find(".col-win-loss").click(function () {
            self.viewBetList(self.$(this), self.transactionType.WinLoss);
        });
        this.$tableReport.find(".col-grand-prize").click(function () {
            self.viewBetList(self.$(this), self.transactionType.GrandPrize);
        });
    }
    changeReportType = () => {
        var url = this.reportTypeComponent.$dropdown.find("option:selected").data("url");
        this.windowLocation.href = url;
    };
    viewBetList = (element: JQuery, transactionType: number) => {
        let param = {
            custid: this.custId,
            fdate: element.data("transfer-date"),
            transtype: transactionType,
            type: "BLDailyST",
            username: this.custName
        };

        var url = this.betListUrl + "?" + jQuery.param(param);

        this.topWindow.popupManager.open(url, this.betListTitle, 900);
    };
}