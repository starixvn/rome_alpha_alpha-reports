"use strict";
class BonusDetail {
    private $: JQueryStatic;
    private topWindow: any;
    private dateRangePickerComponent: DateRangePickerComponent;
    private pagerComponent: PagerComponent;
    private $reportForm: JQuery;
    private $pageTitle: JQuery;
    private $isFilterVisible: JQuery;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.$.blockUI();

        this.cacheElements()
        this.initComponents()
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption("daterange-picker", "report-form", null, () => this.$("#PageIndex").val("1")));

        const dropdownOptions = { numberDisplayed: 1 };
        this.$("#dropdown-statusId").multiselect(dropdownOptions);
        this.$("#dropdown-isComplained").multiselect(dropdownOptions);

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();

        this.$("#tbl-report").stickyTableHeaders();
    }

    public bindEvents() {
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(() => this.$("#PageIndex").val("1"));

        const $popupConfirmPayment = this.$("#popup-confirmPayment");
        if ($popupConfirmPayment.length) {
            new ConfirmPayment(
                this.$,
                new Alert(this.$),
                () => this.$reportForm.submit(),
                $popupConfirmPayment).init();
        }
    }

    onClickIconFilter = () => {
        const isHide = this.$reportForm.hasClass("hide");
        this.$isFilterVisible.val(isHide.toString());

        if (isHide) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    }

    onSubmitReport = () => {
        this.$.blockUI();
        return true;
    }
}

$(function () {
    new BonusDetail(
        $,
        window.top,
        new DateRangePickerComponent($),
        new PagerComponent($)).init();
});