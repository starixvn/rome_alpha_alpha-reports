"use strict";
class BetCreditHistory {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $txtMember: JQuery;
    $msgError: JQuery;
    $requiredMsgError: JQuery;
    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.$.blockUI();

        this.cacheElements();
        this.initComponents();
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$txtMember = this.$("#txt-member");
        this.$msgError = this.$("#msg-error");
        this.$requiredMsgError = this.$("#required-msg-error");
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption("daterange-picker", "report-form", null));

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();

        this.$("#tbl-report").stickyTableHeaders();
    }

    public bindEvents() {
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(() => this.$("#PageIndex").val("1"));

        this.$("#icon-excel").click(this.onExportExcel);
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

    onExportExcel = () => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationUsernameError();
            this.$.unblockUI();

            return;
        }

        this.hideValidationUsernameError();
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitReport = (e: any) => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationUsernameError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.hideValidationUsernameError();
        return true;
    }

    validateUsername = () => {
        if (this.$txtMember.val() === "") {
            return false;
        }

        return true;
    }

    showValidationUsernameError = () => {
        this.$msgError.addClass("hide");
        this.$requiredMsgError.removeClass("hide");
    }

    hideValidationUsernameError = () => this.$requiredMsgError.addClass("hide");
}

$(function () {
    const betCreditHistory = new BetCreditHistory(
        $,
        new DateRangePickerComponent($),
        new PagerComponent($)
    );
    betCreditHistory.init();
});